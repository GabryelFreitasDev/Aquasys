using Aquasys.Controls.Editors;
using Aquasys.Core.Utils;
using Aquasys.MVVM.Views;
using CRM.Entidades;
using DevExpress.Maui.Editors;

namespace Aquasys.Controls
{

    public partial class DCCropImagePage : BasePages
    {
        private TaskCompletionSource<DCImagem> taskCompletionSourceImageEdit { get; set; }
        private DCImagem dcImagem;
        public DCCropImagePage(DCImagem dcImagem)
        {
            InitializeComponent();
            taskCompletionSourceImageEdit = new TaskCompletionSource<DCImagem>();
            this.dcImagem = dcImagem;
            ImageEdit.Source = dcImagem.ImageSource;
        }

        private async void TapGestureRecognizerSalvar_Tapped(object sender, TappedEventArgs e)
        {
            try
            {
                await DCLoadingScreen.Instance.Start();
                await CropImage();
            }
            catch (Exception ex)
            {
                //await DCMessages.DisplaySnackBarAsync(ex.Message);
            }
            finally
            {
                await DCLoadingScreen.Instance.Stop();
            }
            await NavigationUtils.PopAsync(false);
            taskCompletionSourceImageEdit.SetResult(dcImagem);
        }

        private async void TapGestureRecognizerVoltar_Tapped(object sender, TappedEventArgs e)
        {
            await NavigationUtils.PopAsync(false);
        }

        private async Task CropImage()
        {
            ImageSource imageSource = ImageEdit.SaveAsImageSource();
            using var stream = await ((StreamImageSource)imageSource).Stream(CancellationToken.None);

            byte[] dados = await DCUtils.ToByteArrayAsync(stream);
            dcImagem.Content = dados;
        }

        public Task<DCImagem> WaitForResultAsync()
        {
            return taskCompletionSourceImageEdit.Task;
        }
    }
}
