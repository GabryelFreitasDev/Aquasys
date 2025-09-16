using Aquasys.App.Core.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    public class Vessel : SyncableEntity
    {
        [Key]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        [Column("idvessel", TypeName = "bigint")]
        public long IDVessel { get; set; }

        [Required]
        [StringLength(200)]
        [Column("vesselname")]
        public string VesselName { get; set; }

        [Required]
        [StringLength(200)]
        [Column("place")]
        public string Place { get; set; }

        [Required]
        [StringLength(200)]
        [Column("imo")]
        public string IMO { get; set; }

        [Required]
        [StringLength(200)]
        [Column("portregistry")]
        public string PortRegistry { get; set; }

        [Column("manufacturingdate", TypeName = "date")]
        public DateTime? ManufacturingDate { get; set; }

        [StringLength(100)]
        [Column("flag")]
        public string? Flag { get; set; }

        [Required]
        [Column("vesseltype", TypeName = "integer")]
        public VesselType VesselType { get; set; }

        [Required]
        [StringLength(200)]
        [Column("owner")]
        public string Owner { get; set; }

        [Required]
        [StringLength(200)]
        [Column("operator")]
        public string Operator { get; set; }

        [Required]
        [Column("datacadastro", TypeName = "date")]
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        [Column("iduserregistration", TypeName = "bigint")]
        public long IDUserRegistration { get; set; }

        [SQLite.Ignore]
        [ForeignKey(nameof(IDUserRegistration))]
        public virtual User? UserRegistrationEntity { get; set; }
    }
}