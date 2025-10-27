using Aquasys.App.Core.Intefaces;
using Aquasys.Core.Entities;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.App.MVVM.Views.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Aquasys.App.Core.Data;
using Aquasys.App.Controls.Editors;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    [QueryProperty(nameof(IDHold), nameof(IDHold))]
    public partial class HoldInspectionViewModel : BaseViewModels
    {
        private readonly ILocalRepository<HoldInspection> _holdInspectionRepository;
        private readonly ILocalRepository<HoldInspectionImage> _holdInspectionImageRepository;

        [ObservableProperty]
        private HoldInspectionModel holdInspectionModel;

        [ObservableProperty]
        private ObservableCollection<HoldInspectionImageModel> holdInspectionImages;

        [ObservableProperty] private bool _expanded = true;
        [ObservableProperty] private bool _hasImages = false;
        public long IDHold { get; set; }

        public HoldInspectionViewModel(
            ILocalRepository<HoldInspection> holdInspectionRepository,
            ILocalRepository<HoldInspectionImage> holdInspectionImageRepository)
        {
            _holdInspectionRepository = holdInspectionRepository;
            _holdInspectionImageRepository = holdInspectionImageRepository;

            holdInspectionModel = new();
            holdInspectionImages = new();
        }

        public override async Task OnAppearing()
        {
            await PreencheDados();
        }

        private async Task PreencheDados()
        {
            if (Id.IsNotNullOrEmpty())
            {
                var holdInspection = await _holdInspectionRepository.GetByIdAsync(Id.ToLong());
                HoldInspectionModel = mapper.Map<HoldInspectionModel>(holdInspection);
                HoldInspectionModel.InspectionDate = holdInspection.InspectionDateTime.Date;
                HoldInspectionModel.InspectionTime = holdInspection.InspectionDateTime.TimeOfDay;

                var holdInspectionImagesList = await _holdInspectionImageRepository.GetFilteredAsync(x => x.IDHoldInspection == HoldInspectionModel.IDHoldInspection);
                HoldInspectionImages = new ObservableCollection<HoldInspectionImageModel>(mapper.Map<List<HoldInspectionImageModel>>(holdInspectionImagesList));
                HasImages = HoldInspectionImages.Any();
            }
        }

        [RelayCommand]
        private async Task Save() {
            await SaveHoldInspection(true);
        }

        private async Task SaveHoldInspection(bool mostraMensagem = true)
        {
            if (HoldInspectionModel.IDHoldInspection != -1)
            {
                var holdInspection = await _holdInspectionRepository.GetByIdAsync(HoldInspectionModel.IDHoldInspection);
                if (holdInspection != null)
                {
                    holdInspection = mapper.Map<HoldInspection>(HoldInspectionModel);
                    holdInspection.InspectionDateTime = GetInspectionDateTime();
                    await _holdInspectionRepository.UpdateAsync(holdInspection);
                }
            }
            else
            {
                var holdInspection = mapper.Map<HoldInspection>(HoldInspectionModel);
                holdInspection.IDHold = IDHold;
                holdInspection.InspectionDateTime = GetInspectionDateTime();
                await _holdInspectionRepository.InsertAsync(holdInspection);
                HoldInspectionModel.IDHoldInspection = holdInspection.IDHoldInspection;
            }

            if (mostraMensagem)
            {
                await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                await Shell.Current.GoToAsync("..", true);
            }
        }

        private DateTime GetInspectionDateTime()
        {
            var date = HoldInspectionModel.InspectionDate.Date;
            var time = HoldInspectionModel.InspectionTime;
            return date + time;
        }

        [RelayCommand]
        private async Task Expand()
        {
            if (Expanded == true)
                Expanded = false;
            else
                Expanded = true;
        }

        [RelayCommand]
        private async Task AddHoldInspectionImage()
        {
            try
            {
                if (IsProcessRunning)
                    return;

                await SaveHoldInspection(false);

                IsProcessRunning = true;

                var anexo = (await DCFileSelector.GetImagens(1)).FirstOrDefault();
                if (anexo != null && anexo is DCImagem _anexo && (!_anexo.ImageSource?.IsEmpty ?? false))
                {
                    HoldInspectionImage holdInspectionImage = new HoldInspectionImage();
                    holdInspectionImage.Image = anexo.Content;
                    holdInspectionImage.IDHoldInspection = HoldInspectionModel.IDHoldInspection;

                    await _holdInspectionImageRepository.UpsertAsync(holdInspectionImage);

                    MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"{nameof(HoldInspectionImagePage)}?{nameof(Id)}={holdInspectionImage.IDHoldInspectionImage}"));
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
        private async Task EditHoldInspectionImage(HoldInspectionImageModel holdInspectionImageModel)
        {
            try
            {
                if (IsProcessRunning || holdInspectionImageModel is null)
                    return;

                await SaveHoldInspection(false);

                IsProcessRunning = true;

                MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"{nameof(HoldInspectionImagePage)}?{nameof(Id)}={holdInspectionImageModel.IDHoldInspectionImage}"));
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
        private async Task DeleteHoldInspectionImage(HoldInspectionImageModel holdInspectionImageModel)
        {
            try
            {
                if (IsProcessRunning || holdInspectionImageModel is null)
                    return;

                await SaveHoldInspection(false);

                IsProcessRunning = true;

                var holdInspectionImage = mapper.Map<HoldInspectionImage>(holdInspectionImageModel);

                if (await Shell.Current.DisplayAlert("Alerta", "Deseja realmente excluir?", "Sim", "Cancelar"))
                {
                    await _holdInspectionImageRepository.DeleteAsync(holdInspectionImage);
                    HoldInspectionImages.Remove(holdInspectionImageModel);
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
    }
}