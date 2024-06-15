using Aquasys.Controls;
using CommunityToolkit.Maui.Views;

using RGPopup.Maui.Extensions;

namespace Aquasys.Core.Utils
{
    public static class NavigationUtils
    {
        private static bool _isPageTransitioning;

        public static async Task CloseLoading()
        {
            await DCLoadingScreen.Instance.Stop();
        }

        private static async Task StartLoading(bool showLoading)
        {
            if (!showLoading)
                return;

            await DCLoadingScreen.Instance.Start();
        }


        public static async Task PushAsync(Page page, bool animate = true, bool showLoading = true)
        {
            if (_isPageTransitioning)
                return;

            _isPageTransitioning = true;
            try
            {
                await StartLoading(showLoading);

                await Application.Current!.MainPage!.Navigation.PushAsync(page, animate);
            }
            finally
            {
                _isPageTransitioning = false;
            }
        }

        public static async Task PushPopupAsync(Popup page, bool animate = true)
        {
            if (_isPageTransitioning)
                return;

            _isPageTransitioning = true;
            try
            {
                await Application.Current!.MainPage!.ShowPopupAsync(page);
            }
            finally
            {
                _isPageTransitioning = false;
            }
        }

        public static async Task PushPopupAsync(RGPopup.Maui.Pages.PopupPage page, bool animate = false, bool showLoading = true)
        {
            if (_isPageTransitioning)
                return;

            _isPageTransitioning = true;
            try
            {
                await StartLoading(showLoading);
                await page.Navigation.PushPopupAsync(page, animate);
            }
            finally
            {
                _isPageTransitioning = false;
            }
        }




        // Exemplo:
        // Ao invés de: await Application.Current!.MainPage!.Navigation.PushPopupAsync(new ConsultaPedidoDeVendaFiltroPopUpPage(ConsultaPedidoDeVendaModel))
        // Faça: await NavigationUtils.PushPopupAsync<ConsultaPedidoDeVendaFiltroPopUpPage>(ConsultaPedidoDeVendaModel)
        public static async Task PushPopupAsync<T>(params object?[]? parameter)
        {
            if (_isPageTransitioning)
                return;

            _isPageTransitioning = true;

            try
            {
                await StartLoading(true);
                var pageType = typeof(T);
                var page = (RGPopup.Maui.Pages.PopupPage)Activator.CreateInstance(pageType, parameter);
                await page.Navigation.PushPopupAsync(page, false);
            }
            finally
            {
                _isPageTransitioning = false;
            }
        }

        public static async Task PopPopupAsync(bool animate = true)
        {
            if (_isPageTransitioning)
                return;

            _isPageTransitioning = true;
            try
            {
                await MainThread.InvokeOnMainThreadAsync(async () => await Application.Current!.MainPage!.Navigation.PopPopupAsync(animate));
            }
            finally
            {
                _isPageTransitioning = false;
            }
        }

        public static async Task GoToAsync(ShellNavigationState state, bool animate = false, bool showLoading = true)
        {
            if (_isPageTransitioning)
                return;

            _isPageTransitioning = true;
            try
            {
                await StartLoading(showLoading);


                await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync(state, animate));
            }
            finally
            {
                _isPageTransitioning = false;
            }
        }

        public static async Task PopToRootAsync()
        {
            await Application.Current!.MainPage!.Navigation.PopToRootAsync();
        }

        public static async Task PopAsync(bool animate = true)
        {
            await Application.Current!.MainPage!.Navigation.PopAsync(animate);
        }
    }
}
