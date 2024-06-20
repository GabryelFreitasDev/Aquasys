using Aquasys.Core.Enums;
using Aquasys.MVVM.ViewModels;
using CountryData.Standard;
using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.MVVM.Models.Vessel
{
    public class InspectionModel : BaseModels
    {
        public InspectionModel() {}

        public long IDInspection { get; set; }
        public string? OS { get; set; }
        public DateTime StartDateTime { get; set; } = DateTime.Now;
        public DateTime EndDateTime { get; set; } = DateTime.Now;
        public string? ShippingAgent { get; set; }
        public string? LeadInspector { get; set; }
        public DateTime RegistrationDateTime { get; set; }

        public long IDVessel { get; set; }
    }
}
