using Aquasys.Core.BO;
using Aquasys.Core.Entities;
using Aquasys.Core.Utils;
using Aquasys.MVVM.Models.Login;
using Aquasys.MVVM.Views.Login;
using Aquasys.MVVM.Views.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Aquasys.MVVM.ViewModels.Login
{
    public partial class LoginViewModel : BaseViewModels
    {
        [ObservableProperty]
        public LoginModel loginModel;

        [ObservableProperty]
        private bool loadingInitInstanceDB;

        private UserBO userBO = new UserBO();

        public ICommand BtnLoginClickCommand { get; private set; }
        public ICommand BtnCreateAccountClickCommand { get; private set; }
        //public ICommand ChkRememberMeCommand { get; private set; }

        public LoginViewModel()
        {
            LoginModel = new();
            BtnLoginClickCommand = new Command(async () => await ValidateLogin());
            BtnCreateAccountClickCommand = new Command(async () => await CreateNewAccount());
            //ChkRememberMeCommand = new Command(RememberMe);
        }

        public override async void OnAppearing()
        {
            if (IsLoadedViewModel)
                return;

            IsLoadedViewModel = true;

            base.OnAppearing();
            var userRemember = await new UserBO().GetFilteredAsync<User>(x => x.RememberMe == true);

            if (userRemember?.Any() ?? false)
            {
                new ContextUtils(userRemember?.FirstOrDefault() ?? new());
                LoadingInitInstanceDB = false;
                Application.Current!.MainPage = new AppShell();
            }
            LoadingInitInstanceDB = true;
            //else
            //await Application.Current!.MainPage!.Navigation.PushAsync(new LoginView());
        }

        private async Task ValidateLogin()
        {
            if (!string.IsNullOrEmpty(LoginModel.UserName) && !string.IsNullOrEmpty(LoginModel.Password))
            {
                var user = await userBO.GetFilteredAsync<User>(x => x.UserName == LoginModel.UserName && x.Password == LoginModel.Password);
                if (user?.Any() ?? false)
                {
                    new ContextUtils(user?.FirstOrDefault() ?? new());
                    if(ContextUtils.ContextUser.RememberMe != LoginModel.RememberMe)
                    {
                        ContextUtils.ContextUser.RememberMe = LoginModel.RememberMe;
                        await userBO.UpdateAsync(ContextUtils.ContextUser);
                    }
                    Application.Current!.MainPage = new AppShell();
                }
                else
                    await Application.Current!.MainPage!.DisplayAlert("Alerta", "Usuário ou senha incorretos, tente novamente.", "OK");
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert("Alerta", "Preencha os campos corretamente.", "OK");
            }

        }

        //public void RememberMe(object isChecked)
        //{
        //    Login.RememberMe = Convert.ToBoolean(isChecked);
        //}

        private async Task CreateNewAccount()
        {
            await Application.Current!.MainPage!.Navigation.PushAsync(new CreateAccountPage());
        }
    }
}
