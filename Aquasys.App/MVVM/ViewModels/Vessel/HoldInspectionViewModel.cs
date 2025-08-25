using Aquasys.App.Controls.Editors;
using Aquasys.App.Core.BO;
using Aquasys.App.Core.Entities;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.App.MVVM.Views.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    [QueryProperty(nameof(IDInspection), nameof(IDInspection))]
    [QueryProperty(nameof(IDHold), nameof(IDHold))]
    public partial class HoldInspectionViewModel : BaseViewModels
    {
        [ObservableProperty]
        private HoldInspectionModel holdInspectionModel;

        [ObservableProperty]
        private HoldConditionModel holdConditionModel;

        [ObservableProperty]
        private ObservableCollection<HoldImageModel> holdImages;

        [ObservableProperty]
        private bool expanded = true;

        [ObservableProperty]
        private bool hasImages = false;

        public long IDInspection { get; set; }
        public long IDHold { get; set; }

        public HoldInspectionViewModel()
        {
            holdInspectionModel = new();
            holdConditionModel = new();
            holdImages = new();
        }

        public override async Task OnAppearing()
        {
            await PreencheDados();
        }

        private async Task PreencheDados()
        {
            if (Id.IsNotNullOrEmpty())
            {
                var holdInspection = await new HoldInspectionBO().GetByIdAsync(Id.ToLong());
                HoldInspectionModel = mapper.Map<HoldInspectionModel>(holdInspection);

                var holdCondition = await new HoldConditionBO().GetFilteredAsync(x => x.IDHoldInspection == HoldInspectionModel.IDHoldInspection);
                HoldConditionModel = mapper.Map<HoldConditionModel>(holdCondition.FirstOrDefault()) ?? new();

                var holdImages = await new HoldImageBO().GetFilteredAsync(x => x.IDHold == HoldInspectionModel.IDHold);
                ObservableCollection<HoldImageModel> holdImagesModel = new();

                holdImages.ForEach(x => holdImagesModel.Add(mapper.Map<HoldImageModel>(x)));

                HoldImages = holdImagesModel ?? new();

                if (HoldImages.Any())
                    HasImages = true;
            }
        }

        [RelayCommand]
        private async Task SaveHoldInspection()
        {
            await SaveOrUpdateHoldInspection();
        }

        private async Task SaveOrUpdateHoldInspection(bool mostraMensagem = true)
        {
            if(HoldInspectionModel?.IDHoldInspection is not null && HoldInspectionModel?.IDHoldInspection != 0)
            {
                var holdInspection = await new HoldInspectionBO().GetByIdAsync(HoldInspectionModel?.IDHoldInspection ?? -1);
                if(holdInspection is not null)
                {
                    holdInspection = mapper.Map<HoldInspection>(HoldInspectionModel);
                    await new HoldInspectionBO().UpdateAsync(holdInspection);
                }
            }
            else
            {
                var holdInspection = mapper.Map<HoldInspection>(HoldInspectionModel);
                holdInspection.IDHold = IDHold;
                holdInspection.IDInspection = IDInspection;
                await new HoldInspectionBO().InsertAsync(holdInspection);
            }

            if (HoldConditionModel?.IDHoldCondition is not null && HoldConditionModel?.IDHoldCondition != 0)
            {
                var holdCondition = await new HoldConditionBO().GetByIdAsync(HoldConditionModel?.IDHoldCondition ?? -1);
                if (holdCondition is not null)
                {
                    holdCondition = mapper.Map<HoldCondition>(HoldConditionModel);
                    if (await new HoldConditionBO().UpdateAsync(holdCondition) && mostraMensagem)
                    {
                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                        await Shell.Current.GoToAsync("..", true);
                    }
                }
            }
            else
            {
                var holdCondition = mapper.Map<HoldCondition>(HoldConditionModel);
                holdCondition.IDHoldInspection = HoldInspectionModel.IDHoldInspection;
                
                if (await new HoldConditionBO().InsertAsync(holdCondition) && mostraMensagem)
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
        }


        [RelayCommand]
        private async Task Expand(VesselImageModel holdImageModel)
        {
            if (Expanded == true)
                Expanded = false;
            else
                Expanded = true;
        }

        [RelayCommand]
        private async Task AddHoldImage()
        {
            try
            {
                if (IsProcessRunning)
                    return;

                await SaveOrUpdateHoldInspection(false);

                IsProcessRunning = true;

                var anexo = (await DCFileSelector.GetImagens(1)).FirstOrDefault();
                if (anexo != null && anexo is DCImagem _anexo && (!_anexo.ImageSource?.IsEmpty ?? false))
                {
                    HoldImage holdImage = new HoldImage(); 
                    holdImage.Image = anexo.Content;
                    holdImage.IDHold = HoldInspectionModel.IDHold;

                    await new HoldImageBO().InsertAsync(holdImage);

                    MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"{nameof(HoldImagePage)}?{nameof(Id)}={holdImage.IDHoldImage}"));
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
        private async Task EditHoldImage(HoldImageModel holdImageModel)
        {
            try
            {
                if (IsProcessRunning || holdImageModel is null)
                    return;

                await SaveOrUpdateHoldInspection(false);

                IsProcessRunning = true;

                MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"{nameof(HoldImagePage)}?{nameof(Id)}={holdImageModel.IDHoldImage}"));
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
        private async Task DeleteHoldImage(HoldImageModel holdImageModel)
        {
            try
            {
                if (IsProcessRunning || holdImageModel is null)
                    return;

                await SaveOrUpdateHoldInspection(false);

                IsProcessRunning = true;

                var holdImage = mapper.Map<HoldImage>(holdImageModel);

                if (await Shell.Current.DisplayAlert("Alerta", "Deseja realmente excluir?", "Sim", "Cancelar"))
                {
                    await new HoldImageBO().DeleteAsync(holdImage);
                    HoldImages.Remove(holdImageModel);
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