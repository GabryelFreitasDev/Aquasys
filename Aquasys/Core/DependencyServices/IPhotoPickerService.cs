namespace Aquasys.Core.DependencyServices
{
    public interface IPhotoPickerService
    {
        Task<(string Path, byte[] Photo)> CapturePhotoAsync();

        void CapturePhotoResult(int requestCode, int resultCode);
    }
}
