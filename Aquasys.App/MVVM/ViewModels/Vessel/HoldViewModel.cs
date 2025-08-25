using Aquasys.App.Core.BO;
using Aquasys.App.Core.Entities;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    [QueryProperty(nameof(IDVessel), nameof(IDVessel))]
    public partial class HoldViewModel : BaseViewModels
    {
        [ObservableProperty]
        private HoldModel holdModel;

        [ObservableProperty]
        private bool expanded = true;

        public long IDVessel { get; set; }

        public HoldViewModel()
        {
            holdModel = new();
        }

        public override async Task OnAppearing()
        {
            await PreencheDados();
        }

        private async Task PreencheDados()
        {
            if (Id.IsNotNullOrEmpty())
            {
                var hold = await new HoldBO().GetByIdAsync(Id.ToLong());
                HoldModel = mapper.Map<HoldModel>(hold);
            }
        }

        [RelayCommand]
        private async Task SaveHold()
        {
            await SaveOrUpdateHold();
        }

        private async Task SaveOrUpdateHold()
        {
            if(HoldModel?.IDHold is not null && HoldModel?.IDHold != 0)
            {
                var hold = await new HoldBO().GetByIdAsync(HoldModel?.IDHold ?? -1);
                if(hold is not null)
                {
                    hold = mapper.Map<Hold>(HoldModel);
                    if (await new HoldBO().UpdateAsync(hold))
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
                if (await new HoldBO().InsertAsync(hold))
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
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