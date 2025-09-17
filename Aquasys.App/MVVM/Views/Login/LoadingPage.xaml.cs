namespace Aquasys.App.MVVM.Views.Login;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();

        Dispatcher.Dispatch(async () =>
        {
            // Opcional: uma pequena espera para a tela de loading ser visível.
            await Task.Delay(250);

            // 💡 Pede ao sistema de injeção de dependência para construir a LoginPage.
            // Neste ponto, tudo já estará inicializado corretamente.
            var loginPage = this.Handler.MauiContext.Services.GetService<LoginPage>();

            // Substitui a página de loading pela página de login.
            Application.Current.MainPage = new NavigationPage(loginPage);
        });
    }
}