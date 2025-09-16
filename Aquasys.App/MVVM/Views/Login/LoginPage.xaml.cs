using Aquasys.App.MVVM.ViewModels.Login;

namespace Aquasys.App.MVVM.Views.Login;

public partial class LoginPage : BasePages
{
    public LoginPage(LoginViewModel loginViewModel)
    {
        InitializeComponent();
        BindingContext = loginViewModel;
    }
}