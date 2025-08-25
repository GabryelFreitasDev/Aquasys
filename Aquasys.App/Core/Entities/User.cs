using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.App.Core.Entities
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public long IDUser { get; set; }
        [MaxLength(100), NotNull]
        public string UserName { get; set; }
        [MaxLength(20), NotNull]
        public string Password { get; set; }
        [MaxLength(200)]
        public string? Email { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
