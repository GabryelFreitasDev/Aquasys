using Aquasys.Core.Enums;
using Aquasys.MVVM.ViewModels;
using CountryData.Standard;
using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.MVVM.Models.Vessel
{
    public class HoldConditionModel : BaseModels
    {
        public HoldConditionModel() {}

        public long IDHoldCondition { get; set; }
        public int Empty { get; set; }
        public int Clean { get; set; }
        public int Dry { get; set; }
        public int OdorFree { get; set; }
        public int CargoResidue { get; set; }
        public int Insects { get; set; }
        public string? CleaningMethod { get; set; }
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

       public long IDHoldInspection { get; set; }
    }
}
