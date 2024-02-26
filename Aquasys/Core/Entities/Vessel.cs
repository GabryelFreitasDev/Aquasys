using Aquasys.Core.Enums;
using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    public class Vessel
    {
        [PrimaryKey, AutoIncrement] public long IDVessel { get; set; }
        [MaxLength(200), NotNull] public string OS { get; set; } = string.Empty;
        [MaxLength(200), NotNull] public string VesselName { get; set; } = string.Empty;
        [MaxLength(200), NotNull] public string Place { get; set; } = string.Empty;
        [MaxLength(200), NotNull] public string IMO { get; set; } = string.Empty;
        [MaxLength(200), NotNull] public string PortRegistry { get; set; } = string.Empty;
        public DateTime? ManufacturingDate { get; set; }
        [NotNull] public VesselType VesselType { get; set; } = VesselType.BARQUINHO;
        [NotNull] public string Owner { get; set; } = string.Empty;
        [NotNull] public string Operator { get; set; } = string.Empty;
        [ForeignKey("UserRegistration")] public User UserRegistration = new();
        [NotNull] public DateTime DateTimeOfRegister { get; set; } = DateTime.Now;
    }
}
