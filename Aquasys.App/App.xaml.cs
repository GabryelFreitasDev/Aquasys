using Aquasys.App.MVVM.Views.Login;

namespace Aquasys.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Current!.UserAppTheme = AppTheme.Light;

            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
