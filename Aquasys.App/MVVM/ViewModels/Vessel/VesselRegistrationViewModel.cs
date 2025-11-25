using Aquasys.App.Controls.Editors;
using Aquasys.App.Core.Data;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.App.MVVM.Views.Vessel;
using Aquasys.Core.Entities;
using Aquasys.Reports.Enums;
using Aquasys.Reports.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CountryData.Standard;
using System.Collections.ObjectModel;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselRegistrationViewModel : BaseViewModels
    {
        private readonly ILocalRepository<Aquasys.Core.Entities.Vessel> _vesselRepository;
        private readonly ILocalRepository<VesselImage> _vesselImageRepository;
        private readonly ReportGeneratorService _reportService;


        [ObservableProperty]
        private VesselModel vesselModel = new();

        [ObservableProperty]
        private List<Country> flags = new();

        [ObservableProperty]
        private Country? selectedFlag;

        [ObservableProperty]
        private ObservableCollection<VesselImageModel> images = new();

        [ObservableProperty]
        private bool expanded = true;

        [ObservableProperty]
        private bool expandedImages = true;

        [ObservableProperty]
        private bool hasImages;

        private List<Country> allFlags = new();

        public VesselRegistrationViewModel(
            ILocalRepository<Aquasys.Core.Entities.Vessel> vesselRepository,
            ILocalRepository<VesselImage> vesselImageRepository,
            ReportGeneratorService reportService)
        {
            _vesselRepository = vesselRepository;
            _vesselImageRepository = vesselImageRepository;
            _reportService = reportService;
        }

        public override async Task OnAppearing()
        {
            if (!IsLoadedViewModel)
            {
                IsLoadedViewModel = true;

                await LoadVesselAsync();
                LoadFlags();
            }

            await LoadImagesAsync();
        }

        private async Task LoadVesselAsync()
        {
            if (string.IsNullOrWhiteSpace(Id) ||
                !long.TryParse(Id, out var vesselId) ||
                vesselId == 0)
                return;

            var vessel = await _vesselRepository.GetByIdAsync(vesselId);
            if (vessel is null) return;

            VesselModel = mapper.Map<VesselModel>(vessel);
        }

        private async Task LoadImagesAsync()
        {
            Images.Clear();
            HasImages = false;

            if (VesselModel?.IDVessel is null or 0)
                return;

            var items = await _vesselImageRepository.GetFilteredAsync(x => x.IDVessel == VesselModel.IDVessel);

            foreach (var img in items)
                Images.Add(mapper.Map<VesselImageModel>(img));

            HasImages = Images.Any();
        }

        private void LoadFlags()
        {
            var helper = new CountryHelper();

            allFlags = helper.GetCountryData()
                             .Select(x => new Country
                             {
                                 CountryName = x.CountryName,
                                 CountryFlag = x.CountryFlag
                             })
                             .ToList();

            Flags = allFlags.Take(30).ToList();

            if (!string.IsNullOrWhiteSpace(VesselModel.Flag))
                SelectedFlag = allFlags.FirstOrDefault(x => x.CountryName == VesselModel.Flag);
        }

        [RelayCommand]
        private void ChangeFlagName(string name)
        {
            if (string.IsNullOrEmpty(name))
                Flags = allFlags.Take(30).ToList();
            else
                Flags = allFlags.Where(x => x.CountryName.ToLower().Contains(name.ToLower())).Take(30).ToList();
        }

        private async Task<bool> ValidateVesselAsync()
        {
            if (string.IsNullOrWhiteSpace(VesselModel.VesselName))
                return await Alert("Insira o Vessel Name");

            if (string.IsNullOrWhiteSpace(VesselModel.Imo))
                return await Alert("Insira o IMO");

            if (string.IsNullOrWhiteSpace(VesselModel.PortRegistry))
                return await Alert("Insira o Port Registry");

            if (string.IsNullOrWhiteSpace(VesselModel.Owner))
                return await Alert("Insira o Owner");

            if (string.IsNullOrWhiteSpace(VesselModel.VesselOperator))
                return await Alert("Insira o Operator");

            return true;

            async Task<bool> Alert(string msg)
            {
                await Shell.Current.DisplayAlert("Alerta", msg, "OK");
                return false;
            }
        }

        [RelayCommand]
        private async Task BtnSalvarClick()
        {
            await SaveOrUpdateVessel(false);
        }

        public async Task SaveOrUpdateVessel(bool parcial = false)
        {
            Aquasys.Core.Entities.Vessel vesselEntity = new Aquasys.Core.Entities.Vessel();
            if (!await ValidateVesselAsync()) return;

            Aquasys.Core.Entities.Vessel entity;

            if (VesselModel?.IDVessel is long id && id > 0)
                entity = await _vesselRepository.GetByIdAsync(id) ?? new();
            else
                entity = new();

            entity = mapper.Map<Aquasys.Core.Entities.Vessel>(VesselModel);
            entity.Flag = SelectedFlag?.CountryName;
            entity.IDUserRegistration = ContextUtils.ContextUser.IDUser;

            await _vesselRepository.UpsertAsync(entity);

            if (!parcial)
            {
                await Shell.Current.DisplayAlert("Sucesso", "Vessel salvo!", "OK");
                await Shell.Current.GoToAsync("..", true);
            }
            else
            {
                var updated = await _vesselRepository.GetByIdAsync(entity.IDVessel);
                VesselModel = mapper.Map<VesselModel>(updated);
            }
        }

        [RelayCommand]
        private async Task AddVesselImage()
        {
            if (IsProcessRunning) return;

            try
            {
                if (!await ValidateVesselAsync()) return;

                await SaveOrUpdateVessel(true);

                IsProcessRunning = true;

                var imgFile = (await DCFileSelector.GetImagens(1)).FirstOrDefault();
                if (imgFile is not DCImagem dcimg || (dcimg.ImageSource?.IsEmpty ?? true))
                    return;

                var vesselImage = new VesselImage
                {
                    Image = dcimg.Content,
                    IDVessel = VesselModel.IDVessel
                };

                await _vesselImageRepository.UpsertAsync(vesselImage);

                await Shell.Current.GoToAsync($"{nameof(VesselImagePage)}?{nameof(Id)}={vesselImage.IDVesselImage}");

            }
            finally
            {
                await LoadImagesAsync();
                IsProcessRunning = false;
            }
        }

        [RelayCommand]
        private async Task EditVesselImageAsync(VesselImageModel? vesselImageModel)
        {
            if (vesselImageModel is null || IsProcessRunning) return;

            try
            {
                await SaveOrUpdateVessel(true);

                IsProcessRunning = true;

                await Shell.Current.GoToAsync($"{nameof(VesselImagePage)}?{nameof(Id)}={vesselImageModel.IDVesselImage}");
            }
            finally
            {
                await LoadImagesAsync();
                IsProcessRunning = false;
            }
        }

        [RelayCommand]
        private async Task DeleteVesselImageAsync(VesselImageModel? vesselImageModel)
        {
            if (vesselImageModel is null || IsProcessRunning) return;

            try
            {
                await SaveOrUpdateVessel(true);

                IsProcessRunning = true;

                var confirm = await Shell.Current.DisplayAlert("Alerta", "Excluir imagem?", "Sim", "Cancelar");
                if (!confirm) return;

                var entity = mapper.Map<VesselImage>(vesselImageModel);
                await _vesselImageRepository.DeleteAsync(entity);

                Images.Remove(vesselImageModel);
            }
            finally
            {
                IsProcessRunning = false;
            }
        }


        [RelayCommand]
        private void ToggleImagesExpanded()
        {
            ExpandedImages = !ExpandedImages;
        }

        [RelayCommand]
        private void ToggleExpanded()
        {
            Expanded = !Expanded;
        }
    }
}
