using Aquasys.Core.Entities;
using Aquasys.Reports.Enums;
using Aquasys.Reports.Interfaces;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;

namespace Aquasys.Reports.Templates
{
    public class VesselReport : IReportTemplate
    {
        public ReportType TemplateType => ReportType.Vessel;

        public byte[] Generate(object model)
        {
            var vessel = model as Vessel
                ?? throw new ArgumentException("Modelo inválido para VesselReport");

            using var document = new PdfDocument();
            var page = document.Pages.Add();

            var fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 20, PdfFontStyle.Bold);
            var font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

            page.Graphics.DrawString(
                $"Relatório do Navio: {vessel.VesselName}",
                fontTitle,
                PdfBrushes.Black,
                new PointF(0, 0)
            );

            float y = 40;
            float step = 24;

            page.Graphics.DrawString($"IMO: {vessel.IMO}", font, PdfBrushes.Black, new PointF(0, y += step));
            page.Graphics.DrawString($"Porto Registro: {vessel.PortRegistry}", font, PdfBrushes.Black, new PointF(0, y += step));
            page.Graphics.DrawString($"Tipo: {vessel.VesselType}", font, PdfBrushes.Black, new PointF(0, y += step));
            page.Graphics.DrawString($"Bandeira: {vessel.Flag}", font, PdfBrushes.Black, new PointF(0, y += step));
            page.Graphics.DrawString($"Proprietário: {vessel.Owner}", font, PdfBrushes.Black, new PointF(0, y += step));
            page.Graphics.DrawString($"Operador: {vessel.Operator}", font, PdfBrushes.Black, new PointF(0, y += step));
            page.Graphics.DrawString($"Data Fabricação: {vessel.ManufacturingDate:dd/MM/yyyy}", font, PdfBrushes.Black, new PointF(0, y += step));
            page.Graphics.DrawString($"Data Cadastro: {vessel.RegistrationDateTime:dd/MM/yyyy}", font, PdfBrushes.Black, new PointF(0, y += step));

            using var stream = new MemoryStream();
            document.Save(stream);
            return stream.ToArray();
        }
    }
}