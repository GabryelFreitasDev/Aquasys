using Aquasys.Reports.Enums;

namespace Aquasys.Reports.Interfaces
{
    public interface IReportTemplate
    {
        ReportType TemplateType { get; }

        /// <summary>
        /// Gera o relatório em PDF com base no modelo fornecido.
        /// </summary>
        /// <param name="model">Objeto com os dados necessários para o relatório</param>
        /// <returns>PDF em formato byte[]</returns>
        byte[] Generate(object model);
    }
}
