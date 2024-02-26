using SQLite;
using System.Linq.Expressions;

namespace Aquasys.Core.Data
{
    public class DatabaseContext : IAsyncDisposable
    {
        private const string DatabaseName = "database.db3";
        private static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseName);

        private SQLiteAsyncConnection? connection;
        private SQLiteAsyncConnection Database =>
            connection ??= new SQLiteAsyncConnection(DatabasePath,
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);
        public async Task CreateTableIfNotExistsAsync<TTable>() where TTable : class, new()
        {
            await Database.CreateTableAsync<TTable>();
        }

        public async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable : class, new()
        {
            await CreateTableIfNotExistsAsync<TTable>();
            return Database.Table<TTable>();
        }

        public async Task<TResult> Execute<TTable, TResult>(Func<Task<TResult>> action) where TTable : class, new()
        {
            await CreateTableIfNotExistsAsync<TTable>();
            return await action();
        }

        public async Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new()
        {
            var table = await GetTableAsync<TTable>();
            return await table.ToListAsync();
        }

        public async Task<TTable> GetByIdAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            return await Execute<TTable, TTable>(async () => await Database.GetAsync<TTable>(primaryKey));
        }

        public async Task<IEnumerable<TTable>> GetFilteredAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new()
        {
            var table = await GetTableAsync<TTable>();
            return await table.Where(predicate).ToListAsync();
        }

        public async Task<bool> InsertAsync<TTable>(TTable item) where TTable : class, new()
        {
            return await Execute<TTable, bool>(async () => await Database.InsertAsync(item) > 0);
        }

        public async Task<bool> UpdateAsync<TTable>(TTable item) where TTable : class, new()
        {
            return await Execute<TTable, bool>(async () => await Database.UpdateAsync(item) > 0);
        }

        public async Task<bool> DeleteAsync<TTable>(TTable item) where TTable : class, new()
        {
            return await Execute<TTable, bool>(async () => await Database.DeleteAsync(item) > 0);
        }

        public async Task<bool> DeleteByIdAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            return await Execute<TTable, bool>(async () => await Database.DeleteAsync(primaryKey) > 0);
        }

        public async ValueTask DisposeAsync()
        {
            if (connection is not null)
                await connection.CloseAsync();
        }
    }
}
