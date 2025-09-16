using Aquasys.App.MVVM.ViewModels.Vessel;

namespace Aquasys.App.MVVM.Views.Vessel;

public partial class HoldInspectionPage : BasePages
{
    public HoldInspectionPage(HoldInspectionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}