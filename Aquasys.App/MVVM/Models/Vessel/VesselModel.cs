using Aquasys.App.Core.Enums;
using Aquasys.App.MVVM.ViewModels;
using CountryData.Standard;

namespace Aquasys.App.MVVM.Models.Vessel
{
    public class VesselModel : BaseModels
    {
        public VesselModel()
        {
        }
        public long IDVessel { get; set; }
        public string VesselName { get; set; }
        public string Place { get; set; }
        public string Flag { get; set; }
        public string IMO { get; set; }
        public string PortRegistry { get; set; }
        public DateTime ManufacturingDate { get; set; } = DateTime.Now;
        public VesselType VesselType { get; set; } = VesselType.NAVIO;
        public string Owner { get; set; }
        public string Operator { get; set; }

        public byte[] FirstImage { get; set; }
    }
}
