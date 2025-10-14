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
    [QueryProperty(nameof(IDHold), nameof(IDHold))]
    public partial class HoldInspectionViewModel : BaseViewModels
    {
        private readonly ILocalRepository<HoldInspection> _holdInspectionRepository;
        private readonly ILocalRepository<HoldInspectionCondition> _holdConditionRepository;
        private readonly ILocalRepository<HoldInspectionImage> _holdImageRepository;

        [ObservableProperty]
        private HoldInspectionModel holdInspectionModel;

        [ObservableProperty]
        private HoldInspectionConditionModel holdConditionModel;

        [ObservableProperty]
        private ObservableCollection<HoldInspectionImageModel> holdImages;

        [ObservableProperty] private bool _expanded = true;
        [ObservableProperty] private bool _hasImages = false;
        public long IDHold { get; set; }

        public HoldInspectionViewModel(
            ILocalRepository<HoldInspection> holdInspectionRepository,
            ILocalRepository<HoldInspectionCondition> holdConditionRepository,
            ILocalRepository<HoldInspectionImage> holdImageRepository)
        {
            _holdInspectionRepository = holdInspectionRepository;
            _holdConditionRepository = holdConditionRepository;
            _holdImageRepository = holdImageRepository;

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
                var holdInspection = await _holdInspectionRepository.GetByIdAsync(Id.ToLong());
                HoldInspectionModel = mapper.Map<HoldInspectionModel>(holdInspection);

                var holdConditionList = await _holdConditionRepository.GetFilteredAsync(x => x.IDHoldInspection == HoldInspectionModel.IDHoldInspection);
                HoldConditionModel = mapper.Map<HoldInspectionConditionModel>(holdConditionList.FirstOrDefault()) ?? new();

                var holdImagesList = await _holdImageRepository.GetFilteredAsync(x => x.IDHold == HoldInspectionModel.IDHold);
                HoldImages = new ObservableCollection<HoldInspectionImageModel>(mapper.Map<List<HoldInspectionImageModel>>(holdImagesList));
                HasImages = HoldImages.Any();
            }
        }

        [RelayCommand]
        private async Task SaveHoldInspection(bool mostraMensagem = true)
        {
            // Lógica para salvar HoldInspection
            if (HoldInspectionModel.IDHoldInspection != -1)
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
                await _holdInspectionRepository.InsertAsync(holdInspection);
                HoldInspectionModel.IDHoldInspection = holdInspection.IDHoldInspection;
            }

            // Lógica para salvar HoldCondition
            if (HoldConditionModel.IDHoldInspectionCondition != 0)
            {
                var holdCondition = await _holdConditionRepository.GetByIdAsync(HoldConditionModel.IDHoldInspectionCondition);
                if (holdCondition != null)
                {
                    holdCondition = mapper.Map<HoldInspectionCondition>(HoldConditionModel);
                    await _holdConditionRepository.UpdateAsync(holdCondition);
                }
            }
            else
            {
                var holdCondition = mapper.Map<HoldInspectionCondition>(HoldConditionModel);
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