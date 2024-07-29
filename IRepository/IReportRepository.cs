using DoctorAppointment.Dto;

namespace DoctorAppointment.IRepository
{
    public interface IReportRepository
    {
        Task<string> CreateReport(AppointmentUpdateDto dataDto);
        Task<Dictionary<string,string>> GetReportData(string fileName);
        Task<Stream> GetReportFile(string fileName);
        Task<Object> GetReportAnalysis(int userId, int DocId, int pageIndex, int pageSize);

    }
}
