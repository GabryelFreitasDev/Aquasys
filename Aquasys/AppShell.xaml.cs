using Aquasys.MVVM.Views.Login;
using Aquasys.MVVM.Views.MainPage;
using Aquasys.MVVM.Views.Vessel;
using CommunityToolkit.Mvvm.Input;

namespace Aquasys;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
    }

    private void RegisterRoutes()
    {
        #region Login
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(CreateAccountPage), typeof(CreateAccountPage));
        #endregion

        #region Vessel
            Routing.RegisterRoute(nameof(VesselListPage), typeof(VesselListPage));
            Routing.RegisterRoute(nameof(VesselMainPage), typeof(VesselMainPage));
            Routing.RegisterRoute(nameof(VesselImagePage), typeof(VesselImagePage));
        #endregion

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage)); //Queima ou não queima?
    }
}