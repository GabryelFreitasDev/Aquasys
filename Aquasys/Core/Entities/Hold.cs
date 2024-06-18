using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.Core.Entities
{
    public class Hold
    {
        [PrimaryKey, AutoIncrement] public long IDHold { get; set; }
        [NotNull, MaxLength(200)] public string? Name { get; set; }
        [MaxLength(100)] public string? Cargo { get; set; }
        [NotNull] public decimal Capacity { get; set; }
        public decimal LoadPlan { get; set; }
        public decimal ProductWeight { get; set; }
        [MaxLength(100)] public string? LastCargo { get; set; }
        [MaxLength(100)] public string? SecondLastCargo { get; set; }
        [MaxLength(100)] public string? ThirdLastCargo { get; set; }
        [MaxLength(100)] public string? FourthLastCargo { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
        [ForeignKey("IDVessel")] public long IDVessel { get; set; }
    }
}
