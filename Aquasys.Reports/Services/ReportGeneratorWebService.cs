using Aquasys.Reports.Enums;
using Aquasys.Reports.Interfaces;

namespace Aquasys.Reports.Services
{
    public class ReportGeneratorWebService
    {
        private readonly IEnumerable<IReportTemplate> _templates;

        public ReportGeneratorWebService(IEnumerable<IReportTemplate> templates)
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
            return Task.FromResult(Generate(type, model));
        }
    }
}