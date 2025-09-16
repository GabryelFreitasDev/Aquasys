using Aquasys.App.Core.Intefaces;
using Aquasys.Core.Entities;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.App.MVVM.Views.Vessel.Tabs; // Using para as páginas das abas
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using Aquasys.App.Core.Data;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselMainViewModel : BaseViewModels
    {
        private readonly ILocalRepository<Aquasys.Core.Entities.Vessel> _vesselRepository;

        // Propriedades para segurar as PÁGINAS das abas
        [ObservableProperty]
        public VesselRegistrationTabPage _vesselRegistrationTabPage;

        [ObservableProperty]
        public VesselHoldRegistrationTabPage _vesselHoldRegistrationTabPage;

        [ObservableProperty]
        public VesselInspectionRegistrationTabPage _vesselInspectionRegistrationTabPage;

        public VesselMainViewModel(
            ILocalRepository<Aquasys.Core.Entities.Vessel> vesselRepository,
            // O construtor agora recebe as PÁGINAS, já prontas e com suas ViewModels
            VesselRegistrationTabPage vesselRegistrationTabPage,
            VesselHoldRegistrationTabPage vesselHoldRegistrationTabPage,
            VesselInspectionRegistrationTabPage vesselInspectionRegistrationTabPage)
        {
            _vesselRepository = vesselRepository;

            // Atribui as páginas injetadas às propriedades
            _vesselRegistrationTabPage = vesselRegistrationTabPage;
            _vesselHoldRegistrationTabPage = vesselHoldRegistrationTabPage;
            _vesselInspectionRegistrationTabPage = vesselInspectionRegistrationTabPage;
        }

        public override async Task OnAppearing()
        {
            await LoadTabs();
        }

        public async Task LoadTabs()
        {
            if (string.IsNullOrEmpty(Id))
            {
                // Para um novo Vessel, apenas chama OnAppearing para inicializações
                await (VesselRegistrationTabPage.BindingContext as BaseViewModels)?.OnAppearing();
                await (VesselHoldRegistrationTabPage.BindingContext as BaseViewModels)?.OnAppearing();
                return;
            }

            var vessel = await _vesselRepository.GetByIdAsync(Id.ToLong());
            if (vessel is not null)
            {
                // Pega a ViewModel de dentro da Página e passa os dados
                var vmRegistration = VesselRegistrationTabPage.BindingContext as Tabs.VesselRegistrationTabViewModel;
                if (vmRegistration != null)
                {
                    vmRegistration.VesselModel = mapper.Map<VesselModel>(vessel);
                    await vmRegistration.OnAppearing();
                }

                var vmHold = VesselHoldRegistrationTabPage.BindingContext as Tabs.VesselHoldRegistrationTabViewModel;
                if (vmHold != null)
                {
                    vmHold.IDVessel = vessel.IDVessel;
                    await vmHold.OnAppearing();
                }

                var vmInspection = VesselInspectionRegistrationTabPage.BindingContext as Tabs.VesselInspectionRegistrationTabViewModel;
                if (vmInspection != null)
                {
                    vmInspection.IDVessel = vessel.IDVessel;
                    await vmInspection.OnAppearing();
                }
            }
        }
    }
}