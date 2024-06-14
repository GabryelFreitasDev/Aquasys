using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.Core.Entities
{
    internal class VesselImage
    {
        [PrimaryKey, AutoIncrement] public long IDVesselImage { get; set; }
        [NotNull] public byte[] Image { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; }

    }
}
