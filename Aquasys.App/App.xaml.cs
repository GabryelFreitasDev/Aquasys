using Aquasys.App.Core.Data;
using Aquasys.App.MVVM.ViewModels.Login;
using Aquasys.App.MVVM.Views.Login;

namespace Aquasys.App
{
    public partial class App : Application
    {
        public App(LoginPage loginPage) 
        {
            InitializeComponent();
            MainPage = new NavigationPage(loginPage);
        }

        protected override async void OnStart()
        {
            base.OnStart();

            // 1. Define o tema da aplicação de forma segura.
            Current.UserAppTheme = AppTheme.Light;

            // 2. Chama o método para criar todas as tabelas do banco de dados local (SQLite).
            await InitializeDatabaseAsync();
        }

        /// <summary>
        /// Este método busca todos os repositórios que registramos no MauiProgram.cs
        /// e chama o método InitializeAsync de cada um para criar as tabelas no SQLite.
        /// </summary>
        private async Task InitializeDatabaseAsync()
        {
            // Acessa o provedor de serviços do MAUI para obter nossos repositórios.
            var serviceProvider = IPlatformApplication.Current.Services;

            // Encontra todos os tipos de entidade que precisam de uma tabela.
            var entityTypes = typeof(SyncableEntity).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(SyncableEntity)));

            foreach (var type in entityTypes)
            {
                // Para cada tipo, pega o repositório correspondente.
                var repoType = typeof(ILocalRepository<>).MakeGenericType(type);
                dynamic repository = serviceProvider.GetService(repoType);

                // E chama seu método para criar a tabela, se ela não existir.
                if (repository != null)
                {
                    await repository.InitializeAsync();
                }
            }
        }
    }
}
