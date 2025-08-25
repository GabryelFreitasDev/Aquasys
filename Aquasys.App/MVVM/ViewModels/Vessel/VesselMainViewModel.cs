using Aquasys.App.Core.BO;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.App.MVVM.ViewModels.Vessel.Tabs;
using Aquasys.App.MVVM.Views.Vessel.Tabs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Aquasys.App.MVVM.ViewModels.Vessel
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

        [ObservableProperty]
        public VesselInspectionRegistrationTabViewModel vesselInspectionRegistrationTabViewModel;

        [ObservableProperty]
        public VesselInspectionRegistrationTabPage vesselInspectionRegistrationTabPage;

        public VesselMainViewModel()
        {
            vesselRegistrationTabViewModel = new VesselRegistrationTabViewModel();
            vesselRegistrationTabPage = new VesselRegistrationTabPage();

            vesselHoldRegistrationTabViewModel = new VesselHoldRegistrationTabViewModel();
            vesselHoldRegistrationTabPage = new VesselHoldRegistrationTabPage();

            vesselInspectionRegistrationTabViewModel = new VesselInspectionRegistrationTabViewModel();
            vesselInspectionRegistrationTabPage = new VesselInspectionRegistrationTabPage();
        }
        public override async Task OnAppearing()
        {
            await LoadTabs();
        }

        public async Task LoadTabs()
        {
            if (string.IsNullOrEmpty(Id))
            {
                await VesselRegistrationTabViewModel.OnAppearing();
                await VesselHoldRegistrationTabViewModel.OnAppearing();
                return;
            }

            var vessel = await new VesselBO().GetByIdAsync(Id);
            if(vessel is not null)
            {
                VesselRegistrationTabViewModel.VesselModel = mapper.Map<VesselModel>(vessel);
                VesselHoldRegistrationTabViewModel.IDVessel = vessel.IDVessel;
                VesselInspectionRegistrationTabViewModel.IDVessel = vessel.IDVessel;

                await VesselRegistrationTabViewModel.OnAppearing();
                await VesselHoldRegistrationTabViewModel.OnAppearing();
                await VesselInspectionRegistrationTabViewModel.OnAppearing();
            }
        }
    }
}