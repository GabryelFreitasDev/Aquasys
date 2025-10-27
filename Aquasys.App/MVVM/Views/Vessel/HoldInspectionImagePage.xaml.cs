using Aquasys.App.MVVM.ViewModels.Vessel;

namespace Aquasys.App.MVVM.Views.Vessel;

public partial class HoldInspectionImagePage : BasePages
{
    public HoldInspectionImagePage(HoldInspectionImageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}