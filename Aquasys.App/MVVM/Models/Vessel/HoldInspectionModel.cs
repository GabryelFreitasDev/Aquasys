using Aquasys.App.Core.Enums;
using Aquasys.App.MVVM.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CountryData.Standard;
using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aquasys.App.MVVM.Models.Vessel
{
    public partial class HoldInspectionModel : BaseModels
    {
        public HoldInspectionModel() {}

        public long IDHoldInspection { get; set; }
        [ObservableProperty]
        public DateTime inspectionDate = DateTime.Now;
        [ObservableProperty]
        public TimeSpan inspectionTime = DateTime.Now.TimeOfDay;
        [ObservableProperty]
        public string? leadInspector;
        [ObservableProperty]
        public int empty;
        [ObservableProperty]
        public int clean;
        [ObservableProperty]
        public int dry;
        [ObservableProperty]
        public int odorFree;
        [ObservableProperty]
        public int cargoResidue;
        [ObservableProperty]
        public int insects;
        [ObservableProperty]
        public string? cleaningMethod;
        public DateTime RegistrationDateTime { get; set; } = DateTime.Now;

        public long IDHold { get; set; }
    }
}
