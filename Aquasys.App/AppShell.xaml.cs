using Aquasys.App.MVVM.Views.Login;
using Aquasys.App.MVVM.Views.MainPage;
using Aquasys.App.MVVM.Views.Vessel;
using Aquasys.App.MVVM.Views.Vessel.Tabs;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
namespace Aquasys.App;

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
            Routing.RegisterRoute(nameof(VesselRegistrationPage), typeof(VesselRegistrationPage));
            //Routing.RegisterRoute(nameof(VesselHoldRegistrationTabPage), typeof(VesselHoldRegistrationTabPage));
            Routing.RegisterRoute(nameof(HoldPage), typeof(HoldPage));
            Routing.RegisterRoute(nameof(HoldInspectionPage), typeof(HoldInspectionPage));
            Routing.RegisterRoute(nameof(HoldImagePage), typeof(HoldImagePage));
        #endregion

        Routing.RegisterRoute(nameof(OptionsPage), typeof(OptionsPage));

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage)); //Queima ou não queima?
    }
}