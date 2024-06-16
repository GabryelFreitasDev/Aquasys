using Aquasys.MVVM.ViewModels;

namespace Aquasys.MVVM.Models.Vessel
{
    public class VesselImageModel : BaseModels
    {
        public VesselImageModel()
        {
        }
        public long IDVesselImage { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long IDVessel { get; set; }
    }
}
