using SQLite;

namespace Aquasys.Core.Entities
{
    public class TypeVessel
    {
        [PrimaryKey, AutoIncrement] public long IDTypeVessel { get; set; }
        [NotNull] public byte[] Name { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
    }
}
