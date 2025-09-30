namespace Aquasys.Reports.Interfaces
{
    public interface IReportGenerator
    {
        byte[] GenerateReport<T>(T data, string templateName);
    }
}
