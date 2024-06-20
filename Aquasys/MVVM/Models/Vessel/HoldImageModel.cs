using Aquasys.Core.Enums;
using Aquasys.MVVM.ViewModels;
using CountryData.Standard;
using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.MVVM.Models.Vessel
{
    public class HoldImageModel : BaseModels
    {
        public HoldImageModel() {}

        public long IDHoldImage { get; set; }
        public byte[] Image { get; set; }
        public string? Description { get; set; }
        public string? Observation { get; set; }
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
        [ForeignKey("IDHold")] public long IDHold { get; set; }
    }
}
