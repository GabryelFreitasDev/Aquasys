using Aquasys.App.Core.Data;
using Aquasys.App.Core.Intefaces;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.App.MVVM.Views.Vessel;
using Aquasys.Core.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    public partial class VesselListViewModel : BaseViewModels
    {
        private readonly ILocalRepository<Aquasys.Core.Entities.Vessel> _vesselRepository;
        private readonly ILocalRepository<Aquasys.Core.Entities.VesselImage> _vesselImageRepository;

        [ObservableProperty]
        private ObservableCollection<VesselModel> _vessels = new();

        [ObservableProperty]
        private string searchVesselName = string.Empty;

        [RelayCommand]
        public async Task SearchVesselNameChanged(string value)
        {
            await LoadVesselsAsync();
        }

        public VesselListViewModel(
            ILocalRepository<Aquasys.Core.Entities.Vessel> vesselRepository,
            ILocalRepository<Aquasys.Core.Entities.VesselImage> vesselImageRepository)
        {
            _vesselRepository = vesselRepository;
            _vesselImageRepository = vesselImageRepository;
        }

        [RelayCommand]
        private async Task AddVessel()
        {
            await Shell.Current.GoToAsync(nameof(VesselMainPage));
        }

        [RelayCommand]
        public async Task DeleteVessel(VesselModel vessel)
        {
            if (await Shell.Current.DisplayAlert("Alerta", "Deseja realmente excluir?", "Sim", "Cancelar"))
                if (vessel is not null)
                {
                    var vesselToDelete = await _vesselRepository.GetByIdAsync(vessel.IDVessel);
                    await _vesselRepository.DeleteAsync(vesselToDelete);
                    Vessels.Remove(vessel);
                }
        }

        [RelayCommand]
        public async Task EditVessel(VesselModel vessel)
        {
            if (vessel is not null)
                await Shell.Current.GoToAsync($"{nameof(VesselMainPage)}?{nameof(Id)}={vessel.IDVessel}");
        }

        public override async Task OnAppearing()
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
            var orderedVessels = Enumerable.Empty<Aquasys.Core.Entities.Vessel>();

            if (string.IsNullOrWhiteSpace(searchVesselName))
            {
                var vesselsData = await _vesselRepository.GetAllAsync();
                orderedVessels = vesselsData.OrderBy(x => x.VesselName);
            }
            else
            {
                string searchTextLower = searchVesselName.ToLower();
                var vesselsData = await _vesselRepository.GetFilteredAsync(
                    v => v.VesselName.ToLower().Contains(searchTextLower)
                );
                orderedVessels = vesselsData.OrderBy(x => x.VesselName);
            }

            Vessels.Clear();
            if (orderedVessels.Any())
            {
                foreach (var vessel in orderedVessels)
                {
                    var vesselModel = mapper.Map<VesselModel>(vessel);
                    var vesselImages = await _vesselImageRepository.GetFilteredAsync(x => x.IDVessel == vesselModel.IDVessel);
                    vesselModel.FirstImage = vesselImages?.FirstOrDefault()?.Image ?? (byte[])ResourceUtils.GetResourceValue("aquasyslogo");
                    Vessels.Add(vesselModel);
                }
            }
        }
    }
}