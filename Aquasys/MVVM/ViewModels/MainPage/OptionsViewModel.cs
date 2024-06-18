using Aquasys.Core.Entities;
using Aquasys.Core.Utils;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Aquasys.MVVM.ViewModels.MainPage
{
    public partial class OptionsViewModel : BaseViewModels
    {
        [ObservableProperty]
        public User user;

        public OptionsViewModel() 
        {
            user = ContextUtils.ContextUser;
        }
    }
}
