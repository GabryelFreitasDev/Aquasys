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

        [StringLength(200)]
        [Column("os")]
        public string OS { get; set; }

        [StringLength(200)]
        [Column("dockinglocation")]
        public string DockingLocation { get; set; }

        [Required]
        [StringLength(200)]
        [Column("vesselname")]
        public string VesselName { get; set; }

        [Required]
        [StringLength(200)]
        [Column("imo")]
        public string IMO { get; set; }

        [Required]
        [StringLength(200)]
        [Column("portregistry")]
        public string PortRegistry { get; set; }

        [Column("dateofbuilding", TypeName = "date")]
        public DateTime? DateOfBuilding { get; set; }

        [StringLength(200)]
        [Column("shippingagent")]
        public string? ShippingAgent { get; set; }

        [StringLength(100)]
        [Column("flag")]
        public string? Flag { get; set; }

        [Required]
        [StringLength(200)]
        [Column("owner")]
        public string Owner { get; set; }

        [Required]
        [StringLength(200)]
        [Column("vesseloperator")]
        public string VesselOperator { get; set; }

        [StringLength(200)]
        [Column("lastcargo")]
        public string? LastCargo { get; set; }

        [StringLength(200)]
        [Column("secondlastcargo")]
        public string? SecondLastCargo { get; set; }

        [StringLength(200)]
        [Column("thirdlastcargo")]
        public string? ThirdLastCargo { get; set; }

        [StringLength(200)]
        [Column("fourthlastcargo")]
        public string? FourthLastCargo { get; set; }

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