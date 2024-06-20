using Aquasys.MVVM.Views.Login;
using Aquasys.MVVM.Views.MainPage;
using Aquasys.MVVM.Views.Vessel;

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
            Routing.RegisterRoute(nameof(HoldPage), typeof(HoldPage));
            Routing.RegisterRoute(nameof(HoldInspectionPage), typeof(HoldInspectionPage));
            Routing.RegisterRoute(nameof(HoldImagePage), typeof(HoldImagePage));
        #endregion

        Routing.RegisterRoute(nameof(OptionsPage), typeof(OptionsPage));

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage)); //Queima ou não queima?
    }
}