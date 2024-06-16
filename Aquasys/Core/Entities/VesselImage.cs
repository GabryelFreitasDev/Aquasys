using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.Core.Entities
{
    public class VesselImage
    {
        [PrimaryKey, AutoIncrement] public long IDVesselImage { get; set; }
        [NotNull] public byte[] Image { get; set; }
        [MaxLength(200)] public string? Name { get; set; }
        [MaxLength(200)] public string? Description { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; } = DateTime.Now;
        [ForeignKey("IDVessel")] public long IDVessel { get; set; }
    }
}
