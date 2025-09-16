using Aquasys.App.MVVM.ViewModels.Vessel.Tabs;

namespace Aquasys.App.MVVM.Views.Vessel.Tabs;

public partial class VesselInspectionRegistrationTabPage : ContentView {
    public VesselInspectionRegistrationTabPage(VesselInspectionRegistrationTabViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}