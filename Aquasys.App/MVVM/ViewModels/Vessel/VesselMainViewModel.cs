using Aquasys.App.MVVM.ViewModels.Vessel.Tabs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Aquasys.App.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselMainViewModel : BaseViewModels
    {

        [ObservableProperty]
        public VesselRegistrationViewModel vesselRegistrationTabViewModel;

        [ObservableProperty]
        public VesselHoldRegistrationTabViewModel vesselHoldRegistrationTabViewModel;

        private int tabSelecionada = 0;

        public VesselMainViewModel(
            VesselRegistrationViewModel vesselRegistrationTabViewModel,
            VesselHoldRegistrationTabViewModel vesselHoldRegistrationTabViewModel)
        {

            VesselRegistrationTabViewModel = vesselRegistrationTabViewModel;
            VesselHoldRegistrationTabViewModel = vesselHoldRegistrationTabViewModel;
        }

        public override async Task OnAppearing()
        {
            await LoadDataTab(tabSelecionada);
        }

        private async Task LoadDataTab(int index)
        {
            //if (string.IsNullOrEmpty(Id))
            //    return;

            switch (index)
            {
                case 0://aba Informacoes
                    VesselRegistrationTabViewModel.Id = Id;
                    await VesselRegistrationTabViewModel.OnAppearing();
                    break;
                case 1://aba propriedades
                    VesselHoldRegistrationTabViewModel.IDVessel = Id.ToInt64();
                    await VesselHoldRegistrationTabViewModel.OnAppearing();
                    break;
            }

            tabSelecionada = index;
        }

        [RelayCommand]
        public async Task AlterTab(int index)
        {
            await LoadDataTab(index);
        }
    }
}