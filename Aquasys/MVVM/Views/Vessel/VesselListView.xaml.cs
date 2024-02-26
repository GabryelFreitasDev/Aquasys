using Aquasys.MVVM.ViewModels.Vessel;

namespace Aquasys.MVVM.Views.Vessel;

public partial class VesselListView : ContentPage
{
    private VesselListViewModel _vesselListViewModel;
    public VesselListView()
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