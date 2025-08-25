using Aquasys.App.Core.BO;
using Aquasys.App.Core.Entities;
using Aquasys.App.MVVM.Models.Login;
using System.Windows.Input;

namespace Aquasys.App.MVVM.ViewModels.Login
{
    public class CreateAccontViewModel : BaseViewModels
    {
        private UserBO userBO = new UserBO();
        public LoginModel Login { get; set; }
        public ICommand BtnCreateAccountClickCommand { get; private set; }

        public CreateAccontViewModel()
        {
            Login = new LoginModel();

            BtnCreateAccountClickCommand = new Command(async () => await CreateNewAccount());
        }

        private async Task CreateNewAccount()
        {
            if(!string.IsNullOrEmpty(Login.UserName) && !string.IsNullOrEmpty(Login.Password))
            {
                var findUser = await userBO.GetFilteredAsync<User>(x => x.UserName == Login.UserName && x.Password == Login.Password);
                if (findUser.Any())
                {
                    await Application.Current!.MainPage!.DisplayAlert("Informação", "Usuario já existente, realize o login.", "OK");
                    await Application.Current!.MainPage!.Navigation.PopAsync();
                }
                else
                {
                    User user = new User();
                    user.UserName = Login.UserName;
                    user.Password = Login.Password;
                    user.Email = Login.Email ?? string.Empty;

                    if (await userBO.InsertAsync(user))
                    {
                        await Application.Current!.MainPage!.DisplayAlert("Informação", "Cadastro realizado com sucesso! Realize o login.", "OK");
                        await Application.Current!.MainPage!.Navigation.PopAsync();
                    }
                }
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert("Alerta", "Preencha os campos corretamente.", "OK");
            }
        }
    }
}
