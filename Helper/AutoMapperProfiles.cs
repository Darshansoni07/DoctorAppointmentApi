using AutoMapper;
using DoctorAppointment.Dto;
using DoctorAppointment.Models;

namespace DoctorAppointment.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<User_details, RegisterDto>().ReverseMap();
            CreateMap<DoctorMetadata, DoctorDetailDto> ().ReverseMap();
            CreateMap<Slot,SlotGetDetailOnDate> ().ReverseMap();
        }
    }
}
