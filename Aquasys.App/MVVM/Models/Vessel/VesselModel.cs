using CommunityToolkit.Mvvm.ComponentModel;

namespace Aquasys.App.MVVM.Models.Vessel
{
    public partial class VesselModel : BaseModels
    {
        public VesselModel()
        {
        }

        public long IDVessel { get; set; } = -1;

        [ObservableProperty]
        private string os;

        [ObservableProperty]
        private string dockingLocation;

        [ObservableProperty]
        private string vesselName;

        [ObservableProperty]
        private string flag;

        [ObservableProperty]
        private string imo;

        [ObservableProperty]
        private string portRegistry;

        [ObservableProperty]
        private DateTime? dateOfBuilding;

        [ObservableProperty]
        private string owner;

        [ObservableProperty]
        private string vesselOperator;

        [ObservableProperty]
        private string shippingAgent;

        [ObservableProperty]
        private string lastCargo;

        [ObservableProperty]
        private string secondLastCargo;

        [ObservableProperty]
        private string thirdLastCargo;

        [ObservableProperty]
        private string fourthLastCargo;
        public List<HoldModel> Holds { get; set; } = new();
        public string FlagIcon { get; set; }

        public byte[] FirstImage { get; set; }
    }
}