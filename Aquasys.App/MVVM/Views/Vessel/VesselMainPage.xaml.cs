using Aquasys.App.MVVM.ViewModels.Vessel;

namespace Aquasys.App.MVVM.Views.Vessel;

public partial class VesselMainPage : BasePages
{
    public VesselMainPage(VesselMainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}