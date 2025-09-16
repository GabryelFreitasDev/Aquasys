
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    public class HoldCargo : SyncableEntity
    {
        [Key]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        [Column("idholdcargo", TypeName = "bigint")]
        public long IDHoldCargo { get; set; }

        [StringLength(60)]
        [Column("cargo")]
        public string? Cargo { get; set; }

        [Column("order", TypeName = "integer")]
        public int Order { get; set; }

        [Required]
        [Column("datacadastro", TypeName = "date")]
        public DateTime RegistrationDateTime { get; set; }

        [Column("idhold", TypeName = "bigint")]
        public long IDHold { get; set; }

        [SQLite.Ignore]
        [ForeignKey(nameof(IDHold))]
        public virtual Hold? HoldEntity { get; set; }
    }
}
