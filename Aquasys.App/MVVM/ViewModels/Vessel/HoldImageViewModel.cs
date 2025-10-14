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
    public partial class HoldInspectionImageViewModel : BaseViewModels
    {
        private readonly ILocalRepository<HoldInspectionImage> _HoldInspectionImageRepository;

        [ObservableProperty]
        private HoldInspectionImageModel _HoldInspectionImageModel;

        public HoldInspectionImageViewModel(ILocalRepository<HoldInspectionImage> HoldInspectionImageRepository)
        {
            _HoldInspectionImageRepository = HoldInspectionImageRepository;
            _HoldInspectionImageModel = new();
        }

        public override async Task OnAppearing()
        {
            await PreencheDados();
        }

        private async Task PreencheDados()
        {
            if (Id.IsNotNullOrEmpty())
            {
                var HoldInspectionImage = await _HoldInspectionImageRepository.GetByIdAsync(Id.ToLong());
                HoldInspectionImageModel = mapper.Map<HoldInspectionImageModel>(HoldInspectionImage);
            }
        }

        [RelayCommand]
        private async Task SaveHoldInspectionImage()
        {
            if (HoldInspectionImageModel.IDHoldInspectionImage != 0)
            {
                var HoldInspectionImage = await _HoldInspectionImageRepository.GetByIdAsync(HoldInspectionImageModel.IDHoldInspectionImage);
                if (HoldInspectionImage is not null)
                {
                    HoldInspectionImage = mapper.Map<HoldInspectionImage>(HoldInspectionImageModel);
                    if (await _HoldInspectionImageRepository.UpdateAsync(HoldInspectionImage))
                    {
                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                        await Shell.Current.GoToAsync("..", true);
                    }
                }
            }
            else
            {
                var HoldInspectionImage = mapper.Map<HoldInspectionImage>(HoldInspectionImageModel);
                if (await _HoldInspectionImageRepository.InsertAsync(HoldInspectionImage))
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
        }
    }
}