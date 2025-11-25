using Aquasys.App.Core.Data;
using Aquasys.App.Core.Intefaces;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.App.MVVM.Views.Vessel;
using Aquasys.Core.Entities;
using Aquasys.Reports.Enums;
using Aquasys.Reports.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace Aquasys.App.MVVM.ViewModels.Vessel.Tabs
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselHoldRegistrationTabViewModel : BaseViewModels
    {
        private readonly ILocalRepository<Aquasys.Core.Entities.Vessel> _vesselRepository;
        private readonly ILocalRepository<Hold> _holdRepository;
        private readonly ILocalRepository<HoldInspection> _holdInspectionRepository;
        private readonly ReportGeneratorService _reportService;

        [ObservableProperty]
        private VesselModel vesselModel;

        [ObservableProperty]
        private HoldModel holdModel;

        [ObservableProperty]
        private HoldInspectionModel holdInspectionModel;

        public long IDVessel { get; set; }

        [ObservableProperty]
        private ObservableCollection<HoldModel> holds;

        public VesselHoldRegistrationTabViewModel(
            ILocalRepository<Hold> holdRepository,
            ReportGeneratorService reportService,
            ILocalRepository<Aquasys.Core.Entities.Vessel> vesselRepository,
            ILocalRepository<HoldInspection> holdInspectionRepository)
        {
            _holdRepository = holdRepository;
            holds = new();
            _reportService = reportService;
            _vesselRepository = vesselRepository;
            _holdInspectionRepository = holdInspectionRepository;
        }

        public override async Task OnAppearing()
        {
            await CarregaDados();
        }

        private async Task CarregaDados()
        {
            if (IDVessel != -1)
            {
                var holdsData = await _holdRepository.GetFilteredAsync(x => x.IDVessel == IDVessel);
                Holds = new ObservableCollection<HoldModel>(mapper.Map<List<HoldModel>>(holdsData));
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
            if (IsProcessRunning || holdModel is null) return;

            try
            {
                IsProcessRunning = true;
                if (await Shell.Current.DisplayAlert("Warning", "Are you sure you want to delete?", "Yes", "Cancel"))
                {
                    var hold = await _holdRepository.GetByIdAsync(holdModel.IDHold);
                    await _holdRepository.DeleteAsync(hold);
                    Holds.Remove(holdModel);
                }
            }
            finally
            {
                IsProcessRunning = false;
            }
        }

        [RelayCommand]
        private async Task GenerateReport()
        {
            var vesselData = await _vesselRepository.GetByIdAsync(IDVessel);
            var holdsData = await _holdRepository.GetFilteredAsync(x => x.IDVessel == IDVessel);

            var holdIDs = holdsData.Select(h => h.IDHold).ToList();
            var inspectionsData = await _holdInspectionRepository.GetFilteredAsync(i => holdIDs.Contains(i.IDHold));

            if (vesselData != null)
            {
                VesselModel = mapper.Map<VesselModel>(vesselData);
                VesselModel.Holds = mapper.Map<List<HoldModel>>(holdsData);
            }

            try
            {
                var mappedEntityHolds = VesselModel.Holds.Select(hm => mapper.Map<Aquasys.Core.Entities.Hold>(hm)).ToList();

                foreach (var entityHold in mappedEntityHolds)
                {
                    var inspections = inspectionsData.Where(i => i.IDHold == entityHold.IDHold).ToList();
                    entityHold.Inspections = inspections;
                }

                var vesselEntity = mapper.Map<Aquasys.Core.Entities.Vessel>(VesselModel);
                vesselEntity.Holds = mappedEntityHolds;

                var pdfBytes = await _reportService.GenerateAsync(ReportType.Vessel, vesselEntity);

                var fileName = $"Report_{VesselModel.VesselName}.pdf";
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                File.WriteAllBytes(filePath, pdfBytes);

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Open/Share Report",
                    File = new ShareFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error generating report", ex.Message, "OK");
            }
        }
    }
}