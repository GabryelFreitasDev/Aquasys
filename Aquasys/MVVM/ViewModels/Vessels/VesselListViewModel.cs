using System.Collections.ObjectModel;
using System.Windows.Input;
using Aquasys.Core.BO;
using Aquasys.MVVM.Models.Vessel;
using Aquasys.MVVM.Views.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Aquasys.MVVM.ViewModels.Vessel
{
    public partial class VesselListViewModel : BaseViewModel
    {
        VesselBO vesselBO = new VesselBO();

        [ObservableProperty]
        private ObservableCollection<VesselModel> vessels = new();

        public ICommand BtnAddClickCommand { get; set; }

        [RelayCommand]
        public async Task DeleteVessel(VesselModel vessel)
        {
            if(await Shell.Current.DisplayAlert("Alerta", "Deseja realmente excluir?", "Sim", "Cancelar"))
                if (vessel is not null)
                {
                    var vesselDeleted = await vesselBO.GetByIdAsync(vessel.IDVessel);
                    await vesselBO.DeleteAsync(vesselDeleted);
                    Vessels.Remove(vessel);
                }
        }

        [RelayCommand]
        public async Task EditVessel(VesselModel vessel)
        {
            if (vessel is not null)
            {
                await Shell.Current.GoToAsync(nameof(VesselMainView),
                new Dictionary<string, object>
                {
                    [nameof(Id)] = vessel.IDVessel.ToString()
                });
            }
        }


        public VesselListViewModel()
        {
            BtnAddClickCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(VesselMainView)));
        }

        public async Task LoadVesselsAsync()
        {
            var vessels = await vesselBO.GetAllAsync();
            vessels.OrderBy(x => x.VesselName);

            if (vessels is not null && vessels.Any())
            {
                Vessels = new ObservableCollection<VesselModel>();
                foreach (var vessel in vessels)
                {
                    if(!Vessels.Any(x => x.IDVessel == vessel.IDVessel))
                        Vessels.Add(new VesselModel
                        {
                            IDVessel = vessel.IDVessel,
                            VesselName = vessel.VesselName,
                            ManufacturingDate = vessel.ManufacturingDate ?? DateTime.Now
                        });
                }
            }
        }
    }
}
