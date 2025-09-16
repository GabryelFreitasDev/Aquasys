
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
        [Column("datacadastro", TypeName = "date")]
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        [Column("idinspection", TypeName = "bigint")]
        public long IDInspection { get; set; }

        [SQLite.Ignore]
        [ForeignKey(nameof(IDInspection))]
        public virtual Inspection? InspectionEntity { get; set; }

        [Column("idhold", TypeName = "bigint")]
        public long IDHold { get; set; }

        [SQLite.Ignore]
        [ForeignKey(nameof(IDHold))]
        public virtual Hold? HoldEntity { get; set; }
    }
}
