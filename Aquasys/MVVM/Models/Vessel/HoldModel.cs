using Aquasys.Core.Enums;
using Aquasys.MVVM.ViewModels;
using CountryData.Standard;

namespace Aquasys.MVVM.Models.Vessel
{
    public class HoldModel : BaseModels
    {
        public HoldModel() {}

        public long IDHold { get; set; }
        public string? Name { get; set; }
        public string? Cargo { get; set; }
        public decimal Capacity { get; set; }
        public decimal LoadPlan { get; set; }
        public decimal ProductWeight { get; set; }
        public string? LastCargo { get; set; }
        public string? SecondLastCargo { get; set; }
        public string? ThirdLastCargo { get; set; }
        public string? FourthLastCargo { get; set; }

        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
        public long IDVessel { get; set; }
    }
}
