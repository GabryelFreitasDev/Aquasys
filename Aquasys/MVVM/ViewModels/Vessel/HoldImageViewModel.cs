using Aquasys.Core.BO;
using Aquasys.Core.Entities;
using Aquasys.Core.Utils;
using Aquasys.MVVM.Models.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Aquasys.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class HoldImageViewModel : BaseViewModels
    {
        [ObservableProperty]
        private HoldImageModel holdImageModel;

        public HoldImageViewModel()
        {
            holdImageModel = new();
        }

        public override async Task OnAppearing()
        {
            await PreencheDados();
        }

        private async Task PreencheDados()
        {
            if (Id.IsNotNullOrEmpty())
            {
                var holdImage = await new HoldImageBO().GetByIdAsync(Id.ToLong());
                HoldImageModel = mapper.Map<HoldImageModel>(holdImage);
            }
        }

        [RelayCommand]
        private async Task SaveHoldImage()
        {
            await SaveOrUpdateHoldImage();
        }

        private async Task SaveOrUpdateHoldImage()
        {
            if(HoldImageModel?.IDHoldImage is not null && HoldImageModel?.IDHoldImage != 0)
            {
                var holdImage = await new HoldImageBO().GetByIdAsync(HoldImageModel?.IDHoldImage ?? -1);
                if(holdImage is not null)
                {
                    holdImage = mapper.Map<HoldImage>(HoldImageModel);
                    if (await new HoldImageBO().UpdateAsync(holdImage))
                    {
                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                        await Shell.Current.GoToAsync("..", true);
                    }
                }
            }
            else
            {
                var holdImage = mapper.Map<HoldImage>(HoldImageModel);

                if (await new HoldImageBO().InsertAsync(holdImage))
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
            
            
        }

    }
}