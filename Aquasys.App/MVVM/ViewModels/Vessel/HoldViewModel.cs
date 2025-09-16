using Aquasys.App.Core.Data;
using Aquasys.App.Core.Intefaces;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.Core.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    [QueryProperty(nameof(IDVessel), nameof(IDVessel))]
    public partial class HoldViewModel : BaseViewModels
    {
        private readonly ILocalRepository<Hold> _holdRepository;

        [ObservableProperty]
        private HoldModel _holdModel;

        [ObservableProperty]
        private bool _expanded = true;

        public long IDVessel { get; set; }

        public HoldViewModel(ILocalRepository<Hold> holdRepository)
        {
            _holdRepository = holdRepository;
            _holdModel = new();
        }

        public override async Task OnAppearing()
        {
            await PreencheDados();
        }

        private async Task PreencheDados()
        {
            if (Id.IsNotNullOrEmpty())
            {
                var hold = await _holdRepository.GetByIdAsync(Id.ToLong());
                HoldModel = mapper.Map<HoldModel>(hold);
            }
        }

        [RelayCommand]
        private async Task SaveHold()
        {
            if (HoldModel.IDHold != 0)
            {
                var hold = await _holdRepository.GetByIdAsync(HoldModel.IDHold);
                if (hold is not null)
                {
                    hold = mapper.Map<Hold>(HoldModel);
                    if (await _holdRepository.UpdateAsync(hold))
                    {
                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                        await Shell.Current.GoToAsync("..", true);
                    }
                }
            }
            else
            {
                var hold = mapper.Map<Hold>(HoldModel);
                hold.IDVessel = IDVessel;
                if (await _holdRepository.InsertAsync(hold))
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
        }

        [RelayCommand]
        private void Expand()
        {
            Expanded = !Expanded;
        }
    }
}