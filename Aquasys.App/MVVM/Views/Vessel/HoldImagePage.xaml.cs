using Aquasys.App.MVVM.ViewModels.Vessel;

namespace Aquasys.App.MVVM.Views.Vessel;

public partial class HoldImagePage : BasePages
{
    public HoldImagePage(HoldImageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}