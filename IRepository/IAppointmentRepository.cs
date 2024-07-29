using DoctorAppointment.Dto;
using DoctorAppointment.Models;

namespace DoctorAppointment.IRepository
{
    public interface IAppointmentRepository
    {
        Task<string> CreateAsync(AppointmentCreateDto appointmentCreateDto);
        Task<(List<AppointmentUserDetailsDto>, int)> GetAllAppointmentWithRequest(int metId, int pageIndex, int pageSize);
        Task<string> AppointmentApprove(int slotId);
        Task<Object> GetAppointmentDetailsById(int id);
        Task<Dictionary<string,string>> GetAppointmentReportById(int id);
        Task<string> UpdateAppointmentDetail(int id, AppointmentUpdateDto user);
        Task<List<AppointmentUserDetailsDto>> GetAllAppointments(int metaId);
        Task<(List<AppointmentUserDetailsDto>, int)> GetAllApprovedAppointments(int metaId, int pageIndex, int pageSize);
        Task<List<AppointmentUserDetailsDto>> GetAllTodayAppointments(int metaId);
        Task<List<AppointmentUserDetailsDto>> GetAllAppointmentPervious(int metaId);
        Task<List<AppointmentUserDetailsDto>> GetAllSelectedDateAppointment(int metaId, DateTime date, int pageIndex, int pageSize);
        Task<(List<AppointmentUserDetailsDto>,int, int, int)> GetAllApproveAppointment(int metaId, int pageIndex, int pageSize);
        Task<(List<AppointmentBookingDetailUserDto>, int)> GetAllDetailOfBookingUser(int userId, int pageIndex, int pageSize);
        Task<(List<AppointmentUserDetailsDto>,int,int)> AdminGetAllBookedAppoinments(int pageIndex, int pageSize);
        Task<(List<AppointmentBookingDetailUserDto>,int, int, int, int, int, int)> PaitentGetAllAppointmentDetails(int userId, int pageIndex, int pageSize);
    }
}
