using Aquasys.Core.BO;
using Aquasys.Core.Entities;
using Aquasys.MVVM.Models.Login;
using Aquasys.MVVM.Views.Login;
using Aquasys.MVVM.Views.Vessel;
using Aquasys.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Aquasys.MVVM.ViewModels.Login
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        public LoginModel _login = new();

        private UserBO userBO = new UserBO();

        public ICommand BtnLoginClickCommand { get; private set; }
        public ICommand BtnCreateAccountClickCommand { get; private set; }
        public ICommand ChkRememberMeCommand { get; private set; }

        public LoginViewModel()
        {
            BtnLoginClickCommand = new Command(async () => await ValidateLogin());
            BtnCreateAccountClickCommand = new Command(async () => await CreateNewAccount());
            ChkRememberMeCommand = new Command(RememberMe);
        }

        private async Task ValidateLogin()
        {
            if (!string.IsNullOrEmpty(Login.UserName) && !string.IsNullOrEmpty(Login.Password))
            {
                var user = await userBO.GetFilteredAsync<User>(x => x.UserName == Login.UserName && x.Password == Login.Password);
                if (user?.Any() ?? false)
                {
                    new ContextUtils(user?.FirstOrDefault() ?? new());
                    if(ContextUtils.ContextUser.RememberMe != Login.RememberMe)
                    {
                        ContextUtils.ContextUser.RememberMe = Login.RememberMe;
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

        public void RememberMe(object isChecked)
        {
            Login.RememberMe = Convert.ToBoolean(isChecked);
        }

        private async Task CreateNewAccount()
        {
            await Application.Current!.MainPage!.Navigation.PushAsync(new CreateAccountView());
        }
    }
}
