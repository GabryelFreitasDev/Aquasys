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
using DevExpress.Maui.Core.Internal;
using System.Collections.ObjectModel;

namespace Aquasys.MVVM.ViewModels.Vessel.Tabs
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselInspectionRegistrationTabViewModel : BaseViewModels
    {
        public long IDVessel;
        public long IDInspection;
        public long IDHold;

        [ObservableProperty]
        private InspectionModel inspectionModel;

        [ObservableProperty]
        private ObservableCollection<HoldModel> holds;

        [ObservableProperty]
        private bool expanded = true;

        private InspectionBO inspectionBO;

        public VesselInspectionRegistrationTabViewModel()
        {
            InspectionModel = new();
            Holds = new();
            inspectionBO = new();
        }

        public override async void OnAppearing()
        {
            await CarregaDados();
        }

        private async Task CarregaDados()
        {
            if (IDVessel != 0)
            {

                await CarregaInspection();
                await CarregaHolds();
            }
        }

        private async Task CarregaInspection()
        {
            var inspection = await inspectionBO.GetFilteredAsync(x => x.IDVessel == IDVessel);
            InspectionModel = mapper.Map<InspectionModel>(inspection.FirstOrDefault()) ?? new();
        }

        private async Task CarregaHolds()
        {
            var holds = await new HoldBO().GetFilteredAsync(x => x.IDVessel == IDVessel);
            ObservableCollection<HoldModel> holdsModel = new();

            holds.ForEach(x => holdsModel.Add(mapper.Map<HoldModel>(x)));

            HoldInspectionBO holdInspectionBO = new();
            foreach(HoldModel hold in holdsModel)
            {
                var has = await holdInspectionBO.GetFilteredAsync(y => y.IDHold == hold.IDHold);
                hold.Inspectioned = has.Any() ? true : false;
            }

            Holds = holdsModel;
        }

        [RelayCommand]
        private async Task Save()
        {
            //if (await ValidateVessel())
                await SaveOrUpdateVessel(false);
        }

        private async Task SaveOrUpdateVessel(bool mostraMensagem = true)
        {
            if (InspectionModel?.IDInspection is not null && InspectionModel?.IDInspection != 0)
            {
                var inspectionExists = await inspectionBO.GetByIdAsync(InspectionModel?.IDInspection ?? -1);
                if (inspectionExists is not null)
                {
                    inspectionExists = mapper.Map<Inspection>(InspectionModel);
                    if (await inspectionBO.UpdateAsync(inspectionExists) && mostraMensagem)
                    {
                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                        //await Shell.Current.GoToAsync("..", true);
                    }
                }
            }
            else
            {
                var inspectionSave = mapper.Map<Inspection>(InspectionModel);
                inspectionSave.IDVessel = IDVessel;
                inspectionSave.RegistrationDateTime = DateTime.Now;

                if (await inspectionBO.InsertAsync(inspectionSave) && mostraMensagem)
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    //await Shell.Current.GoToAsync("..", true);
                }

                InspectionModel = mapper.Map<InspectionModel>(inspectionSave);
            }
        }

        [RelayCommand]
        private async Task InspectionHold(HoldModel holdModel)
        {
            var holdInspection = await new HoldInspectionBO().GetFilteredAsync(x => x.IDHold == holdModel.IDHold);
            if(holdInspection?.Any() ?? false)
                await Shell.Current.GoToAsync($"{nameof(HoldInspectionPage)}?{nameof(Id)}={holdInspection?.FirstOrDefault()?.IDHoldInspection}");
            else
                await Shell.Current.GoToAsync($"{nameof(HoldInspectionPage)}?{nameof(IDInspection)}={InspectionModel.IDInspection}&{nameof(IDHold)}={holdModel.IDHold}");
        }

        [RelayCommand]
        private async Task Expand(VesselImageModel vesselImageModel)
        {
            if (Expanded == true)
                Expanded = false;
            else
                Expanded = true;
        }
    }
}
