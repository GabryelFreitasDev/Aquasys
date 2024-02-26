using Aquasys.MVVM.ViewModels.Vessel;

namespace Aquasys.MVVM.Views.Vessel;

public partial class VesselMainView : ContentPage
{
    VesselMainViewModel vesselMainViewModel;

    public VesselMainView()
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