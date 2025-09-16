using Aquasys.App.Core.Intefaces; // Importante para ILocalRepository
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

namespace Aquasys.App.MVVM.ViewModels.Vessel.Tabs
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselInspectionRegistrationTabViewModel : BaseViewModels
    {
        // --- Repositórios Injetados ---
        private readonly ILocalRepository<Inspection> _inspectionRepository;
        private readonly ILocalRepository<Hold> _holdRepository;
        private readonly ILocalRepository<HoldInspection> _holdInspectionRepository;

        // --- Propriedades da UI ---
        public long IDVessel { get; set; }
        public long IDInspection { get; set; }
        public long IDHold { get; set; }

        [ObservableProperty]
        private InspectionModel _inspectionModel;

        [ObservableProperty]
        private ObservableCollection<HoldModel> _holds;

        [ObservableProperty]
        private bool _expanded = true;

        /// <summary>
        /// Construtor que recebe todas as dependências necessárias via Injeção de Dependência.
        /// </summary>
        public VesselInspectionRegistrationTabViewModel(
            ILocalRepository<Inspection> inspectionRepository,
            ILocalRepository<Hold> holdRepository,
            ILocalRepository<HoldInspection> holdInspectionRepository)
        {
            _inspectionRepository = inspectionRepository;
            _holdRepository = holdRepository;
            _holdInspectionRepository = holdInspectionRepository;

            _inspectionModel = new();
            _holds = new();
        }

        public override async Task OnAppearing()
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
            var inspectionList = await _inspectionRepository.GetFilteredAsync(x => x.IDVessel == IDVessel);
            InspectionModel = mapper.Map<InspectionModel>(inspectionList.FirstOrDefault()) ?? new();
        }

        private async Task CarregaHolds()
        {
            var holdsList = await _holdRepository.GetFilteredAsync(x => x.IDVessel == IDVessel);
            var holdsModel = new ObservableCollection<HoldModel>(mapper.Map<List<HoldModel>>(holdsList));

            foreach (var hold in holdsModel)
            {
                var hasInspection = await _holdInspectionRepository.GetFilteredAsync(y => y.IDHold == hold.IDHold);
                hold.Inspectioned = hasInspection.Any();
            }

            Holds = holdsModel;
        }

        [RelayCommand]
        private async Task Save()
        {
            // if (await ValidateVessel()) // Se precisar de validação, pode adicionar aqui
            await SaveOrUpdateInspection(true);
        }

        private async Task SaveOrUpdateInspection(bool mostraMensagem = true)
        {
            if (InspectionModel?.IDInspection != 0)
            {
                var inspectionExists = await _inspectionRepository.GetByIdAsync(InspectionModel.IDInspection);
                if (inspectionExists is not null)
                {
                    inspectionExists = mapper.Map<Inspection>(InspectionModel);
                    if (await _inspectionRepository.UpdateAsync(inspectionExists) && mostraMensagem)
                    {
                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    }
                }
            }
            else
            {
                var inspectionSave = mapper.Map<Inspection>(InspectionModel);
                inspectionSave.IDVessel = IDVessel;
                inspectionSave.RegistrationDateTime = DateTime.Now;

                if (await _inspectionRepository.InsertAsync(inspectionSave) && mostraMensagem)
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                }
                InspectionModel = mapper.Map<InspectionModel>(inspectionSave); // Atualiza o modelo com o novo ID
            }
        }

        [RelayCommand]
        private async Task InspectionHold(HoldModel holdModel)
        {
            // Salva a inspeção principal antes de navegar, para garantir que ela tenha um ID
            await SaveOrUpdateInspection(false);

            var holdInspectionList = await _holdInspectionRepository.GetFilteredAsync(x => x.IDHold == holdModel.IDHold);
            if (holdInspectionList?.Any() ?? false)
            {
                await Shell.Current.GoToAsync($"{nameof(HoldInspectionPage)}?{nameof(Id)}={holdInspectionList.FirstOrDefault().IDHoldInspection}");
            }
            else
            {
                await Shell.Current.GoToAsync($"{nameof(HoldInspectionPage)}?{nameof(IDInspection)}={InspectionModel.IDInspection}&{nameof(IDHold)}={holdModel.IDHold}");
            }
        }

        [RelayCommand]
        private void Expand()
        {
            Expanded = !Expanded;
        }
    }
}