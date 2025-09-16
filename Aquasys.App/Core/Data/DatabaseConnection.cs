using SQLite;

namespace Aquasys.App.Core.Data
{
    public class DatabaseConnection
    {
        // Lazy<T> garante que a conexão só será criada quando for usada pela primeira vez
        private static readonly Lazy<SQLiteAsyncConnection> _lazyInitializer = new(() =>
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = Path.Combine(basePath, "Aquasys.db3");
            return new SQLiteAsyncConnection(dbPath);
        });

        public static SQLiteAsyncConnection Instance => _lazyInitializer.Value;
    }
}
