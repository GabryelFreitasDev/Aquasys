using CommunityToolkit.Mvvm.ComponentModel;

namespace Aquasys.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselMainViewModel : BaseViewModel
    {
        [ObservableProperty]
        public VesselRegistrationTabViewModel vesselRegistrationTabViewModel;

        public VesselMainViewModel()
        {
            vesselRegistrationTabViewModel = new VesselRegistrationTabViewModel();
        }

        public void LoadTabs()
        {
            if (string.IsNullOrEmpty(Id))
                return;

            VesselRegistrationTabViewModel.IdVessel = Convert.ToInt64(Id);
            VesselRegistrationTabViewModel.OnAppearing();
        }
    }
}
