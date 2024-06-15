using Aquasys.Core.Enums;
using Aquasys.MVVM.ViewModels;
using CountryData.Standard;

namespace Aquasys.MVVM.Models.Vessel
{
    public class VesselModel : BaseModels
    {
        public VesselModel()
        {
        }
        public long IDVessel { get; set; }
        public string OS { get; set; }
        public string VesselName { get; set; }
        public string Place { get; set; }
        public string Flag { get; set; }
        public string IMO { get; set; }
        public string PortRegistry { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public VesselType VesselType { get; set; }
        public string Owner { get; set; }
        public string Operator { get; set; }
    }
}
