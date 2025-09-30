using Aquasys.App.Core.Data;
using Aquasys.App.Core.Intefaces;
using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Models.Vessel;
using Aquasys.App.MVVM.ViewModels.Vessel.Tabs;
using Aquasys.App.MVVM.Views.Vessel.Tabs; // Using para as páginas das abas
using Aquasys.Core.Entities;
using Aquasys.Reports.Enums;
using Aquasys.Reports.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselMainViewModel : BaseViewModels
    {
        private readonly ILocalRepository<Aquasys.Core.Entities.Vessel> _vesselRepository;
        private readonly ReportGeneratorService _reportService;

        // Propriedades para segurar as PÁGINAS das abas
        [ObservableProperty]
        public VesselRegistrationTabPage _vesselRegistrationTabPage;

        [ObservableProperty]
        public VesselHoldRegistrationTabPage _vesselHoldRegistrationTabPage;

        [ObservableProperty]
        public VesselInspectionRegistrationTabPage _vesselInspectionRegistrationTabPage;

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                // Antes de mudar o valor, salva a aba anterior
                SaveCurrentTabData().ContinueWith(t => {
                    // Garante que a atualização da propriedade ocorra na thread da UI
                    MainThread.BeginInvokeOnMainThread(() => {
                        SetProperty(ref _selectedTabIndex, value);
                    });
                });
            }
        }

        public VesselMainViewModel(
            ILocalRepository<Aquasys.Core.Entities.Vessel> vesselRepository,
            // O construtor agora recebe as PÁGINAS, já prontas e com suas ViewModels
            VesselRegistrationTabPage vesselRegistrationTabPage,
            VesselHoldRegistrationTabPage vesselHoldRegistrationTabPage,
            VesselInspectionRegistrationTabPage vesselInspectionRegistrationTabPage,
            ReportGeneratorService reportService)
        {
            _vesselRepository = vesselRepository;
            _reportService = reportService;

            // Atribui as páginas injetadas às propriedades
            _vesselRegistrationTabPage = vesselRegistrationTabPage;
            _vesselHoldRegistrationTabPage = vesselHoldRegistrationTabPage;
            _vesselInspectionRegistrationTabPage = vesselInspectionRegistrationTabPage;
        }

        public override async Task OnAppearing()
        {
            await LoadTabs();
        }

        private async Task SaveCurrentTabData()
        {
            switch (_selectedTabIndex)
            {
                case 0:
                    var vmRegistration = VesselRegistrationTabPage.BindingContext as VesselRegistrationTabViewModel;
                    if (vmRegistration != null) await vmRegistration.SaveOrUpdateVessel(false);
                    break;
                    // Adicione casos para outras abas se elas também precisarem salvar dados
                    // case 1:
                    //     var vmHold = VesselHoldRegistrationTabPage.BindingContext as VesselHoldRegistrationTabViewModel;
                    //     if (vmHold != null) await vmHold.SaveData(); // (Este método precisaria ser criado)
                    //     break;
            }
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