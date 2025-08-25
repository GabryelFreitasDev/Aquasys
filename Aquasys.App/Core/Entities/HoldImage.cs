using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.App.Core.Entities
{
    public class HoldImage
    {
        [PrimaryKey, AutoIncrement] public long IDHoldImage { get; set; }
        [NotNull] public byte[] Image { get; set; }
        [MaxLength(60)] public string? Description { get; set; }
        [MaxLength(300)] public string? Observation { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
        [ForeignKey("IDHold")] public long IDHold { get; set; }
    }
}
