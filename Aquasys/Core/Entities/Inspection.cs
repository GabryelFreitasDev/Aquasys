using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aquasys.Core.Entities
{
    internal class Inspection
    {
        [PrimaryKey, AutoIncrement] public long IDInspection { get; set; }
        public int WorkOrder { get; set; }
        [NotNull] public DateTime StartDateTime { get; set; } = DateTime.Now;
        public DateTime EndDateTime { get; set; } = DateTime.Now;
        [MaxLength(60)] public string? ShippingAgent { get; set; }
        [MaxLength(60)] public string? LeadInspector { get; set; }
        [NotNull] public DateTime RegistrationDateTime { get; set; }

        [ForeignKey("IDVessel")] public Vessel Vessel = new();
    }
}
