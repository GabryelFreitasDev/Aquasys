using System.Collections.ObjectModel;
using System.Windows.Input;
using Aquasys.Core.BO;
using Aquasys.Core.Utils;
using Aquasys.MVVM.Models.Vessel;
using Aquasys.MVVM.Views.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Aquasys.MVVM.ViewModels.Vessel
{
    public partial class VesselListViewModel : BaseViewModels
    {
        VesselBO vesselBO = new VesselBO();

        [ObservableProperty]
        private ObservableCollection<VesselModel> vessels = new();
        
        [RelayCommand]
        private async Task BtnAddClick()
        {
            await Shell.Current.GoToAsync(nameof(VesselMainPage));
        }

        [RelayCommand]
        public async Task DeleteVessel(VesselModel vessel)
        {
            if(await Shell.Current.DisplayAlert("Alerta", "Deseja realmente excluir?", "Sim", "Cancelar"))
                if (vessel is not null)
                {
                    var vesselDeleted = await vesselBO.GetByIdAsync(vessel.IDVessel);
                    await vesselBO.DeleteAsync(vesselDeleted);
                    Vessels.Remove(vessel);
                }
        }

        [RelayCommand]
        public async Task EditVessel(VesselModel vessel)
        {
            if (vessel is not null)
                await Shell.Current.GoToAsync($"{nameof(VesselMainPage)}?{nameof(Id)}={vessel.IDVessel}");
        }

        public override async void OnAppearing()
        {
            await ValidPermissions();
            await LoadVesselsAsync();
        }

        private async Task ValidPermissions()
        {
            await PermissionUtils.ValidPermissions();
        }

        public async Task LoadVesselsAsync()
        {
            var vessels = await vesselBO.GetAllAsync();
            vessels.OrderBy(x => x.VesselName);

            Vessels.Clear();

            if (vessels is not null && vessels.Any())
            {
                foreach (var vessel in vessels)
                {
                    if(!Vessels.Any(x => x.IDVessel == vessel.IDVessel))
                        Vessels.Add(mapper.Map<VesselModel>(vessel));
                }
            }
        }
    }
}
