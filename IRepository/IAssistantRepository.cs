using DoctorAppointment.Dto;
using DoctorAppointment.Models;

namespace DoctorAppointment.IRepository
{
    public interface IAssistantRepository
    {
        Task<AssistantRegisterDto> User_Register(AssistantRegisterDto registerDto);
        Task<DoctorAssistantMapRegisterDto> User_Update(DoctorAssistantMapRegisterDto  mapRegisterDto);
        Task<DoctorAssistantMapRegisterDto> AssistantMapDetails(string Email);
        Task<AssistantDoctorDataDto> DoctorDetailsByAssistantId(int metaId);
        Task<(List<AssistantDetailDto>,int)> DoctorGetAllOwnAssistant(int metaId, int pageIndex, int pageSize);
        
                
    }
}
