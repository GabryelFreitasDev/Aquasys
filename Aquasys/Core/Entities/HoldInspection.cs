using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.Core.Entities
{
    public class HoldInspection
    {
        [PrimaryKey, AutoIncrement] public long IDHoldInspection { get; set; }
        [MaxLength(60)] public string? CleaningMethod { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        [ForeignKey("IDInspection")] public Inspection IDInspection = new();
        [ForeignKey("IDHold")] public Hold Hold = new();

    }
}
