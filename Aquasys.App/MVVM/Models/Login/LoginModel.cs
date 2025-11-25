using CommunityToolkit.Mvvm.ComponentModel;

namespace Aquasys.App.MVVM.Models.Login
{
    public partial class LoginModel : BaseModels
    {
        [ObservableProperty]
        public string userName;
        [ObservableProperty]
        public string? email;
        [ObservableProperty]
        public string password;
        [ObservableProperty]
        public bool rememberMe = true;
        [ObservableProperty]
        public string apiIp;
        [ObservableProperty]
        public string apiPort;
    }
}
