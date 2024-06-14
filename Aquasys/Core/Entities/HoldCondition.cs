using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.Core.Entities
{
    internal class HoldCondition
    {
        [PrimaryKey, AutoIncrement] public long IDHoldCondition { get; set; }
        public int Empty { get; set; }
        public int Clean { get; set; }
        public int Dry { get; set; }
        public int OdorFree { get; set; }
        public int CargoResidue { get; set; }
        public int Insects { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        [ForeignKey("IDHoldInspection")] public HoldInspection HoldInspection = new();
    }
}
