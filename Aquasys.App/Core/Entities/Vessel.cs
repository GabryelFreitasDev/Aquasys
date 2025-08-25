using Aquasys.App.Core.Enums;
using SQLite;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.App.Core.Entities
{
    public class Vessel
    {
        [PrimaryKey, AutoIncrement] public long IDVessel { get; set; }
        [MaxLength(200), NotNull] public string? VesselName { get; set; }
        [MaxLength(200), NotNull] public string? Place { get; set; }
        [MaxLength(200), NotNull] public string? IMO { get; set; } 
        [MaxLength(200), NotNull] public string? PortRegistry { get; set; } 
        public DateTime? ManufacturingDate { get; set; }
        public string? Flag { get; set; }
        [NotNull] public VesselType? VesselType { get; set; }
        [NotNull] public string? Owner { get; set; }
        [NotNull] public string? Operator { get; set; }
        [ForeignKey("IDUserRegistration")] public long IDUserRegistration { get; set; }
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
    }
}
