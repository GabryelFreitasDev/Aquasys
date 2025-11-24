using Aquasys.Core.Interfaces;
using SQLite;
using System.Linq.Expressions;

namespace Aquasys.App.Core.Data
{
    public interface ILocalRepository<T> : IBaseRepository where T : SyncableEntity, new()
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(object primaryKey);
        Task<bool> InsertAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(T item);

        Task UpsertAsync(T item, bool fromServer = false);
    }

    public class LocalRepository<T> : ILocalRepository<T> where T : SyncableEntity, new()
    {
        private readonly SQLiteAsyncConnection _database;

        public LocalRepository()
        {
            _database = DatabaseConnection.Instance;

            Task.Run(async () => await InitializeAsync());
        }

        public async Task InitializeAsync()
        {
            await _database.CreateTableAsync<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _database.Table<T>().ToListAsync();
        }

        public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate)
        {
            return await _database.Table<T>().Where(predicate).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object primaryKey)
        {
            try
            {
                return await _database.GetAsync<T>(primaryKey);
            }
            catch
            {
                return default;
            }
        }

        public async Task<bool> InsertAsync(T item)
        {
            if (item.GlobalId == Guid.Empty)
                item.GlobalId = Guid.NewGuid();

            item.LastModifiedAt = DateTime.UtcNow;
            item.IsSynced = false;

            return await _database.InsertAsync(item) > 0;
        }

        public async Task<bool> UpdateAsync(T item)
        {
            item.LastModifiedAt = DateTime.UtcNow;
            item.IsSynced = false;

            return await _database.UpdateAsync(item) > 0;
        }

        public async Task<bool> DeleteAsync(T item)
        {
            return await _database.DeleteAsync(item) > 0;
        }

        // Usado pelas telas do app (dados locais)
        public async Task UpsertLocalAsync(T item)
        {
            // 1) Tenta encontrar o registro existente por PK OU por GlobalId
            var type = typeof(T);
            var pkProp = type.GetProperties()
                .FirstOrDefault(p => p.IsDefined(typeof(PrimaryKeyAttribute), true));

            object? pkValue = pkProp?.GetValue(item);
            T? existing = null;

            // Se veio com PK preenchido (ex: IDVesselImage != 0), tenta por PK primeiro
            if (pkValue != null && !IsDefaultValue(pkValue))
            {
                try
                {
                    existing = await _database.FindAsync<T>(pkValue);
                }
                catch
                {
                    // ignore se não achar
                }
            }

            // Se não achou por PK e tem GlobalId, tenta por GlobalId
            if (existing == null && item.GlobalId != Guid.Empty)
            {
                existing = await _database.Table<T>()
                    .FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
            }

            // Se achou registro, mas o item veio sem GlobalId, reaproveita o existente
            if (existing != null && item.GlobalId == Guid.Empty)
            {
                item.GlobalId = existing.GlobalId;
            }

            // Se ainda não tem GlobalId (registro realmente novo), gera um
            if (item.GlobalId == Guid.Empty)
            {
                item.GlobalId = Guid.NewGuid();
            }

            // Campos de controle para alterações locais
            item.LastModifiedAt = DateTime.UtcNow;
            item.IsSynced = false;

            if (existing != null)
            {
                // Garante que o PK do item seja o mesmo do registro existente
                pkValue?.CopyTo(item);

                await _database.UpdateAsync(item);
            }
            else
            {
                await _database.InsertAsync(item);
            }
        }

        // helper pra saber se o valor do PK é "zero"/default
        private static bool IsDefaultValue(object value)
        {
            var type = value.GetType();

            if (type == typeof(int)) return (int)value == 0;
            if (type == typeof(long)) return (long)value == 0L;
            if (type == typeof(short)) return (short)value == 0;
            if (type == typeof(Guid)) return (Guid)value == Guid.Empty;

            // fallback generico
            return value.Equals(Activator.CreateInstance(type)!);
        }


        // Usado pelo PULL (dados vindos do servidor)
        public async Task UpsertFromServerAsync(T item)
        {
            var existing = await _database.Table<T>()
                .FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);

            // dados vindo do servidor já estão sincronizados
            item.IsSynced = true;

            if (existing != null)
            {
                // regra LWW pelo LastModifiedAt
                if (item.LastModifiedAt > existing.LastModifiedAt)
                {
                    // importante: mantém o ID que já está no SQLite
                    var pkProp = typeof(T).GetProperties()
                        .FirstOrDefault(p => p.IsDefined(typeof(PrimaryKeyAttribute), true));
                    if (pkProp != null)
                        pkProp.SetValue(item, pkProp.GetValue(existing));

                    await _database.UpdateAsync(item);
                }
            }
            else
            {
                // aqui é onde queremos respeitar o ID que veio do servidor
                // não mexa em ID, não mexa em GlobalId, nem em LastModifiedAt
                await _database.InsertAsync(item);
            }
        }



        public Type GetEntityType() => typeof(T);

        public async Task<IEnumerable<SyncableEntity>> GetUnsyncedAsync()
        {
            var list = await _database.Table<T>()
                                      .Where(x => !x.IsSynced)
                                      .ToListAsync();
            return list.Cast<SyncableEntity>();
        }

        public async Task MarkAsSyncedAsync(List<Guid> globalIds)
        {
            if (globalIds == null || globalIds.Count == 0)
                return;

            var itemsToUpdate = await _database.Table<T>()
                                               .Where(x => globalIds.Contains(x.GlobalId))
                                               .ToListAsync();

            foreach (var item in itemsToUpdate)
            {
                item.IsSynced = true;
                await _database.UpdateAsync(item);
            }
        }

        public Task UpsertAsync(SyncableEntity item, bool fromServer = false)
        {
            if (!fromServer)
                return UpsertLocalAsync((T)item);
            else
                return UpsertFromServerAsync((T)item);
        }

        public Task UpsertAsync(T item, bool fromServer = false)
        {
            if (!fromServer)
                return UpsertLocalAsync((T)item);
            else
                return UpsertFromServerAsync((T)item);
        }
    }

    internal static class PrimaryKeyExtensions
    {
        public static object? GetPrimaryKeyValue(this object instance)
        {
            var pk = instance.GetType().GetProperties()
                .FirstOrDefault(p => p.IsDefined(typeof(PrimaryKeyAttribute), true));

            return pk?.GetValue(instance);
        }

        public static void CopyTo(this object? pkValue, object target)
        {
            if (pkValue == null) return;

            var pk = target.GetType().GetProperties()
                .FirstOrDefault(p => p.IsDefined(typeof(PrimaryKeyAttribute), true));

            pk?.SetValue(target, pkValue);
        }
    }
}
