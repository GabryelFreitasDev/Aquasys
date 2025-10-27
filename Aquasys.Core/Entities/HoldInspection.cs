
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    [Table("holdinspection")]
    public class HoldInspection : SyncableEntity
    {
        [Key]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        [Column("idholdinspection", TypeName = "bigint")]
        public long IDHoldInspection { get; set; }

        [Required]
        [Column("inspectiondatetime", TypeName = "date")]
        public DateTime InspectionDateTime { get; set; }

        [StringLength(60)]
        [Column("leadinspector")]
        public string? LeadInspector { get; set; }

        [Column("empty", TypeName = "integer")]
        public int Empty { get; set; }

        [Column("clean", TypeName = "integer")]
        public int Clean { get; set; }

        [Column("dry", TypeName = "integer")]
        public int Dry { get; set; }

        [Column("odorfree", TypeName = "integer")]
        public int OdorFree { get; set; }

        [Column("cargoresidue", TypeName = "integer")]
        public int CargoResidue { get; set; }

        [Column("insects", TypeName = "integer")]
        public int Insects { get; set; }

        [StringLength(60)]
        [Column("cleaningmethod")]
        public string? CleaningMethod { get; set; }

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
