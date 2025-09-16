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

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    [QueryProperty(nameof(IDInspection), nameof(IDInspection))]
    [QueryProperty(nameof(IDHold), nameof(IDHold))]
    public partial class HoldInspectionViewModel : BaseViewModels
    {
        private readonly ILocalRepository<HoldInspection> _holdInspectionRepository;
        private readonly ILocalRepository<HoldCondition> _holdConditionRepository;
        private readonly ILocalRepository<HoldImage> _holdImageRepository;

        [ObservableProperty]
        private HoldInspectionModel _holdInspectionModel;

        [ObservableProperty]
        private HoldConditionModel _holdConditionModel;

        [ObservableProperty]
        private ObservableCollection<HoldImageModel> _holdImages;

        [ObservableProperty] private bool _expanded = true;
        [ObservableProperty] private bool _hasImages = false;

        public long IDInspection { get; set; }
        public long IDHold { get; set; }

        public HoldInspectionViewModel(
            ILocalRepository<HoldInspection> holdInspectionRepository,
            ILocalRepository<HoldCondition> holdConditionRepository,
            ILocalRepository<HoldImage> holdImageRepository)
        {
            _holdInspectionRepository = holdInspectionRepository;
            _holdConditionRepository = holdConditionRepository;
            _holdImageRepository = holdImageRepository;

            _holdInspectionModel = new();
            _holdConditionModel = new();
            _holdImages = new();
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

                var holdConditionList = await _holdConditionRepository.GetFilteredAsync(x => x.IDHoldInspection == HoldInspectionModel.IDHoldInspection);
                HoldConditionModel = mapper.Map<HoldConditionModel>(holdConditionList.FirstOrDefault()) ?? new();

                var holdImagesList = await _holdImageRepository.GetFilteredAsync(x => x.IDHold == HoldInspectionModel.IDHold);
                HoldImages = new ObservableCollection<HoldImageModel>(mapper.Map<List<HoldImageModel>>(holdImagesList));
                HasImages = HoldImages.Any();
            }
        }

        [RelayCommand]
        private async Task SaveHoldInspection(bool mostraMensagem = true)
        {
            // Lógica para salvar HoldInspection
            if (HoldInspectionModel.IDHoldInspection != 0)
            {
                var holdInspection = await _holdInspectionRepository.GetByIdAsync(HoldInspectionModel.IDHoldInspection);
                if (holdInspection != null)
                {
                    holdInspection = mapper.Map<HoldInspection>(HoldInspectionModel);
                    await _holdInspectionRepository.UpdateAsync(holdInspection);
                }
            }
            else
            {
                var holdInspection = mapper.Map<HoldInspection>(HoldInspectionModel);
                holdInspection.IDHold = IDHold;
                holdInspection.IDInspection = IDInspection;
                await _holdInspectionRepository.InsertAsync(holdInspection);
                HoldInspectionModel.IDHoldInspection = holdInspection.IDHoldInspection;
            }

            // Lógica para salvar HoldCondition
            if (HoldConditionModel.IDHoldCondition != 0)
            {
                var holdCondition = await _holdConditionRepository.GetByIdAsync(HoldConditionModel.IDHoldCondition);
                if (holdCondition != null)
                {
                    holdCondition = mapper.Map<HoldCondition>(HoldConditionModel);
                    await _holdConditionRepository.UpdateAsync(holdCondition);
                }
            }
            else
            {
                var holdCondition = mapper.Map<HoldCondition>(HoldConditionModel);
                holdCondition.IDHoldInspection = HoldInspectionModel.IDHoldInspection;
                await _holdConditionRepository.InsertAsync(holdCondition);
            }

            if (mostraMensagem)
            {
                await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                await Shell.Current.GoToAsync("..", true);
            }
        }

        // ... Outros RelayCommands como Add, Edit e Delete de imagens continuam aqui
        // Lembre-se de substituir 'new HoldImageBO()' por '_holdImageRepository' neles.
    }
}