using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    public class Hold
    {
        [PrimaryKey, AutoIncrement] public long IDHold { get; set; }
        [NotNull] public decimal Capacity { get; set; }
        public decimal LoadPlan { get; set; }
        public decimal ProductWeight { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        [ForeignKey("IDVessel")] public long IDVessel { get; set; }
    }
}
