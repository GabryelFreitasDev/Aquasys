using Aquasys.Core.Enums;
using Aquasys.MVVM.ViewModels;
using CountryData.Standard;
using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.MVVM.Models.Vessel
{
    public class HoldInspectionModel : BaseModels
    {
        public HoldInspectionModel() {}

        public long IDHoldInspection { get; set; }
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        public long IDInspection { get; set; }
        public long IDHold { get; set; }
    }
}
