using Aquasys.MVVM.ViewModels.Login;

namespace Aquasys.MVVM.Views.Login;

public partial class CreateAccountView : ContentPage
{
	public CreateAccountView()
	{
		InitializeComponent();
		BindingContext = new CreateAccontViewModel();
	}
}