using Aquasys.App.MVVM.ViewModels.Login;

namespace Aquasys.App.MVVM.Views.Login;

public partial class CreateAccountPage : ContentPage
{
	public CreateAccountPage()
	{
		InitializeComponent();
		BindingContext = new CreateAccontViewModel();
	}
}