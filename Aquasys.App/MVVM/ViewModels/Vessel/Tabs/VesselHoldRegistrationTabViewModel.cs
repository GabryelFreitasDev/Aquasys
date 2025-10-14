using Aquasys.App.Core.Intefaces;
using Aquasys.Core.Entities;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.App.MVVM.Views.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Aquasys.App.Core.Data;

namespace Aquasys.App.MVVM.ViewModels.Vessel.Tabs
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselHoldRegistrationTabViewModel : BaseViewModels
    {
        private readonly ILocalRepository<Hold> _holdRepository;

        public long IDVessel { get; set; }

        [ObservableProperty]
        private ObservableCollection<HoldModel> holds;

        public VesselHoldRegistrationTabViewModel(ILocalRepository<Hold> holdRepository)
        {
            _holdRepository = holdRepository;
            holds = new();
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
                if (await Shell.Current.DisplayAlert("Alerta", "Deseja realmente excluir?", "Sim", "Cancelar"))
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
    }
}