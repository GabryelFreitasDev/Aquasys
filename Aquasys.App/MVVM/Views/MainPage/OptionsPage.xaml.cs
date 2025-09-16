using Aquasys.App.MVVM.ViewModels.MainPage;

namespace Aquasys.App.MVVM.Views.MainPage;

public partial class OptionsPage : BasePages
{
    public OptionsPage(OptionsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}