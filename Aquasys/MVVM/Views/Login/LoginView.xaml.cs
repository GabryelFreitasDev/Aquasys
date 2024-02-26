using Aquasys.Core.BO;
using Aquasys.Core.Entities;
using Aquasys.MVVM.ViewModels.Login;
using Aquasys.Utils;

namespace Aquasys.MVVM.Views.Login;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }

    protected async override void OnAppearing()
    {
        var userRemember = await new UserBO().GetFilteredAsync<User>(x => x.RememberMe == true);

        if (userRemember?.Any() ?? false)
        {
            new ContextUtils(userRemember?.FirstOrDefault() ?? new());
            Application.Current!.MainPage = new AppShell();
        }
        base.OnAppearing();
        //else
            //await Application.Current!.MainPage!.Navigation.PushAsync(new LoginView());
    }
}