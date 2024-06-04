using Aquasys.MVVM.ViewModels.Vessel;

namespace Aquasys.MVVM.Views.Vessel;

public partial class VesselListPage : ContentPage
{
    private VesselListViewModel _vesselListViewModel;
    public VesselListPage()
	{
		InitializeComponent();
        _vesselListViewModel = new VesselListViewModel();
        BindingContext = _vesselListViewModel;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _vesselListViewModel.LoadVesselsAsync();
    }
}