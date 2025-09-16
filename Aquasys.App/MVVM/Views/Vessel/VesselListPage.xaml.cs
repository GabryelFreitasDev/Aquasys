using Aquasys.App.MVVM.ViewModels.Vessel;

namespace Aquasys.App.MVVM.Views.Vessel;

public partial class VesselListPage : BasePages
{
    public VesselListPage(VesselListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}