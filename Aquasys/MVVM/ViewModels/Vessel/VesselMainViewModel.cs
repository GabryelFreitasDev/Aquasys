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

        public VesselMainViewModel()
        {
            vesselRegistrationTabViewModel = new VesselRegistrationTabViewModel();
            vesselRegistrationTabPage = new VesselRegistrationTabPage();
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
                return;
            }

            var vessel = await new VesselBO().GetByIdAsync(Id);

            VesselRegistrationTabViewModel.VesselModel = mapper.Map<VesselModel>(vessel);

            VesselRegistrationTabViewModel.OnAppearing();
        }
    }
}