using Aquasys.Core.BO;
using Aquasys.MVVM.Models.Vessel;
using Aquasys.MVVM.ViewModels.Vessel.Tabs;
using Aquasys.MVVM.Views.Vessel.Tabs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Aquasys.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselMainViewModel : BaseViewModels
    {
        [ObservableProperty]
        public VesselRegistrationTabViewModel vesselRegistrationTabViewModel;

        [ObservableProperty]
        public VesselRegistrationTabPage vesselRegistrationTabPage;

        [ObservableProperty]
        public VesselHoldRegistrationTabViewModel vesselHoldRegistrationTabViewModel;

        [ObservableProperty]
        public VesselHoldRegistrationTabPage vesselHoldRegistrationTabPage;

        public VesselMainViewModel()
        {
            vesselRegistrationTabViewModel = new VesselRegistrationTabViewModel();
            vesselRegistrationTabPage = new VesselRegistrationTabPage();

            vesselHoldRegistrationTabViewModel = new VesselHoldRegistrationTabViewModel();
            vesselHoldRegistrationTabPage = new VesselHoldRegistrationTabPage();
        }
        public override async void OnAppearing()
        {
            await LoadTabs();
        }

        public async Task LoadTabs()
        {
            if (string.IsNullOrEmpty(Id))
            {
                VesselRegistrationTabViewModel.OnAppearing();
                VesselHoldRegistrationTabViewModel.OnAppearing();
                return;
            }

            var vessel = await new VesselBO().GetByIdAsync(Id);
            if(vessel is not null)
            {
                VesselRegistrationTabViewModel.VesselModel = mapper.Map<VesselModel>(vessel);
                VesselHoldRegistrationTabViewModel.IDVessel = vessel.IDVessel;

                VesselRegistrationTabViewModel.OnAppearing();
                VesselHoldRegistrationTabViewModel.OnAppearing();
            }
        }
    }
}