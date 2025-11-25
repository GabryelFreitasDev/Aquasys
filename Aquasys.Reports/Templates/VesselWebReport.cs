using Aquasys.Reports.Enums;
using Aquasys.Reports.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aquasys.Reports.Templates
{
    public class VesselReportDto
    {
        public string VesselName { get; set; }
        public string OS { get; set; }
        public string DockingLocation { get; set; }
        public string IMO { get; set; }
        public string Flag { get; set; }
        public string PortRegistry { get; set; }
        public string Owner { get; set; }
        public string VesselOperator { get; set; }
        public string ShippingAgent { get; set; }
        public DateTime? DateOfBuilding { get; set; }
        public DateTime? RegistrationDateTime { get; set; }
        public string LastCargo { get; set; }
        public string SecondLastCargo { get; set; }
        public string ThirdLastCargo { get; set; }
        public string FourthLastCargo { get; set; }
        public List<HoldDto> Holds { get; set; } = new();
    }

    public class HoldDto
    {
        public string BasementNumber { get; set; }
        public string Agent { get; set; }
        public string Cargo { get; set; }
        public DateTime? RegistrationDateTime { get; set; }
        public List<HoldInspectionDto> Inspections { get; set; } = new();
    }

    public class HoldInspectionDto
    {
        public DateTime? InspectionDateTime { get; set; }
        public string LeadInspector { get; set; }
        public bool? Empty { get; set; }
        public bool? Clean { get; set; }
        public bool? Dry { get; set; }
        public bool? OdorFree { get; set; }
        public bool? CargoResidue { get; set; }
        public bool? Insects { get; set; }
        public string CleaningMethod { get; set; }
        public DateTime? RegistrationDateTime { get; set; }
    }

    public class VesselWebReport : IReportTemplate
    {
        public ReportType TemplateType => ReportType.Vessel;

        public byte[] Generate(object model)
        {
            var vessel = model as VesselReportDto
                ?? throw new ArgumentException("Invalid model for VesselWebReport");

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .Text($"Vessel Report: {vessel.VesselName}")
                        .SemiBold().FontSize(18).FontColor(Colors.Blue.Darken2);

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().Text("Vessel Information").Bold().FontSize(14);
                        column.Item().Grid(grid =>
                        {
                            grid.Columns(2);
                            grid.Item().Text($"Work Order (OS): {vessel.OS}");
                            grid.Item().Text($"Docking Location: {vessel.DockingLocation}");
                            grid.Item().Text($"IMO: {vessel.IMO}");
                            grid.Item().Text($"Port of Registry: {vessel.PortRegistry}");
                            grid.Item().Text($"Flag: {vessel.Flag}");
                            grid.Item().Text($"Owner: {vessel.Owner}");
                            grid.Item().Text($"Operator: {vessel.VesselOperator}");
                            grid.Item().Text($"Shipping Agent: {vessel.ShippingAgent}");
                            if (vessel.DateOfBuilding.HasValue)
                                grid.Item().Text($"Build Date: {vessel.DateOfBuilding:dd/MM/yyyy}");
                            if (vessel.RegistrationDateTime.HasValue)
                                grid.Item().Text($"Registration Date: {vessel.RegistrationDateTime:dd/MM/yyyy}");
                        });

                        column.Item().PaddingTop(10)
                            .Text("Last Cargos").Bold().FontSize(14);
                        column.Item().Grid(grid =>
                        {
                            grid.Columns(2);
                            grid.Item().Text($"Last Cargo: {vessel.LastCargo}");
                            grid.Item().Text($"Second Last Cargo: {vessel.SecondLastCargo}");
                            grid.Item().Text($"Third Last Cargo: {vessel.ThirdLastCargo}");
                            grid.Item().Text($"Fourth Last Cargo: {vessel.FourthLastCargo}");
                        });

                        column.Item().PaddingTop(10)
                            .Text("Holds").Bold().FontSize(14);

                        if (vessel.Holds?.Any() == true)
                        {
                            foreach (var hold in vessel.Holds)
                            {
                                column.Item().BorderBottom(0.5f).PaddingBottom(5).Row(row =>
                                {
                                    row.AutoItem().Text($"Hold: {hold.BasementNumber}").Bold();
                                    row.RelativeItem().Text($"Agent: {hold.Agent}");
                                    row.RelativeItem().Text($"Cargo: {hold.Cargo}");
                                    if (hold.RegistrationDateTime.HasValue)
                                        row.RelativeItem().Text($"Hold Registration: {hold.RegistrationDateTime:dd/MM/yyyy}");
                                });

                                if (hold.Inspections?.Any() == true)
                                {
                                    foreach (var insp in hold.Inspections)
                                    {
                                        column.Item().PaddingLeft(15).Row(row =>
                                        {
                                            row.AutoItem().Text("Inspection:").Bold();
                                            row.RelativeItem().Text(
                                                $"Date: {insp.InspectionDateTime:dd/MM/yyyy} | " +
                                                $"Lead: {insp.LeadInspector} | Empty: {BoolStr(insp.Empty)} | Clean: {BoolStr(insp.Clean)} | Dry: {BoolStr(insp.Dry)}"
                                            );
                                            // adicione mais campos conforme quiser...
                                        });
                                    }
                                }
                                else
                                {
                                    column.Item().PaddingLeft(15).Text("Hold not inspected.").Italic();
                                }
                            }
                        }
                        else
                        {
                            column.Item().Text("No holds found.").Italic();
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Line("Generated by Aquasys");
                            x.CurrentPageNumber();
                        });
                });
            });

            return doc.GeneratePdf();
        }

        private string BoolStr(bool? b) => b == true ? "Yes" : "No";
    }
}