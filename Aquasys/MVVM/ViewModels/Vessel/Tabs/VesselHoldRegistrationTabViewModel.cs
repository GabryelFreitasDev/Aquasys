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
        private ObservableCollection<HoldModel> holds;

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
            if (IDVessel != 0)
            {
                var holds = await holdBO.GetFilteredAsync(x => x.IDVessel == IDVessel);
                ObservableCollection<HoldModel> vesselImagesModel = new();

                holds.ForEach(x => vesselImagesModel.Add(mapper.Map<HoldModel>(x)));

                Holds = vesselImagesModel;
            }
        }

        [RelayCommand]
        private async Task AddHold()
        {
            await Shell.Current.GoToAsync($"{nameof(HoldPage)}?{nameof(IDVessel)}={IDVessel}");
        }

        [RelayCommand]
        private async Task EditHold(HoldModel holdModel)
        {
            if (holdModel is not null)
                await Shell.Current.GoToAsync($"{nameof(HoldPage)}?{nameof(Id)}={holdModel.IDHold}");
        }

        [RelayCommand]
        private async Task DeleteHold(HoldModel holdModel)
        {
            try
            {
                if (IsProcessRunning || holdModel is null)
                    return;

                IsProcessRunning = true;

                var hold = mapper.Map<Hold>(holdModel);

                if (await Shell.Current.DisplayAlert("Alerta", "Deseja realmente excluir?", "Sim", "Cancelar"))
                    await holdBO.DeleteAsync(hold);
                Holds.Remove(holdModel);
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
