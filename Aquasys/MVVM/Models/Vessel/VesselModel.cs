using Aquasys.Core.Entities;
using Aquasys.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.MVVM.Models.Vessel
{
    public class VesselModel
    {
        public VesselModel()
        {
        }
        public long IDVessel { get; set; }
        public string OS { get; set; } = string.Empty;
        public string VesselName { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public string IMO { get; set; } = string.Empty;
        public string PortRegistry { get; set; } = string.Empty;
        public DateTime ManufacturingDate { get; set; }
        public VesselType VesselType { get; set; } = VesselType.BARQUINHO;
        public string Owner { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
    }
}
