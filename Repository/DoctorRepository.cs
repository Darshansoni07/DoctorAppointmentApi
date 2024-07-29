using DoctorAppointment.Dbcontext;
using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using DoctorAppointment.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointment.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext dbContext;
        public DoctorRepository(AppDbContext dbContext) 
        {
            this.dbContext = dbContext;            
        }

        public async Task<string> CheckDoctorFilled(int User_id)
        {
            bool doesNotContainUser = !dbContext.DS_Doctor_Metadata
                .Any(doc => doc.UserDetailsUser_Id == User_id);
            if(doesNotContainUser)
            {
                return "doctor Not Found";                
            }
            else
            {
                bool containsUser = dbContext.DS_Doctor_Metadata
                    .Any(doc => doc.UserDetailsUser_Id == User_id && doc.RequestDoctorApprove);
                if(containsUser)
                {
                    return "doctor found and approved";
                }
                else
                {
                    return "doctor found but not approved";
                }
            }

            //return true;
        }

        public async Task<DoctorDetailDto> DoctorfCreateAsync(DoctorDetailDto doctorDetailDto)
        {
            bool doesNotContainUser = !dbContext.DS_Doctor_Metadata
                  .Any(doc => doc.UserDetailsUser_Id == doctorDetailDto.UserDetailsUser_Id);
            if (doctorDetailDto != null) 
            {
                if (doesNotContainUser)
                {
                    DoctorMetadata doctorMetadata = new DoctorMetadata();
                    doctorMetadata.Specialist = doctorDetailDto.Specialist;
                    doctorMetadata.License = doctorDetailDto.License;
                    doctorMetadata.FeesAmount = doctorDetailDto.FeesAmount;
                    doctorMetadata.UserDetailsUser_Id = doctorDetailDto.UserDetailsUser_Id;
                    doctorMetadata.CreatedOn = DateTime.Now;
                    doctorMetadata.UpdatedOn = null;
                    doctorMetadata.RequestDoctorApprove = false;
                    dbContext.DS_Doctor_Metadata.Add(doctorMetadata);
                    dbContext.SaveChanges();
                    return doctorDetailDto;
                }
                return null;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public Task<DoctorDetailDto> DoctorfUpdateAsync(int doctorId, DoctorDetailDto dto)
        {
            throw new NotImplementedException();
        }



        public async Task<DoctorMetadata> DoctorGetMetaDataAsync(int doctorId)
        {
            if (doctorId == 0)
            {
                return null;
            }
            else
            {
                var data = await dbContext.DS_Doctor_Metadata.FirstOrDefaultAsync(s => s.UserDetailsUser_Id == doctorId);
                return data;
            }
        }
    }
}
