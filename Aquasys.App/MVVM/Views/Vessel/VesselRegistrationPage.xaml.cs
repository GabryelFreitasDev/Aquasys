using Aquasys.App.MVVM.ViewModels.Vessel;

namespace Aquasys.App.MVVM.Views.Vessel;

public partial class VesselRegistrationPage : BasePages
{
    public VesselRegistrationPage(VesselRegistrationViewModel vesselRegistrationViewModel)
    {
        InitializeComponent();
        BindingContext = vesselRegistrationViewModel;
    }
}