using Aquasys.App.MVVM.ViewModels.MainPage;

namespace Aquasys.App.MVVM.Views.MainPage
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            Shell.Current.Navigation.PopAsync();
            return true;
        }
    }

}
