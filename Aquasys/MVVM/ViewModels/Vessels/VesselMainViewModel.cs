using Aquasys.Core.BO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Aquasys.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselMainViewModel : BaseViewModels
    {
        [ObservableProperty]
        public VesselRegistrationTabViewModel vesselRegistrationTabViewModel;

        public VesselMainViewModel()
        {
            vesselRegistrationTabViewModel = new VesselRegistrationTabViewModel();
        }

        public async void LoadTabs()
        {
            if (string.IsNullOrEmpty(Id))
                return;

            VesselBO vesselBO = new VesselBO();

            var vessel = await vesselBO.GetByIdAsync(Id);

            VesselRegistrationTabViewModel.VesselModel.IDVessel = Convert.ToInt64(Id);
            VesselRegistrationTabViewModel.VesselModel.OS = vessel.OS;
            VesselRegistrationTabViewModel.VesselModel.VesselName = vessel.VesselName;
            VesselRegistrationTabViewModel.VesselModel.Place = vessel.Place;
            VesselRegistrationTabViewModel.VesselModel.IMO = vessel.IMO;
            VesselRegistrationTabViewModel.VesselModel.PortRegistry = vessel.PortRegistry;
            VesselRegistrationTabViewModel.VesselModel.ManufacturingDate = vessel.ManufacturingDate;
            VesselRegistrationTabViewModel.VesselModel.Owner = vessel.Owner
            VesselRegistrationTabViewModel.VesselModel.Operator = vessel.Operator;


            VesselRegistrationTabViewModel.OnAppearing();

            
        }
    }
}
