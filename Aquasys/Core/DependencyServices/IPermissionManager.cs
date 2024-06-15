namespace CRM.Mobile.Core.DependencyServices
{
    public interface IPermissionManager
    {
        /// <summary>
        ///android.permission.READ_MEDIA_IMAGES
        ///android.permission.READ_MEDIA_VIDEO
        ///android.permission.READ_MEDIA_AUDIO
        /// </summary>
        /// <returns></returns>
        Task<bool> RequestPermission(string[] permissions);


        /// <summary>
        ///appTrackingTransparencyService - Apenas no IOS
        /// </summary>
        /// <returns></returns>
        Task<PermissionStatus> CheckStatusTrackingTransparencyAsync();

        /// <summary>
        /// appTrackingTransparencyService - Apenas no IOS
        /// </summary>
        /// <param name="completionAction"></param>
        /// <returns></returns>
        Task RequestTrackingTransparencyAsync(Action<PermissionStatus> completionAction);

    }
}
