
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    public class User : SyncableEntity
    {
        [Key]
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        [Column("iduser", TypeName = "bigint")]
        public long IDUser { get; set; }

        [Required]
        [StringLength(100)]
        [Column("username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20)]
        [Column("password")]
        public string Password { get; set; }

        [StringLength(200)]
        [Column("email")]
        public string? Email { get; set; }

        [Column("rememberme", TypeName = "boolean")]
        public bool RememberMe { get; set; } = false;
    }
}