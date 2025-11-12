using CommunityToolkit.Mvvm.ComponentModel;

namespace Aquasys.App.MVVM.Models.Vessel
{
    public partial class HoldModel : BaseModels
    {
        public HoldModel() {}

        public long IDHold { get; set; }

        [ObservableProperty]
        public string basementNumber;
        [ObservableProperty]
        public string agent;
        [ObservableProperty]
        public string cargo;
        [ObservableProperty]
        public decimal capacity;
        [ObservableProperty]
        public decimal productWeight;
        [ObservableProperty]
        public string loadPlan;

        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
        public long IDVessel { get; set; }

        [ObservableProperty]
        public bool inspectioned = false;
        public HoldInspectionModel? HoldInspection { get; set; }
    }
}
