using Aquasys.App.Core.Intefaces;
using Aquasys.Core.Entities;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Aquasys.App.Core.Data;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class HoldImageViewModel : BaseViewModels
    {
        private readonly ILocalRepository<HoldImage> _holdImageRepository;

        [ObservableProperty]
        private HoldImageModel _holdImageModel;

        public HoldImageViewModel(ILocalRepository<HoldImage> holdImageRepository)
        {
            _holdImageRepository = holdImageRepository;
            _holdImageModel = new();
        }

        public override async Task OnAppearing()
        {
            await PreencheDados();
        }

        private async Task PreencheDados()
        {
            if (Id.IsNotNullOrEmpty())
            {
                var holdImage = await _holdImageRepository.GetByIdAsync(Id.ToLong());
                HoldImageModel = mapper.Map<HoldImageModel>(holdImage);
            }
        }

        [RelayCommand]
        private async Task SaveHoldImage()
        {
            if (HoldImageModel.IDHoldImage != 0)
            {
                var holdImage = await _holdImageRepository.GetByIdAsync(HoldImageModel.IDHoldImage);
                if (holdImage is not null)
                {
                    holdImage = mapper.Map<HoldImage>(HoldImageModel);
                    if (await _holdImageRepository.UpdateAsync(holdImage))
                    {
                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                        await Shell.Current.GoToAsync("..", true);
                    }
                }
            }
            else
            {
                var holdImage = mapper.Map<HoldImage>(HoldImageModel);
                if (await _holdImageRepository.InsertAsync(holdImage))
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
        }
    }
}