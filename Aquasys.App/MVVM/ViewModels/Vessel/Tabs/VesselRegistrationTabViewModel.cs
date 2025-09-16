using Aquasys.App.Controls.Editors;

using Aquasys.Core.Entities;
using Aquasys.App.Core.Enums;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.App.MVVM.Views.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CountryData.Standard;
using System.Collections.ObjectModel;
using Aquasys.App.Core.Data;

namespace Aquasys.App.MVVM.ViewModels.Vessel.Tabs
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselRegistrationTabViewModel : BaseViewModels
    {

        private readonly ILocalRepository<Aquasys.Core.Entities.Vessel> _vesselRepository;
        private readonly ILocalRepository<VesselImage> _vesselImageRepository;

        [ObservableProperty]
        private VesselModel _vesselModel;

        [ObservableProperty] private List<Country> _flags;
        [ObservableProperty] private Country _flagSelecionada;
        [ObservableProperty] private List<EntidadeGenerica> _vesselsType;
        [ObservableProperty] private EntidadeGenerica _vesselTypeSelecionado;
        [ObservableProperty] private ObservableCollection<VesselImageModel> _images;
        [ObservableProperty] private bool _expanded = true;
        [ObservableProperty] private bool _hasImages = false;

        public VesselRegistrationTabViewModel(
            ILocalRepository<Aquasys.Core.Entities.Vessel> vesselRepository,
            ILocalRepository<VesselImage> vesselImageRepository)
        {
            _vesselRepository = vesselRepository;
            _vesselImageRepository = vesselImageRepository;

            _vesselModel = new();
            _flags = new();
            _vesselsType = new();
            _flagSelecionada = new();
            _images = new();
        }

        public override async Task OnAppearing()
        {
            await CarregaDados();
        }

        private async Task CarregaDados()
        {
            CarregaFlag();
            CarregaVesselType();
            await CarregaImages();
        }

        private void CarregaFlag()
        {
            Flags = new CountryHelper().GetCountryData().ToList();

            if (!string.IsNullOrEmpty(VesselModel.Flag))
                FlagSelecionada = Flags.Where(x => x.CountryName == VesselModel?.Flag).First();
            else
                FlagSelecionada = Flags.First();
        }

        private void CarregaVesselType()
        {
            VesselsType = Enum.GetValues(typeof(VesselType)).Cast<VesselType>()
            .Select(x => new EntidadeGenerica
            {
                Chave = x,
                Valor = x.GetEnumDescription(),
            }).ToList();

            if (VesselModel?.VesselType is not null)
                VesselTypeSelecionado = new EntidadeGenerica { Chave = VesselModel.VesselType, Valor = VesselModel.VesselType.GetEnumDescription() };
            else
                VesselTypeSelecionado = VesselsType.FirstOrDefault();
        }

        private async Task CarregaImages()
        {
            if (VesselModel?.IDVessel is not null && VesselModel?.IDVessel != 0)
            {
                var vesselImages = await _vesselImageRepository.GetFilteredAsync(x => x.IDVessel == VesselModel.IDVessel);
                ObservableCollection<VesselImageModel> vesselImagesModel = new();

                vesselImages.ForEach(x => vesselImagesModel.Add(mapper.Map<VesselImageModel>(x)));

                Images = vesselImagesModel;

                if (Images.Any())
                    HasImages = true;
            }
        }

        [RelayCommand]
        private void ChangeFlag(Country flag)
        {
            if (flag is not null)
                VesselModel.Flag = flag.CountryName?.ToString() ?? string.Empty;
        }

        [RelayCommand]
        private void ChangVesselType(EntidadeGenerica vesselType)
        {
            if (vesselType is not null)
                VesselModel.VesselType = Enum.GetValues(typeof(VesselType)).Cast<VesselType>().Where(x => x.GetEnumDescription() == vesselType.Valor.ToString()).FirstOrDefault();
        }

        #region ValidateVessel
        private async Task<bool> ValidateVessel()
        {
            if (string.IsNullOrEmpty(VesselModel.VesselName))
            {
                await Shell.Current.DisplayAlert("Alerta", "Insira o Vessel Name", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(VesselModel.Place))
            {
                await Shell.Current.DisplayAlert("Alerta", "Insira o Place", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(VesselModel.IMO))
            {
                await Shell.Current.DisplayAlert("Alerta", "Insira o IMO", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(VesselModel.PortRegistry))
            {
                await Shell.Current.DisplayAlert("Alerta", "Insira o Port Registry", "OK");
                return false;
            }

            if (VesselModel?.VesselType is null)
            {
                await Shell.Current.DisplayAlert("Alerta", "Insira o Vessel Type", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(VesselModel.Owner))
            {
                await Shell.Current.DisplayAlert("Alerta", "Insira o Owner", "OK");
                return false;
            }

            if (string.IsNullOrEmpty(VesselModel.Operator))
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
            if(await ValidateVessel())
                await SaveOrUpdateVessel();
        }

        private async Task SaveOrUpdateVessel(bool mostraMensagem = true)
        {
            if (VesselModel?.IDVessel is not null && VesselModel?.IDVessel != 0)
            {
                var vesselExists = await _vesselRepository.GetByIdAsync(VesselModel?.IDVessel ?? -1);
                if (vesselExists is not null)
                {
                    vesselExists = mapper.Map<Aquasys.Core.Entities.Vessel>(VesselModel);
                    if (await _vesselRepository.UpdateAsync(vesselExists) && mostraMensagem)
                    {

                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                        await Shell.Current.GoToAsync("..", true);
                    }
                }
            }
            else
            {
                var vesselSave = mapper.Map<Aquasys.Core.Entities.Vessel>(VesselModel);
                vesselSave.IDUserRegistration = ContextUtils.ContextUser.IDUser;
                
                if (await _vesselRepository.InsertAsync(vesselSave) && mostraMensagem)
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }

                VesselModel = mapper.Map<VesselModel>(vesselSave);
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
