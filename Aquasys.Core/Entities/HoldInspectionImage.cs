
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    public class HoldInspectionImage : SyncableEntity
    {
        [Key]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        [Column("idholdinspection", TypeName = "bigint")]
        public long IDHoldInspectionImage { get; set; }

        [Required]
        [Column("image", TypeName = "bytea")]
        public byte[] Image { get; set; }

        [StringLength(60)]
        [Column("description")]
        public string? Description { get; set; }

        [StringLength(300)]
        [Column("observation")]
        public string? Observation { get; set; }

        [Required]
        [Column("datacadastro", TypeName = "date")]
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        [Column("idhold", TypeName = "bigint")]
        public long IDHold { get; set; }

        [SQLite.Ignore]
        [ForeignKey(nameof(IDHold))]
        public virtual Hold? HoldEntity { get; set; }
    }
}