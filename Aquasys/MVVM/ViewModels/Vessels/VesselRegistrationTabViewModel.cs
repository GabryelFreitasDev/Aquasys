using Aquasys.Core.BO;
using Aquasys.MVVM.Models.Vessel;
using Aquasys.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Aquasys.MVVM.ViewModels.Vessel
{
    public partial class VesselRegistrationTabViewModel : BaseViewModels
    {
        [ObservableProperty]
        private long? idVessel;

        [ObservableProperty]
        private VesselModel vesselModel;

        private VesselBO vesselBO;
        public VesselRegistrationTabViewModel()
        {
            vesselBO = new VesselBO();
            vesselModel = new();
        }

        public override async void OnAppearing()
        {
            await LoadVesselModel();
        }

        private async Task LoadVesselModel()
        {
            if (IdVessel.HasValue)
            {
                var vessel = await vesselBO.GetByIdAsync(IdVessel);

                VesselModel = new VesselModel() {
                    IDVessel = vessel.IDVessel,
                    VesselName = vessel.VesselName,
                    ManufacturingDate = vessel?.ManufacturingDate ?? DateTime.Now
                };
            }
        }

        [RelayCommand]
        private async Task BtnSalvarClick()
        {
            VesselModel ??= new();
            if (VesselModel?.IDVessel is not null && VesselModel?.IDVessel != 0)
            {
                var vesselExists = await vesselBO.GetByIdAsync(VesselModel?.IDVessel ?? -1);
                if (vesselExists is not null)
                {
                    vesselExists.VesselName = VesselModel?.VesselName ?? "";
                    vesselExists.ManufacturingDate = VesselModel?.ManufacturingDate ?? DateTime.Now;
                    vesselExists.VesselType = VesselModel?.VesselType ?? Core.Enums.VesselType.CANOADORACHA;
                    vesselExists.OS = VesselModel?.OS ?? "";
                    vesselExists.IMO = VesselModel?.IMO ?? "";
                    vesselExists.Operator = VesselModel?.Operator ?? "";
                    vesselExists.PortRegistry = VesselModel?.PortRegistry ?? "";
                    vesselExists.Owner = VesselModel?.Owner ?? "";
                    vesselExists.Place = VesselModel?.Place ?? "";
                    vesselExists.UserRegistration = ContextUtils.ContextUser;
                    vesselExists.DateTimeOfRegister = DateTime.Now;

                    if (await vesselBO.UpdateAsync(vesselExists))
                    {
                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                        await Shell.Current.GoToAsync("..", true);
                    }
                }
            }
            else
            {
                Core.Entities.Vessel vesselSave = new Core.Entities.Vessel();

                vesselSave.VesselName = VesselModel?.VesselName ?? "";
                vesselSave.ManufacturingDate = VesselModel?.ManufacturingDate ?? DateTime.Now;
                vesselSave.VesselType = VesselModel?.VesselType ?? Core.Enums.VesselType.CANOADORACHA;
                vesselSave.OS = VesselModel?.OS ?? "";
                vesselSave.IMO = VesselModel?.IMO ?? "";
                vesselSave.Operator = VesselModel?.Operator ?? "";
                vesselSave.PortRegistry = VesselModel?.PortRegistry ?? "";
                vesselSave.Owner = VesselModel?.Owner ?? "";
                vesselSave.Place = VesselModel?.Place ?? "";
                vesselSave.UserRegistration = ContextUtils.ContextUser;
                vesselSave.DateTimeOfRegister = DateTime.Now;

                if (await vesselBO.InsertAsync(vesselSave))
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
        }
    }
}
