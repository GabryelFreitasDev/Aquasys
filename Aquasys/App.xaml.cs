using Aquasys.MVVM.Views.Login;

namespace Aquasys
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Current!.UserAppTheme = AppTheme.Light;

            MainPage = new NavigationPage(new LoginView());
        }
    }
}
