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
        private VesselModel vesselModel;

        [ObservableProperty] private List<Country> flags;
        [ObservableProperty] private Country? flagSelecionada;
        [ObservableProperty] private ObservableCollection<VesselImageModel> images;
        [ObservableProperty] private bool expanded = true;
        [ObservableProperty] private bool hasImages = false;
        [ObservableProperty] private bool isEnabled = false;

        private List<Country> allFlags =  new List<Country>();

        public VesselRegistrationViewModel(
            ILocalRepository<Aquasys.Core.Entities.Vessel> vesselRepository,
            ILocalRepository<VesselImage> vesselImageRepository,
            ReportGeneratorService reportService)
        {
            _vesselRepository = vesselRepository;
            _vesselImageRepository = vesselImageRepository;
            _reportService = reportService;

            vesselModel = new();
            flags = new();
            flagSelecionada = null;
            images = new();
        }

        public override async Task OnAppearing()
        {
            await CarregaDados();
        }

        private async Task CarregaDados()
        {
            if (!string.IsNullOrEmpty(Id) && long.TryParse(Id, out long idVessel) && idVessel != 0)
            {
                var vesselData = await _vesselRepository.GetByIdAsync(idVessel);
                if (vesselData != null)
                {
                    VesselModel = mapper.Map<VesselModel>(vesselData);
                    await CarregaImages();
                }
            }

            CarregaFlag();
        }

        private void CarregaFlag()
        {
            allFlags = new CountryHelper().GetCountryData().Select(x => new Country { CountryName = x.CountryName, CountryFlag = x.CountryFlag }).ToList();

            Flags = allFlags.Take(30).ToList();

            if (!string.IsNullOrEmpty(VesselModel.Flag))
                FlagSelecionada = allFlags.Where(x => x.CountryName == VesselModel.Flag).First();
        }

        private async Task CarregaImages()
        {
            if (VesselModel?.IDVessel != null && VesselModel?.IDVessel != 0)
            {
                var vesselImages = await _vesselImageRepository.GetFilteredAsync(x => x.IDVessel == VesselModel!.IDVessel);
                ObservableCollection<VesselImageModel> vesselImagesModel = new();

                vesselImages.ForEach(image => vesselImagesModel.Add(mapper.Map<VesselImageModel>(image)));

                Images = vesselImagesModel;

                if (Images.Any())
                    HasImages = true;
            }
        }

        [RelayCommand]
        private void ChangeFlag(Country flag)
        {
            if (flag is not null)
                VesselModel.Flag = flag.CountryName;
        }

        [RelayCommand]
        private void ChangeFlagName(string name)
        {
            if (string.IsNullOrEmpty(name))
                Flags = allFlags.Take(30).ToList();
            else
                Flags = allFlags.Where(x => x.CountryName.ToLower().Contains(name.ToLower())).Take(30).ToList();
        }

        #region ValidateVessel
        private async Task<bool> ValidateVessel()
        {
            if (string.IsNullOrEmpty(VesselModel.VesselName))
            {
                await Shell.Current.DisplayAlert("Alerta", "Insira o Vessel Name", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(VesselModel.Imo))
            {
                await Shell.Current.DisplayAlert("Alerta", "Insira o IMO", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(VesselModel.PortRegistry))
            {
                await Shell.Current.DisplayAlert("Alerta", "Insira o Port Registry", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(VesselModel.Owner))
            {
                await Shell.Current.DisplayAlert("Alerta", "Insira o Owner", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(VesselModel.VesselOperator))
            {
                await Shell.Current.DisplayAlert("Alerta", "Insira o Operator", "OK");
                return false;
            }

            return true;
        }
        #endregion

        [RelayCommand]
        private async Task BtnSalvarClick()
        {
            await SaveOrUpdateVessel(true);
        }

        public async Task SaveOrUpdateVessel(bool mostraMensagem = true)
        {
            // Verifique se VesselModel não é null antes de validar
            if (VesselModel == null ||
                string.IsNullOrWhiteSpace(VesselModel.VesselName) ||
                string.IsNullOrWhiteSpace(VesselModel.Imo) ||
                string.IsNullOrWhiteSpace(VesselModel.PortRegistry) ||
                string.IsNullOrWhiteSpace(VesselModel.Owner) ||
                string.IsNullOrWhiteSpace(VesselModel.VesselOperator))
            {
                await Application.Current!.MainPage!.DisplayAlert("Alert", "Please fill the required fields.", "OK");
                return;
            }

            Aquasys.Core.Entities.Vessel vesselEntity = new Aquasys.Core.Entities.Vessel();

            if (VesselModel?.IDVessel != null && VesselModel?.IDVessel != -1)
                vesselEntity = await _vesselRepository.GetByIdAsync(VesselModel?.IDVessel ?? -1);

            if (mostraMensagem)
            {
                vesselEntity = mapper.Map<Aquasys.Core.Entities.Vessel>(VesselModel);
                vesselEntity.IDUserRegistration = ContextUtils.ContextUser.IDUser;
            }

            await _vesselRepository.UpsertAsync(vesselEntity);

            if (mostraMensagem)
            {
                await Shell.Current.DisplayAlert("Alert", "Saved successfully", "OK");
                await Shell.Current.GoToAsync("..", true);
            }
            else
            {
                vesselEntity = await _vesselRepository.GetByIdAsync(VesselModel?.IDVessel ?? -1);
                VesselModel = mapper.Map<VesselModel>(vesselEntity);
            }
        }

        [RelayCommand]
        private async Task AddVesselImage()
        {
            try
            {
                if (IsProcessRunning)
                    return;

                await SaveOrUpdateVessel(false);

                IsProcessRunning = true;

                var anexo = (await DCFileSelector.GetImagens(1)).FirstOrDefault();
                if (anexo != null && anexo is DCImagem _anexo && (!_anexo.ImageSource?.IsEmpty ?? false))
                {
                    VesselImage vesselImage = new VesselImage();
                    vesselImage.Image = anexo.Content;
                    vesselImage.IDVessel = VesselModel.IDVessel;

                    await _vesselImageRepository.InsertAsync(vesselImage);

                    MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"{nameof(VesselImagePage)}?{nameof(Id)}={vesselImage.IDVesselImage}"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await CarregaImages();
                IsProcessRunning = false;
            }
        }
        [RelayCommand]
        private async Task EditVesselImage(VesselImageModel vesselImageModel)
        {
            try
            {
                if (IsProcessRunning || vesselImageModel is null)
                    return;

                await SaveOrUpdateVessel(false);

                IsProcessRunning = true;

                MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"{nameof(VesselImagePage)}?{nameof(Id)}={vesselImageModel.IDVesselImage}"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                await CarregaImages();
                IsProcessRunning = false;
            }
        }

        [RelayCommand]
        private async Task DeleteVesselImage(VesselImageModel vesselImageModel)
        {
            try
            {
                if (IsProcessRunning || vesselImageModel is null)
                    return;

                await SaveOrUpdateVessel(false);

                IsProcessRunning = true;

                var vesselImage = mapper.Map<VesselImage>(vesselImageModel);

                if (await Shell.Current.DisplayAlert("Alerta", "Deseja realmente excluir?", "Sim", "Cancelar"))
                {
                    await _vesselImageRepository.DeleteAsync(vesselImage);
                    Images.Remove(vesselImageModel);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                IsProcessRunning = false;
            }
        }

        [RelayCommand]
        private async Task ExpandImages(VesselImageModel vesselImageModel)
        {
            if (Expanded == true)
                Expanded = false;
            else
                Expanded = true;
        }
    }
}
