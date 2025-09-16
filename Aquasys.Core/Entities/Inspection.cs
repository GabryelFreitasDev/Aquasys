
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    public class Inspection : SyncableEntity
    {
        [Key]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        [Column("idinspection", TypeName = "bigint")]
        public long IDInspection { get; set; }

        [Required]
        [StringLength(200)]
        [Column("os")]
        public string OS { get; set; }

        [Required]
        [Column("startdatetime", TypeName = "timestamp")]
        public DateTime StartDateTime { get; set; } = DateTime.Now;

        [Column("enddatetime", TypeName = "timestamp")]
        public DateTime EndDateTime { get; set; }

        [StringLength(60)]
        [Column("shippingagent")]
        public string? ShippingAgent { get; set; }

        [StringLength(60)]
        [Column("leadinspector")]
        public string? LeadInspector { get; set; }

        [Required]
        [Column("datacadastro", TypeName = "date")]
        public DateTime RegistrationDateTime { get; set; }

        [Column("idvessel", TypeName = "bigint")]
        public long IDVessel { get; set; }

        [SQLite.Ignore]
        [ForeignKey(nameof(IDVessel))]
        public virtual Vessel? VesselEntity { get; set; }
    }
}