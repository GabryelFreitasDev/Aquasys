using Aquasys.Core.Enums;
using Aquasys.MVVM.ViewModels;
using CountryData.Standard;

namespace Aquasys.MVVM.Models.Vessel
{
    public class HoldModel : BaseModels
    {
        public HoldModel()
        {
        }

        public long IDHold { get; set; }
        public decimal Capacity { get; set; }
        public decimal LoadPlan { get; set; }
        public decimal ProductWeight { get; set; }
        public long IDVessel { get; set; }
    }
}
