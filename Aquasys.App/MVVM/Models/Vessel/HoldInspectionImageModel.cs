using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.App.MVVM.Models.Vessel
{
    public class HoldInspectionImageModel : BaseModels
    {
        public HoldInspectionImageModel() {}

        public long IDHoldInspectionImage { get; set; }
        public byte[] Image { get; set; }
        public string? Description { get; set; }
        public string? Observation { get; set; }
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
        public long IDHoldInspection { get; set; }
    }
}
