using Aquasys.MVVM.ViewModels.MainPage;

namespace Aquasys.MVVM.Views.MainPage
{
    public partial class MainPageView : ContentPage
    {
        public MainPageView()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            Shell.Current.Navigation.PopAsync();
            return true;
        }
    }

}
