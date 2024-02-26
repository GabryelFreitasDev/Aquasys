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
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(CreateAccountView), typeof(CreateAccountView));
        #endregion

        #region Vessel
            Routing.RegisterRoute(nameof(VesselListView), typeof(VesselListView));
            Routing.RegisterRoute(nameof(VesselMainView), typeof(VesselMainView));
            Routing.RegisterRoute(nameof(VesselCargoHoldInspectionView), typeof(VesselCargoHoldInspectionView));
        #endregion

        Routing.RegisterRoute(nameof(MainPageView), typeof(MainPageView)); //Queima ou não queima?
    }
}