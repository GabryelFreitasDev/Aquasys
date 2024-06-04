using Aquasys.MVVM.ViewModels.Vessel;

namespace Aquasys.MVVM.Views.Vessel;

public partial class VesselMainPage : ContentPage
{
    VesselMainViewModel vesselMainViewModel;

    public VesselMainPage()
	{
		InitializeComponent();
        vesselMainViewModel = new VesselMainViewModel();
        BindingContext = vesselMainViewModel;
	}

    protected override void OnAppearing()
    {
        vesselMainViewModel.LoadTabs();
        base.OnAppearing();
    }
}