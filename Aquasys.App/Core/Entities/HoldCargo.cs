using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.App.Core.Entities
{
    public class HoldCargo
    {
        [PrimaryKey, AutoIncrement] public long IDHoldCargo { get; set; }
        [MaxLength(60)] public string? Cargo { get; set; }
        public int Order { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; }

        [ForeignKey("IDHold")] public Hold Hold = new();
    }
}
