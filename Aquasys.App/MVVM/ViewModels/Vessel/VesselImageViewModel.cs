using Aquasys.App.Core.Intefaces;
using Aquasys.Core.Entities;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Aquasys.App.Core.Data;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselImageViewModel : BaseViewModels
    {
        private readonly ILocalRepository<VesselImage> _vesselImageRepository;

        [ObservableProperty]
        private VesselImageModel _vesselImageModel;

        public VesselImageViewModel(ILocalRepository<VesselImage> vesselImageRepository)
        {
            _vesselImageRepository = vesselImageRepository;
            _vesselImageModel = new();
        }

        public override async Task OnAppearing()
        {
            await PreencheDados();
        }

        private async Task PreencheDados()
        {
            if (Id.IsNotNullOrEmpty())
            {
                var vesselImage = await _vesselImageRepository.GetByIdAsync(Id.ToLong());
                VesselImageModel = mapper.Map<VesselImageModel>(vesselImage);
            }
        }

        [RelayCommand]
        private async Task SaveVesselImage()
        {
            if (IsProcessRunning)
                return;

            try
            {
                IsProcessRunning = true;

                var vesselImage = mapper.Map<VesselImage>(VesselImageModel);
                await _vesselImageRepository.UpsertAsync(vesselImage);

                await Shell.Current.DisplayAlert("Alerta", "Salvo com sucesso", "OK");
                await Shell.Current.GoToAsync("..", true);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao salvar imagem: {ex.Message}", "OK");
            }
            finally
            {
                IsProcessRunning = false;
            }
        }
    }
}