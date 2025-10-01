using Aquasys.Reports.Enums;
using Aquasys.Reports.Interfaces;

namespace Aquasys.Reports.Services
{
    public class ReportGeneratorService
    {
        private readonly IEnumerable<IReportTemplate> _templates;

        public ReportGeneratorService(IEnumerable<IReportTemplate> templates)
        {
            _templates = templates;
        }

        public byte[] Generate(ReportType type, object model)
        {
            var template = _templates.FirstOrDefault(t => t.TemplateType == type);

            if (template == null)
                throw new InvalidOperationException($"Nenhum template registrado para {type}");

            return template.Generate(model);
        }

        public Task<byte[]> GenerateAsync(ReportType type, object model)
        {
            return Task.Run(() => Generate(type, model));
        }
    }
}