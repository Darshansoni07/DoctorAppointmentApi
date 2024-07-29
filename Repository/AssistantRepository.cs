using DoctorAppointment.Dbcontext;
using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using DoctorAppointment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DoctorAppointment.Repository
{
    public class AssistantRepository : IAssistantRepository
    {
        private readonly AppDbContext dbContext;
        public AssistantRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Assistant getting metaId from email Id from Doctor_Map_Assistant
        /// </summary>
        /// <param name="Email"></param>
        /// <returns> DoctorAssistantMapRegisterDto </returns>
        public async Task<DoctorAssistantMapRegisterDto> AssistantMapDetails(string Email)
        {
            try
            {                
                var data = await dbContext.DS_Doctor_Map_Assistant
                .FirstOrDefaultAsync(doc => doc.Email == Email);

                if (data == null)
                {
                    return null;
                }

                DoctorAssistantMapRegisterDto doctorAssistantMapRegisterDto = new DoctorAssistantMapRegisterDto();
                doctorAssistantMapRegisterDto.Email = data.Email;
                doctorAssistantMapRegisterDto.DoctorMetadataDoc_meta_Id = data.DoctorMetadataDoc_meta_Id;
                doctorAssistantMapRegisterDto.AssistantAssistaId = data.AssistantAssistaId;
                return new DoctorAssistantMapRegisterDto
                {
                    DoctorMetadataDoc_meta_Id = data.DoctorMetadataDoc_meta_Id,
                    AssistantAssistaId = data.AssistantAssistaId,
                    Email = data.Email,
                    IsAssistantApprove = data.IsAssistantApprove
                };
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Exception occurred: {ex.Message}");
                Console.WriteLine($"Error number: {ex.Number}");
                Console.WriteLine($"Error state: {ex.State}");
                Console.WriteLine($"Error severity: {ex.Class}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Assistant getting there Doctor details by metaId
        /// </summary>
        /// <param name="metaId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<AssistantDoctorDataDto> DoctorDetailsByAssistantId(int metaId)
        {
            try
            {
                var doctorDetails = await (from doctor in dbContext.DS_Doctor_Metadata
                                     join user in dbContext.DS_User_Account
                                     on doctor.UserDetailsUser_Id equals user.User_Id
                                     where doctor.Doc_meta_Id == metaId
                                     select new AssistantDoctorDataDto
                                     {
                                         Doc_meta_Id = doctor.Doc_meta_Id,
                                         UserDetailsUser_Id= user.User_Id,
                                         First_Name = user.First_Name,
                                         Last_Name = user.Last_Name,
                                         Specialist = doctor.Specialist,
                                         Email = user.Email,
                                         FeesAmount = doctor.FeesAmount
                                     }).FirstOrDefaultAsync();

                return doctorDetails;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            
        }


        /// <summary>
        /// Register Assistant of doctor personal details 
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        public async Task<AssistantRegisterDto> User_Register(AssistantRegisterDto registerDto)
        {
            
            try
            {
                Assistant user_Details = new Assistant();
                user_Details.Email = registerDto.Email;
                user_Details.First_Name = registerDto.First_Name;
                user_Details.Last_Name = registerDto.Last_Name;
                user_Details.Password = registerDto.Password;
                user_Details.profile_Img = @"assets/images/avatar/01.jpg";
                user_Details.PhoneNumber = registerDto.PhoneNumber;
                user_Details.CreatedOn = DateTime.UtcNow;
                user_Details.UpdatedOn = null; 
                user_Details.roles_UserRole_Id = 4;
                user_Details.MetadataDoc_meta_Id = registerDto.MetadataDoc_meta_Id;
                dbContext.DS_Assistant_Invitation.Add(user_Details);
                dbContext.SaveChanges();               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            await dbContext.SaveChangesAsync();

            return registerDto;
        }

        /// <summary>
        /// Updating assistant and register to user_details map and map with doctorassistant 
        /// </summary>
        /// <param name="mapRegisterDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DoctorAssistantMapRegisterDto> User_Update(DoctorAssistantMapRegisterDto mapRegisterDto)
        {
            try
            {  
                DoctorMapAssistant doctorAssistantMap = new DoctorMapAssistant();
                doctorAssistantMap.AssistantAssistaId = mapRegisterDto.AssistantAssistaId;
                doctorAssistantMap.DoctorMetadataDoc_meta_Id = mapRegisterDto.DoctorMetadataDoc_meta_Id;
                doctorAssistantMap.IsAssistantApprove = mapRegisterDto.IsAssistantApprove;
                doctorAssistantMap.Email = mapRegisterDto.Email;
                dbContext.DS_Doctor_Map_Assistant.Add(doctorAssistantMap);
                dbContext.SaveChanges();
                return mapRegisterDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
            
        }


        public async Task<(List<AssistantDetailDto>, int)> DoctorGetAllOwnAssistant(int metaId, int pageIndex, int pageSize)
        {
            try
            {
                int totalAssistant = await (from totalassit in dbContext.DS_Doctor_Map_Assistant
                                            where totalassit.DoctorMetadataDoc_meta_Id == metaId
                                            select totalassit).CountAsync();

                var doctorDetails = await(from assistant in dbContext.DS_Assistant_Invitation
                                          where assistant.MetadataDoc_meta_Id == metaId
                                          select new AssistantDetailDto { 
                                              AssistaId = assistant.AssistaId,
                                              First_Name = assistant.First_Name,
                                              Last_Name = assistant.Last_Name,
                                              Email = assistant.Email,
                                              profile_Img = assistant.profile_Img,
                                              PhoneNumber = assistant.PhoneNumber,
                                              IsEmailVerified = assistant.IsEmailVerified,
                                              CreatedOn = assistant.CreatedOn,
                                              roles_UserRole_Id = assistant.roles_UserRole_Id,
                                              MetadataDoc_meta_Id = assistant.MetadataDoc_meta_Id
                                          }).ToListAsync();
                                          

                return (doctorDetails,totalAssistant);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
