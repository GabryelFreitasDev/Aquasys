using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.Core.Entities
{
    public class Inspection
    {
        [PrimaryKey, AutoIncrement] public long IDInspection { get; set; }
        [MaxLength(200), NotNull] public string? OS { get; set; }
        [NotNull] public DateTime StartDateTime { get; set; } = DateTime.Now;
        public DateTime EndDateTime { get; set; } = DateTime.Now;
        [MaxLength(60)] public string? ShippingAgent { get; set; }
        [MaxLength(60)] public string? LeadInspector { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; }

        [ForeignKey("IDVessel")] public long IDVessel { get; set; }
}
}
