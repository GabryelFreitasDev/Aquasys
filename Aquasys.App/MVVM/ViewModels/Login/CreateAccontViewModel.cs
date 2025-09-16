
using Aquasys.Core.Entities;
using Aquasys.App.MVVM.Models.Login;
using System.Windows.Input;
using Aquasys.App.Core.Data;

namespace Aquasys.App.MVVM.ViewModels.Login
{
    // Corrigido o nome da classe de "CreateAccontViewModel" para "CreateAccountViewModel"
    public class CreateAccountViewModel : BaseViewModels
    {
        private readonly ILocalRepository<User> _userRepository;
        public LoginModel Login { get; set; }
        public ICommand BtnCreateAccountClickCommand { get; private set; }

        public CreateAccountViewModel(ILocalRepository<User> userRepository)
        {
            _userRepository = userRepository;
            Login = new LoginModel();
            BtnCreateAccountClickCommand = new Command(async () => await CreateNewAccount());
        }

        private async Task CreateNewAccount()
        {
            if (!string.IsNullOrEmpty(Login.UserName) && !string.IsNullOrEmpty(Login.Password))
            {
                var findUser = await _userRepository.GetFilteredAsync(x => x.UserName == Login.UserName);
                if (findUser.Any())
                {
                    await Application.Current!.MainPage!.DisplayAlert("Informação", "Usuario já existente, realize o login.", "OK");
                    await Application.Current!.MainPage!.Navigation.PopAsync();
                }
                else
                {
                    User user = new User
                    {
                        UserName = Login.UserName,
                        Password = Login.Password,
                        Email = Login.Email ?? string.Empty
                    };

                    if (await _userRepository.InsertAsync(user))
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