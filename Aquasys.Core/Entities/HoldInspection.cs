
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    public class HoldInspection : SyncableEntity
    {
        [Key]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        [Column("idholdinspection", TypeName = "bigint")]
        public long IDHoldInspection { get; set; }

        [Required]
        [Column("inspectiondatetime", TypeName = "date")]
        public DateTime InspectionDateTime { get; set; } = DateTime.Now;

        [StringLength(60)]
        [Column("leadinspector")]
        public string? LeadInspector { get; set; }

        [Required]
        [Column("registrationdatetime", TypeName = "date")]
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        [Column("idhold", TypeName = "bigint")]
        public long IDHold { get; set; }

        [SQLite.Ignore]
        [ForeignKey(nameof(IDHold))]
        public virtual Hold? HoldEntity { get; set; }
    }
}
