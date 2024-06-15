using CRM.Mobile.Core.DependencyServices;

namespace Aquasys.Core.Utils
{
    public static class PermissionUtils
    {
        //public static async Task<PermissionStatus> CheckAndRequestLocationPermission()
        //{
        //    var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
        //    if (status == PermissionStatus.Granted)
        //        return status;

        //    if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        //        return status;

        //    if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.Android)
        //    {
        //        string mensagemColetaDaLocalizacao = @$"{AppInfo.Name} {TZ.MSG_LOCALIZACAO_PERMISSAOLOCALIZACAO()}";

        //        await DCPopUpConfimacao.Confirmacao(TZ.TXT_LOCALIZACAO(),
        //                                       message: mensagemColetaDaLocalizacao,
        //                                       cancelButtonLabel: TZ.TXT_NEGAR(),
        //                                       confirmButtonLabel: TZ.TXT_ACEITAR());
        //    }

        //    status = await Permissions.RequestAsync<Permissions.LocationAlways>();

        //    return status;
        //}

        public static async Task<PermissionStatus> CheckAndRequestCameraPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
                return status;

            if (Permissions.ShouldShowRationale<Permissions.Camera>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.Camera>();


            return status;
        }

        public static async Task<PermissionStatus> CheckAndRequestMicrophonePermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
                return status;

            if (Permissions.ShouldShowRationale<Permissions.Microphone>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.Microphone>();


            return status;
        }

        public static async Task<PermissionStatus> CheckAndRequestPhotoPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
                return status;

            if (Permissions.ShouldShowRationale<Permissions.Photos>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.Photos>();


            return status;
        }

        public static async Task<PermissionStatus> CheckAndRequestStorageReadPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
                return status;

            if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.StorageRead>();


            return status;
        }

        public static async Task<PermissionStatus> CheckAndRequestMedia()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Media>();
            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
                return status;

            if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.StorageRead>();


            return status;
        }


        public static async Task<bool> ValidPermissions()
        {
            //PermissionStatus LocationStatus = await PermissionUtils.CheckAndRequestLocationPermission();
            //bool ValidPermissionLocation = LocationStatus == PermissionStatus.Granted;

            PermissionStatus CameraStatus = await PermissionUtils.CheckAndRequestCameraPermission();
            bool ValidPermissionCamera = CameraStatus == PermissionStatus.Granted;

            PermissionStatus MicrophoneStatus = await PermissionUtils.CheckAndRequestMicrophonePermission();
            bool ValidPermissionMicrophone = MicrophoneStatus == PermissionStatus.Granted;

            bool ValidPermissionMedia = false;
            bool ValidPermissionPhoto = false;

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                PermissionStatus StorageStatus;
                if (DeviceInfo.Version.Major >= 13)
                    StorageStatus = await PermissionUtils.CheckAndRequestMedia();
                else StorageStatus = await PermissionUtils.CheckAndRequestStorageReadPermission();

                ValidPermissionMedia = StorageStatus == PermissionStatus.Granted;
                ValidPermissionPhoto = StorageStatus == PermissionStatus.Granted;
            }

            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                PermissionStatus PhotoStatus = await PermissionUtils.CheckAndRequestPhotoPermission();
                ValidPermissionPhoto = PhotoStatus == PermissionStatus.Granted;
            }

            return //ValidPermissionLocation ||
                ValidPermissionCamera || ValidPermissionMicrophone || ValidPermissionMedia || ValidPermissionPhoto;
        }


        public static async Task<PermissionStatus> CheckAndRequestTrackingTransparencyPermission()
        {
            try
            {
                if (DeviceInfo.Platform != DevicePlatform.iOS)
                    return PermissionStatus.Granted;

                var appTrackingTransparencyService = DependencyService.
                   Get<IPermissionManager>();

                var status = await appTrackingTransparencyService?.CheckStatusTrackingTransparencyAsync();

                if (status != PermissionStatus.Granted)
                {
                    await appTrackingTransparencyService.
                        RequestTrackingTransparencyAsync(s =>
                        {
                            if (status != PermissionStatus.Granted)
                                return;
                        });
                }
                return status;
            }
            catch (Exception ex)
            {
                //await DCMessages.DisplaySnackBarAsync(ex.Message);
            }

            return PermissionStatus.Granted;
        }
    }
}
