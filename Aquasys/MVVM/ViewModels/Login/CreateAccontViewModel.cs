using Aquasys.Core.BO;
using Aquasys.Core.Entities;
using Aquasys.MVVM.Models.Login;
using System.Windows.Input;

namespace Aquasys.MVVM.ViewModels.Login
{
    public class CreateAccontViewModel : BaseViewModel
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
                    await Shell.Current.DisplayAlert("Informação", "Usuario já existente, realize o login.", "OK");
                    await Application.Current!.MainPage!.Navigation.PopAsync();
                }
                else
                {
                    User user = new User();
                    user.UserName = Login.UserName;
                    user.Password = Login.Password;
                    user.Email = Login.Email ?? string.Empty;

                    if (await userBO.InsertAsync(user))
                        await Application.Current!.MainPage!.Navigation.PopAsync();
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Alerta", "Preencha os campos corretamente.", "OK");
            }
        }
    }
}
