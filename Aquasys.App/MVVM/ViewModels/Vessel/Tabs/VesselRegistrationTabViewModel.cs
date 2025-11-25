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

namespace Aquasys.App.MVVM.ViewModels.Vessel.Tabs
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselRegistrationTabViewModel : BaseViewModels
    {
        private readonly ILocalRepository<Aquasys.Core.Entities.Vessel> _vesselRepository;
        private readonly ILocalRepository<VesselImage> _vesselImageRepository;
        private readonly ReportGeneratorService _reportService;
        private readonly CountryHelper _countryHelper = new();

        // ------------- Propriedades ligadas à tela -------------

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

        [ObservableProperty]
        private bool isEnabled; // se for usar depois para habilitar/desabilitar campos

        private List<Country> allFlags = new();

        public VesselRegistrationTabViewModel(
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
            if (IsLoadedViewModel)
                return;

            IsLoadedViewModel = true;
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await LoadVesselAsync();
            await LoadImagesAsync();
            LoadFlags();
        }

        private async Task LoadVesselAsync()
        {
            if (string.IsNullOrWhiteSpace(Id) || !long.TryParse(Id, out var vesselId) || vesselId == 0)
                return;

            var vesselData = await _vesselRepository.GetByIdAsync(vesselId);
            if (vesselData is null)
                return;

            VesselModel = mapper.Map<VesselModel>(vesselData);
        }

        private async Task LoadImagesAsync()
        {
            Images.Clear();
            HasImages = false;

            if (VesselModel?.IDVessel is null or 0)
                return;

            var vesselImages = await _vesselImageRepository
                .GetFilteredAsync(x => x.IDVessel == VesselModel.IDVessel);

            foreach (var image in vesselImages)
                Images.Add(mapper.Map<VesselImageModel>(image));

            HasImages = Images.Any();
        }

        private void LoadFlags()
        {
            allFlags = _countryHelper.GetCountryData()
                .Select(x => new Country
                {
                    CountryName = x.CountryName,
                    CountryFlag = x.CountryFlag
                })
                .ToList();

            Flags = allFlags.Take(30).ToList();

            if (!string.IsNullOrWhiteSpace(VesselModel.Flag))
            {
                SelectedFlag = allFlags
                    .FirstOrDefault(x => x.CountryName == VesselModel.Flag);
            }
        }

        [RelayCommand]
        private void ChangeFlagName(string name)
        {
            if (string.IsNullOrEmpty(name))
                Flags = allFlags.Take(30).ToList();
            else
                Flags = allFlags.Where(x => x.CountryName.ToLower().Contains(name.ToLower())).Take(30).ToList();
        }

        [RelayCommand]
        private async Task AddVesselImageAsync()
        {
            if (IsProcessRunning)
                return;

            try
            {
                IsProcessRunning = true;

                var arquivo = (await DCFileSelector.GetImagens(1)).FirstOrDefault();
                if (arquivo is not DCImagem imagem || (imagem.ImageSource?.IsEmpty ?? true))
                    return;

                if (VesselModel?.IDVessel is null or 0)
                {
                    await Shell.Current.DisplayAlert("Aviso", "Salve o Vessel antes de adicionar imagens.", "OK");
                    return;
                }

                var vesselImage = new VesselImage
                {
                    Image = arquivo.Content,
                    IDVessel = VesselModel.IDVessel
                };

                await _vesselImageRepository.UpsertAsync(vesselImage);

                Images.Add(mapper.Map<VesselImageModel>(vesselImage));
                HasImages = Images.Any();

                await MainThread.InvokeOnMainThreadAsync(
                    () => Shell.Current.GoToAsync($"{nameof(VesselImagePage)}?{nameof(Id)}={vesselImage.IDVesselImage}")
                );
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao adicionar imagem: {ex.Message}", "OK");
            }
            finally
            {
                IsProcessRunning = false;
            }
        }

        [RelayCommand]
        private async Task EditVesselImage(VesselImageModel? vesselImageModel)
        {
            if (IsProcessRunning || vesselImageModel is null)
                return;

            try
            {
                IsProcessRunning = true;

                await MainThread.InvokeOnMainThreadAsync(
                    () => Shell.Current.GoToAsync($"{nameof(VesselImagePage)}?{nameof(Id)}={vesselImageModel.IDVesselImage}")
                );
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao abrir imagem: {ex.Message}", "OK");
            }
            finally
            {
                IsProcessRunning = false;
            }
        }

        [RelayCommand]
        private async Task DeleteVesselImage(VesselImageModel? vesselImageModel)
        {
            if (IsProcessRunning || vesselImageModel is null)
                return;

            try
            {
                IsProcessRunning = true;

                var confirm = await Shell.Current.DisplayAlert(
                    "Alerta", "Deseja realmente excluir?", "Sim", "Cancelar");

                if (!confirm)
                    return;

                var vesselImage = mapper.Map<VesselImage>(vesselImageModel);
                await _vesselImageRepository.DeleteAsync(vesselImage);

                Images.Remove(vesselImageModel);
                HasImages = Images.Any();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao excluir imagem: {ex.Message}", "OK");
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

        [RelayCommand]
        private async Task GenerateReportAsync()
        {
            try
            {
                if (VesselModel == null || VesselModel.IDVessel == 0)
                {
                    await Shell.Current.DisplayAlert(
                        "Aviso",
                        "Salve o Vessel antes de gerar o relatório.",
                        "OK");
                    return;
                }

                var vesselEntity = mapper.Map<Aquasys.Core.Entities.Vessel>(VesselModel);

                var pdfBytes = await _reportService.GenerateAsync(ReportType.Vessel, vesselEntity);
                var fileName = $"Relatorio_{VesselModel.VesselName}.pdf";
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                File.WriteAllBytes(filePath, pdfBytes);

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Abrir/Compartilhar Relatório",
                    File = new ShareFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro ao gerar relatório", ex.Message, "OK");
            }
        }
    }
}
