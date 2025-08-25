using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace Aquasys.App.Controls
{
    public partial class DCLoadingScreen : PopupPage
    {
        private static DCLoadingScreen instance;
        public static DCLoadingScreen Instance
        {
            get
            {
                if (instance == null)
                    instance = new DCLoadingScreen();
                return instance;
            }
        }

        private static bool Open;

        private DCLoadingScreen()
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
        }

        public async Task Start(bool transparentBackground = false)
        {
            if (Open)
                return;

            if (transparentBackground)
                BackgroundColor = Colors.Transparent;

            Open = true;

            await PopupNavigation.Instance.PushAsync(instance);
        }

        public async Task Stop()
        {
            if (Open)
            {
                Open = false;
                await PopupNavigation.Instance.RemovePageAsync(instance);

                if (BackgroundColor == Colors.Transparent)
                    instance = null;
            }
        }

        public static async Task ShowLoadingWhileTaskIsRunningAsync(Func<Task> task, bool transparentBackground = false)
        {
            DCLoadingScreen loading = new();
            try
            {

                await loading.Start(transparentBackground);

                if (task != null)
                    await Task.Run(task).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
            finally
            {
                await loading.Stop();
            }
        }

    }
}