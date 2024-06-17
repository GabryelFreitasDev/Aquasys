using Aquasys.Controls.Editors;
using Aquasys.Core.BO;
using Aquasys.Core.Entities;
using Aquasys.Core.Enums;
using Aquasys.Core.Utils;
using Aquasys.MVVM.Models.Vessel;
using Aquasys.MVVM.Views.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CountryData.Standard;
using System.Collections.ObjectModel;

namespace Aquasys.MVVM.ViewModels.Vessel.Tabs
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselHoldRegistrationTabViewModel : BaseViewModels
    {
        public long IDVessel;

        [ObservableProperty]
        private List<Hold> holds;

        [ObservableProperty]
        private bool expanded = true;

        [ObservableProperty]
        private bool hasImages = false;

        private HoldBO holdBO;

        public VesselHoldRegistrationTabViewModel()
        {
            holdBO = new();
            Holds = new();
        }

        public override async void OnAppearing()
        {
            await CarregaDados();
        }

        private async Task CarregaDados()
        {
            if(IDVessel != 0)
            {
                Holds = await holdBO.GetFilteredAsync(x => x.IDVessel == IDVessel);
            }
        }

        //[RelayCommand]
        //private async Task BtnSalvarClick()
        //{
        //    if(await ValidateVessel())
        //        await SaveOrUpdateVessel();
        //}

        //private async Task SaveOrUpdateVessel(bool mostraMensagem = true)
        //{
        //    if (VesselModel?.IDVessel is not null && VesselModel?.IDVessel != 0)
        //    {
        //        var vesselExists = await vesselBO.GetByIdAsync(VesselModel?.IDVessel ?? -1);
        //        if (vesselExists is not null)
        //        {
        //            vesselExists = mapper.Map<Core.Entities.Vessel>(VesselModel);
        //            if (await vesselBO.UpdateAsync(vesselExists) && mostraMensagem)
        //            {
        //                await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
        //                await Shell.Current.GoToAsync("..", true);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var vesselSave = mapper.Map<Core.Entities.Vessel>(VesselModel);
        //        vesselSave.IDUserRegistration = ContextUtils.ContextUser.IDUser;

        //        if (await vesselBO.InsertAsync(vesselSave) && mostraMensagem)
        //        {
        //            await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
        //            await Shell.Current.GoToAsync("..", true);
        //        }
        //    }
        //}

        [RelayCommand]
        private async Task AddHold()
        {
            //try
            //{
            //    if (IsProcessRunning)
            //        return;

            //    //await SaveOrUpdateVessel(false);

            //    IsProcessRunning = true;

            //    var anexo = (await DCFileSelector.GetImagens(1)).FirstOrDefault();
            //    if (anexo != null && anexo is DCImagem _anexo && (!_anexo.ImageSource?.IsEmpty ?? false))
            //    {
            //        VesselImage vesselImage = new VesselImage();
            //        vesselImage.Image = anexo.Content;
            //        vesselImage.IDVessel = VesselModel.IDVessel;

            //        await new VesselImageBO().InsertAsync(vesselImage);

            //        MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"{nameof(VesselImagePage)}?{nameof(Id)}={vesselImage.IDVesselImage}"));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            //finally
            //{
            //    IsProcessRunning = false;
            //}
        }
        //[RelayCommand]
        //private async Task EditHold(VesselImageModel vesselImageModel)
        //{
        //    try
        //    {
        //        if (IsProcessRunning || vesselImageModel is null)
        //            return;

        //        await SaveOrUpdateVessel(false);

        //        IsProcessRunning = true;

        //        MainThread.BeginInvokeOnMainThread(async () => await Shell.Current.GoToAsync($"{nameof(VesselImagePage)}?{nameof(Id)}={vesselImageModel.IDVesselImage}"));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        IsProcessRunning = false;
        //    }
        //}

        //[RelayCommand]
        //private async Task DeleteHold(VesselImageModel vesselImageModel)
        //{
        //    try
        //    {
        //        if (IsProcessRunning || vesselImageModel is null)
        //            return;

        //        await SaveOrUpdateVessel(false);

        //        IsProcessRunning = true;

        //        var vesselImage = mapper.Map<VesselImage>(vesselImageModel);

        //        if (await Shell.Current.DisplayAlert("Alerta", "Deseja realmente excluir?", "Sim", "Cancelar"))
        //            await vesselBO.DeleteAsync(vesselImage);
        //            Images.Remove(vesselImageModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        IsProcessRunning = false;
        //    }
        //}
    }
}
