using DoctorAppointment.Dto;
using DoctorAppointment.Migrations;
using DoctorAppointment.Models;

namespace DoctorAppointment.IRepository
{
    public interface IAdminRepository
    {
        Task<(List<DoctorAccessReqDto>,int)> GetDoctorWithRequest(int pageIndex, int pageSize);
        Task<List<AssistantDetailDto>> GetAssistantWithRequest();
        Task<bool> DoctorApprove(int doctorMetaId);
        Task<bool> AssistantApprove(int assistantId);
        Task<(List<DoctorAccessReqDto>,int)> GetApprovedDoctorDetails(int pageIndex, int pageSize, string? filter);
        Task<(List<AssistantDoctorDetailsMapDto>,int)> GetApprovedAssistantDetails(int pageIndex, int pageSize);
        Task<List<DoctorAccessReqDto>> GetApprovedDoctorDetails();
        Task<DoctorAccessReqDto> GetDoctorById(int doctorMetaId);
        Task<(List<ClientDetailDto>, int)> GetClientDetials(int pageIndex, int pageSize);
        Task<ClientDetailDto> GetUserDetailsById(int id);
        Task<User_details> UpdateDetailsClient(int id, ClientDetailDto user);
        Task DeleteUserById(int id);
    }
}