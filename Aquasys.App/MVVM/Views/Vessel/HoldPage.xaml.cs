using Aquasys.App.MVVM.ViewModels.Vessel;

namespace Aquasys.App.MVVM.Views.Vessel;

public partial class HoldPage : BasePages
{
    public HoldPage(HoldViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}