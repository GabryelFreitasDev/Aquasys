using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.Core.Entities
{
    internal class Hold
    {
        [PrimaryKey, AutoIncrement] public long IDHold { get; set; }
        [NotNull] public decimal Capacity { get; set; }
        public decimal LoadPlan { get; set; }
        public decimal ProductWeight { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        [ForeignKey("IDVessel")] public Vessel Vessel = new();
    }
}
