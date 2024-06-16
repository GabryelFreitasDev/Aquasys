using Aquasys.Core.BO;
using Aquasys.Core.Entities;
using Aquasys.Core.Utils;
using Aquasys.MVVM.Models.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Aquasys.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselImageViewModel : BaseViewModels
    {
        [ObservableProperty]
        private VesselImageModel vesselImageModel;

        public VesselImageViewModel()
        {
            vesselImageModel = new();
        }

        public override async void OnAppearing()
        {
            await PreencheDados();
        }

        private async Task PreencheDados()
        {
            if (Id.IsNotNullOrEmpty())
            {
                var vesselImage = await new VesselImageBO().GetByIdAsync(Id.ToLong());
                VesselImageModel = mapper.Map<VesselImageModel>(vesselImage);
            }
        }

        [RelayCommand]
        private async Task SaveVesselImage()
        {
            await SaveOrUpdateVesselImage();
        }

        private async Task SaveOrUpdateVesselImage()
        {
            if(VesselImageModel?.IDVesselImage is not null && VesselImageModel?.IDVesselImage != 0)
            {
                var vesselImage = await new VesselImageBO().GetByIdAsync(VesselImageModel?.IDVesselImage ?? -1);
                if(vesselImage is not null)
                {
                    vesselImage = mapper.Map<VesselImage>(VesselImageModel);
                    if (await new VesselImageBO().UpdateAsync(vesselImage))
                    {
                        await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                        await Shell.Current.GoToAsync("..", true);
                    }
                }
            }
            else
            {
                var vesselImage = mapper.Map<VesselImage>(VesselImageModel);

                if (await new VesselImageBO().InsertAsync(vesselImage))
                {
                    await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                    await Shell.Current.GoToAsync("..", true);
                }
            }
            
            
        }

    }
}