using Aquasys.App.Core.Data;
using Aquasys.App.Core.Services;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Login;
using Aquasys.App.MVVM.Views.Login;
using Aquasys.Core.Entities;
using Aquasys.Core.Sync;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using Microsoft.Maui.Storage;

namespace Aquasys.App.MVVM.ViewModels.Login
{
    public partial class LoginViewModel : BaseViewModels
    {
        [ObservableProperty]
        private LoginModel _loginModel;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ValidateLoginCommand))]
        [NotifyCanExecuteChangedFor(nameof(CreateNewAccountCommand))]
        private bool _isBusy;

        [ObservableProperty]
        private string _statusMessage;

        private readonly ILocalRepository<User> _userRepository;
        private readonly ISyncService _syncService;
        private readonly IAuthService _authService;
        private readonly HttpClient _httpClient;

        public LoginViewModel(
            ILocalRepository<User> userRepository,
            ISyncService syncService,
            IAuthService authService,
            HttpClient httpClient)
        {
            _loginModel = new();
            _userRepository = userRepository;
            _syncService = syncService;
            _authService = authService;
            _httpClient = httpClient;
        }

        public override async Task OnAppearing()
        {
            if (IsLoadedViewModel) return;
            IsLoadedViewModel = true;

            // Carrega IP/Porta salvos e já configura o HttpClient
            await LoadApiSettingsAsync();

            await AutoLoginAsync();
        }

        private async Task LoadApiSettingsAsync()
        {
            try
            {
                var ip = await SecureStorage.Default.GetAsync("ApiIp");
                var port = await SecureStorage.Default.GetAsync("ApiPort");

                if (!string.IsNullOrWhiteSpace(ip) && !string.IsNullOrWhiteSpace(port))
                {
                    LoginModel.ApiIp = ip;
                    LoginModel.ApiPort = port;
                    ConfigureApiBaseAddress();
                }
                else
                {
                    // Se quiser, define um padrão na primeira execução:
                    // LoginModel.ApiIp = "10.0.2.2";
                    // LoginModel.ApiPort = "5270";
                    // ConfigureApiBaseAddress();
                }
            }
            catch
            {
            }
        }

        private bool ConfigureApiBaseAddress()
        {
            if (string.IsNullOrWhiteSpace(LoginModel.ApiIp) ||
                string.IsNullOrWhiteSpace(LoginModel.ApiPort))
                return false;

            try
            {
                _httpClient.BaseAddress = new Uri($"http://{LoginModel.ApiIp}:{LoginModel.ApiPort}");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task AutoLoginAsync()
        {
            try
            {
                var userRememberList = await _userRepository.GetFilteredAsync(x => x.RememberMe == true);
                var userRemember = userRememberList.FirstOrDefault();

                if (userRemember != null && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    await AuthenticateAndSync(userRemember);
                else if (userRemember != null)
                    Application.Current!.MainPage = new AppShell();
            }
            catch
            {
                return;
            }
        }

        [RelayCommand(CanExecute = nameof(CanExecuteCommands))]
        private async Task ValidateLogin()
        {
            if (string.IsNullOrWhiteSpace(LoginModel.ApiIp) ||
                string.IsNullOrWhiteSpace(LoginModel.ApiPort))
            {
                await Application.Current!.MainPage!.DisplayAlert("Erro", "Informe IP e Porta da API.", "OK");
                return;
            }

            try
            {
                _httpClient.BaseAddress = new Uri($"http://{LoginModel.ApiIp}:{LoginModel.ApiPort}");
            }
            catch
            {
                await Application.Current!.MainPage!.DisplayAlert("Erro", "IP ou porta inválidos.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(LoginModel.userName) || string.IsNullOrEmpty(LoginModel.Password))
            {
                await Application.Current!.MainPage!.DisplayAlert("Alerta", "Preencha os campos corretamente.", "OK");
                return;
            }

            if (IsBusy) return;

            try
            {
                IsBusy = true;
                StatusMessage = "Autenticando...";

                User? authenticatedUser = null;

                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    authenticatedUser = await _authService.LoginAsync(LoginModel.UserName, LoginModel.Password);
                    if (authenticatedUser != null)
                    {
                        authenticatedUser.RememberMe = LoginModel.RememberMe;
                        await _userRepository.UpsertAsync(authenticatedUser);

                        // (Opcional) salvar IP/Porta
                        await SecureStorage.Default.SetAsync("ApiIp", LoginModel.ApiIp);
                        await SecureStorage.Default.SetAsync("ApiPort", LoginModel.ApiPort);
                    }
                }

                await AuthenticateAndSync(authenticatedUser);
            }
            catch (Exception ex)
            {
                await Application.Current!.MainPage!.DisplayAlert("Erro", $"Ocorreu um erro inesperado: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                StatusMessage = string.Empty;
            }
        }


        private async Task AuthenticateAndSync(User? user)
        {
            if (user != null)
            {
                if (user.RememberMe != LoginModel.RememberMe)
                {
                    user.RememberMe = LoginModel.RememberMe;
                    await _userRepository.UpdateAsync(user);
                }

                StatusMessage = "Sincronizando dados, por favor aguarde...";
                var progress = new Progress<string>(msg => StatusMessage = msg);
                await _syncService.SynchronizeAsync();

                new ContextUtils(user);
                Application.Current!.MainPage = new AppShell();
            }
            else
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    await Application.Current!.MainPage!.DisplayAlert("Alerta", "Usuário ou senha incorretos, tente novamente.", "OK");
                else
                    await Application.Current!.MainPage!.DisplayAlert("Alerta", "Conecte-se á internet", "OK");
            }
        }

        [RelayCommand]
        private async Task CreateNewAccount()
        {
            await Shell.Current.GoToAsync(nameof(CreateAccountPage));
        }

        private bool CanExecuteCommands() => !IsBusy;
    }
}
