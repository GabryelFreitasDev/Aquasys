using Aquasys.App.Core.Data;
using Aquasys.App.Core.Services;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Login;
using Aquasys.App.MVVM.Views.Login;
using Aquasys.Core.Entities;
using Aquasys.Core.Sync;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        public LoginViewModel(
            ILocalRepository<User> userRepository,
            ISyncService syncService,
            IAuthService authService) 
        {
            _loginModel = new();
            _userRepository = userRepository;
            _syncService = syncService;
            _authService = authService;
        }

        public override async Task OnAppearing()
        {
            if (IsLoadedViewModel) return;
            IsLoadedViewModel = true;

            await AutoLoginAsync();
        }

        private async Task AutoLoginAsync()
        {
            try
            {
                var userRememberList = await _userRepository.GetFilteredAsync(x => x.RememberMe == true);
                var userRemember = userRememberList.FirstOrDefault();

                if (userRemember != null)
                    await AuthenticateAndSync(userRemember);
            }
            catch {
                return;
            }
        }

        [RelayCommand(CanExecute = nameof(CanExecuteCommands))]
        private async Task ValidateLogin()
        {
            //LoginModel.UserName = "inspector.joao";
            //LoginModel.Password = "Password123!";

            if (string.IsNullOrEmpty(LoginModel.UserName) || string.IsNullOrEmpty(LoginModel.Password))
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