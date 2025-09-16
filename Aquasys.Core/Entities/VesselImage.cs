
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    public class VesselImage : SyncableEntity
    {
        [Key]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        [Column("idvesselimage", TypeName = "bigint")]
        public long IDVesselImage { get; set; }

        [Required]
        [Column("image", TypeName = "bytea")] // TypeName para PostgreSQL
        public byte[] Image { get; set; }

        [StringLength(200)]
        [Column("name")]
        public string? Name { get; set; }

        [StringLength(200)]
        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("datacadastro", TypeName = "date")]
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        // Chave Estrangeira
        [Column("idvessel", TypeName = "bigint")]
        public long IDVessel { get; set; }

        [SQLite.Ignore]
        [ForeignKey(nameof(IDVessel))]
        public virtual Vessel? VesselEntity { get; set; }
    }
}