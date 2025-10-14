namespace Aquasys.App.MVVM.Models.Vessel
{
    public class HoldInspectionConditionModel : BaseModels
    {
        public HoldInspectionConditionModel() {}

        public long IDHoldInspectionCondition { get; set; }
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
