using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    public class Hold : SyncableEntity 
    {
        [Key]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        [Column("idhold", TypeName = "bigint")]
        public long IDHold { get; set; }

        [Required]
        [StringLength(200)]
        [Column("basementnumber")]
        public string? BasementNumber { get; set; }

        [Required]
        [StringLength(200)]
        [Column("agent")]
        public string? Agent { get; set; }

        [StringLength(100)] 
        [Column("cargo")]
        public string? Cargo { get; set; }

        [Required]
        [Column("capacity", TypeName = "numeric(18,5)")]
        public decimal Capacity { get; set; }

        [Column("loadplan", TypeName = "numeric(18,5)")]
        public decimal LoadPlan { get; set; }

        [Column("productweight", TypeName = "numeric(18,5)")]
        public decimal ProductWeight { get; set; }

        [Required]
        [Column("datacadastro", TypeName = "date")]
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        [Column("idvessel", TypeName = "bigint")]
        public long IDVessel { get; set; }

        [SQLite.Ignore]
        [ForeignKey(nameof(IDVessel))]
        public virtual Vessel? VesselEntity { get; set; }
    }
}
