using System.Windows.Input;

namespace Aquasys.App.MVVM.ViewModels.MainPage
{
    public class MainPageViewModel
    {
        public ICommand BtnVesselClickCommand { get; set; }

        public MainPageViewModel() {
            BtnVesselClickCommand = new Command(async () =>
            {
                //await Shell.Current.GoToAsync(nameof(VesselListView));
            });

        }
    }
}
