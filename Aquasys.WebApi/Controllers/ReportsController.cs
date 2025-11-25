using Aquasys.Reports.Enums;
using Aquasys.Reports.Templates; // VesselWebReport, DTOs
using Aquasys.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using System.Text.RegularExpressions;
using QuestPDF.Infrastructure;

namespace Aquasys.WebApi.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        // Teste QuestPDF simples
        [HttpGet("testpdf")]
        public IActionResult TestPdf()
        {
            try
            {
                QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
                var doc = QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(QuestPDF.Helpers.PageSizes.A4);
                        page.Margin(40);
                        page.DefaultTextStyle(x => x.FontSize(20));
                        page.Content()
                            .Text("Hello QuestPDF!")
                            .Bold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                    });
                });
                var pdfBytes = doc.GeneratePdf();
                return File(pdfBytes, "application/pdf", "hello-quest.pdf");
            }
            catch (Exception ex)
            {
                // Apenas retorna o erro pro client/dev:
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("vessel/{vesselId:guid}")]
        public async Task<IActionResult> GenerateVesselReport(Guid vesselId)
        {
            try
            {
                // Projeciona direto para DTO QuestPDF
                var vessel = await _context.Vessels
                    .AsNoTracking()
                    .Where(v => v.GlobalId == vesselId)
                    .Select(v => new VesselReportDto
                    {
                        VesselName = v.VesselName,
                        OS = v.OS,
                        DockingLocation = v.DockingLocation,
                        IMO = v.IMO,
                        Flag = v.Flag,
                        PortRegistry = v.PortRegistry,
                        Owner = v.Owner,
                        VesselOperator = v.VesselOperator,
                        ShippingAgent = v.ShippingAgent,
                        DateOfBuilding = v.DateOfBuilding,
                        RegistrationDateTime = v.RegistrationDateTime,
                        LastCargo = v.LastCargo,
                        SecondLastCargo = v.SecondLastCargo,
                        ThirdLastCargo = v.ThirdLastCargo,
                        FourthLastCargo = v.FourthLastCargo,
                        Holds = v.Holds.Select(h => new HoldDto
                        {
                            BasementNumber = h.BasementNumber,
                            Agent = h.Agent,
                            Cargo = h.Cargo,
                            RegistrationDateTime = h.RegistrationDateTime,
                            Inspections = h.Inspections.Select(i => new HoldInspectionDto
                            {
                                InspectionDateTime = i.InspectionDateTime,
                                LeadInspector = i.LeadInspector,
                                Empty = i.Empty == 1,
                                Clean = i.Clean == 1,
                                Dry = i.Dry == 1,
                                OdorFree = i.OdorFree == 1,
                                CargoResidue = i.CargoResidue == 1,
                                Insects = i.Insects == 1,
                                CleaningMethod = i.CleaningMethod,
                                RegistrationDateTime = i.RegistrationDateTime
                            }).ToList()
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (vessel == null)
                    return NotFound("Embarcação não encontrada.");

                var report = new VesselWebReport();
                var pdfBytes = report.Generate(vessel);

                var vesselNameSafe = Regex.Replace(vessel.VesselName ?? "Vessel", @"[^a-zA-Z0-9\-_]", "_");
                var fileName = $"Report_{vesselNameSafe}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                try
                {
                    var logDir = "C:\\temp";
                    var logPath = Path.Combine(logDir, "aquasys-report-error.txt");
                    if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
                    System.IO.File.WriteAllText(logPath,
                        $"[{DateTime.Now}]\n" +
                        $"Erro ao gerar relatório para vesselId {vesselId}\n" +
                        $"{ex}\n"
                    );
                }
                catch { /* Ignore logging errors */ }
                Console.WriteLine(ex);
                return StatusCode(500, $"Erro interno ao gerar relatório: {ex.Message}");
            }
        }
    }
}