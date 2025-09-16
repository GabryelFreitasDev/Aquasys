using Aquasys.App.MVVM.ViewModels.Vessel.Tabs;

namespace Aquasys.App.MVVM.Views.Vessel.Tabs;

public partial class VesselRegistrationTabPage : ContentView {
    public VesselRegistrationTabPage(VesselRegistrationTabViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}