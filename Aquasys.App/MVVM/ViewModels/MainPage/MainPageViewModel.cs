using Aquasys.App.Core.Utils;
using Aquasys.App.MVVM.Views.Vessel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Aquasys.App.MVVM.ViewModels.MainPage
{
    public partial class MainPageViewModel : BaseViewModels // Supondo que você tenha uma BaseViewModels
    {
        [ObservableProperty]
        private string _userName;

        public MainPageViewModel()
        {
            // Carrega o nome do usuário do contexto global que definimos no login
            _userName = ContextUtils.ContextUser?.UserName ?? "Usuário";
        }

        [RelayCommand]
        private async Task NavigateToVesselModule()
        {
            // Navega para a página de lista de embarcações
            await Shell.Current.GoToAsync(nameof(VesselListPage));
        }
    }
}