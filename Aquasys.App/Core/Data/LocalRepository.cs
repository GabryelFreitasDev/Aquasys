using Aquasys.Core.Interfaces;
using SQLite;
using System.Linq.Expressions;

namespace Aquasys.App.Core.Data
{
    // Altere ILocalRepository<T> para herdar de IBaseRepository
    public interface ILocalRepository<T> : IBaseRepository where T : SyncableEntity, new()
    {
        // Os métodos específicos e fortemente tipados continuam aqui
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(object primaryKey);
        Task<bool> InsertAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(T item);

        // O método Upsert fortemente tipado
        Task UpsertAsync(T item);
    }

    // A implementação concreta e completa que usa o SQLite
    public class LocalRepository<T> : ILocalRepository<T> where T : SyncableEntity, new()
    {
        private readonly SQLiteAsyncConnection _database;

        public LocalRepository()
        {
            _database = DatabaseConnection.Instance;
            //InitializeAsync().Wait(); // Garante que a tabela seja criada
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

        public async Task<T> GetByIdAsync(object primaryKey)
        {
            return await _database.GetAsync<T>(primaryKey);
        }

        public async Task<bool> InsertAsync(T item)
        {
            if (item.GlobalId == Guid.Empty)
            {
                item.GlobalId = Guid.NewGuid();
            }
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

        // --- Métodos de Sincronização ---

        public Type GetEntityType() => typeof(T);

        public async Task<IEnumerable<SyncableEntity>> GetUnsyncedAsync()
        {
            return await _database.Table<T>().Where(x => !x.IsSynced).ToListAsync();
        }

        public async Task MarkAsSyncedAsync(List<Guid> globalIds)
        {
            var itemsToUpdate = await _database.Table<T>().Where(x => globalIds.Contains(x.GlobalId)).ToListAsync();
            foreach (var item in itemsToUpdate)
            {
                item.IsSynced = true;
                await _database.UpdateAsync(item);
            }
        }

        // A versão fortemente tipada do Upsert
        public async Task UpsertAsync(T item)
        {
            var existingItem = await _database.Table<T>().FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
            item.IsSynced = true;
            if (existingItem != null)
            {
                if (item.LastModifiedAt > existingItem.LastModifiedAt)
                {
                    var pkProp = typeof(T).GetProperties().FirstOrDefault(p => p.IsDefined(typeof(PrimaryKeyAttribute), true));
                    if (pkProp != null) pkProp.SetValue(item, pkProp.GetValue(existingItem));
                    await UpdateAsync(item);
                }
            }
            else
            {
                await InsertAsync(item);
            }
        }

        public Task UpsertAsync(SyncableEntity item)
        {
            return UpsertAsync((T)item);
        }
    }
}
