using DoctorAppointment.Dto;
using DoctorAppointment.Models;

namespace DoctorAppointment.IRepository
{
    public interface IDoctorRepository
    {
        Task<DoctorDetailDto> DoctorfCreateAsync(DoctorDetailDto dto);
        Task<DoctorDetailDto> DoctorfUpdateAsync(int doctorId, DoctorDetailDto dto);
        Task<DoctorMetadata> DoctorGetMetaDataAsync(int doctorId);        
        Task<string> CheckDoctorFilled(int User_id);         
    }
}
