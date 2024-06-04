using Aquasys.MVVM.ViewModels.Login;

namespace Aquasys.MVVM.Views.Login;

public partial class CreateAccountPage : ContentPage
{
	public CreateAccountPage()
	{
		InitializeComponent();
		BindingContext = new CreateAccontViewModel();
	}
}