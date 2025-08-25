using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.App.Core.Entities
{
    public class HoldInspection
    {
        [PrimaryKey, AutoIncrement] public long IDHoldInspection { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        [ForeignKey("IDInspection")] public long IDInspection { get; set; }
        [ForeignKey("IDHold")] public long IDHold { get; set; }

    }
}
