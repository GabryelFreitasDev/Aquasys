using Aquasys.Core.BO;
using Aquasys.MVVM.Models.Vessel;
using Aquasys.MVVM.ViewModels.Vessel.Tabs;
using Aquasys.MVVM.Views.Vessel.Tabs;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Aquasys.MVVM.ViewModels.Vessel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class VesselImageViewModel : BaseViewModels
    {
        public VesselImageViewModel()
        {
           
        }

        public override async void OnAppearing()
        {
        }

    }
}