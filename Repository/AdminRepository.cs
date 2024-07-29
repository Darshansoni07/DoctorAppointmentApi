 using DoctorAppointment.Dbcontext;
using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using DoctorAppointment.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointment.Repository
{
    public class AdminRepository : IAdminRepository
    {

        private readonly TimeZoneInfo IndianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        private readonly AppDbContext dbContext;
        private readonly IAccountRepository accountRepository;
        private readonly IAssistantRepository assistantRepository;
        public AdminRepository(AppDbContext dbContext, IAccountRepository accountRepository, IAssistantRepository assistantRepository) 
        {
            this.assistantRepository = assistantRepository;
            this.accountRepository = accountRepository;
            this.dbContext = dbContext;
        }

        //---------------------------------Doctor Approve------------------------------
        public async Task<bool> DoctorApprove(int Id)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            var user = await dbContext.DS_Doctor_Metadata.FindAsync(Id);
            if (user != null) 
            {
                user.RequestDoctorApprove = true;
                user.UpdatedOn = currentTime;
                dbContext.DS_Doctor_Metadata.Update(user);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Assistant approve by id form admin panel 
        /// </summary>
        /// <param name="assistantMetaId"></param>
        /// <returns>Boolean value</returns>
        public async Task<bool> AssistantApprove(int assistantMetaId)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            try
            {
                var user = await dbContext.DS_Assistant_Invitation.FindAsync(assistantMetaId);
                if (user != null)
                {
                    user.IsEmailVerified = true;
                    user.UpdatedOn = DateTime.UtcNow;
                    dbContext.DS_Assistant_Invitation.Update(user);
                    await dbContext.SaveChangesAsync();

                    //----- Map with user_details table
                    RegisterDto registerDto = new RegisterDto();
                    registerDto.CreatedOn = currentTime;
                    registerDto.Email = user.Email;
                    registerDto.First_Name = user.First_Name;
                    registerDto.Last_Name = user.Last_Name;
                    registerDto.PhoneNumber = user.PhoneNumber;
                    registerDto.Password = user.Password;
                    registerDto.profile_Img = user.profile_Img;
                    registerDto.roles_UserRole_Id = user.roles_UserRole_Id;
                    await accountRepository.User_Register(registerDto);
                    

                    //--- Map with Assistant_map with doctor 
                    DoctorAssistantMapRegisterDto doctorAssistant = new DoctorAssistantMapRegisterDto();
                    doctorAssistant.AssistantAssistaId = user.AssistaId;
                    doctorAssistant.DoctorMetadataDoc_meta_Id = user.MetadataDoc_meta_Id;
                    doctorAssistant.IsAssistantApprove = true;
                    doctorAssistant.Email = user.Email;
                    await assistantRepository.User_Update(doctorAssistant);
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        
        //----------Get All Doctor details which is Approved by Admin ----------------
        public async Task<(List<DoctorAccessReqDto>, int)> GetApprovedDoctorDetails(int pageIndex, int pageSize, string? filter)
        {
            try
            {
                var query = dbContext.DS_Doctor_Metadata
            .Where(doc => doc.RequestDoctorApprove);

                if (!string.IsNullOrEmpty(filter))
                {
                    query = query.Where(doc => doc.Specialist.ToLower().Contains(filter.ToLower()));
                }

                int totalData = await query
                    .Join(dbContext.DS_User_Account,
                        doc => doc.UserDetailsUser_Id,
                        user => user.User_Id,
                        (doc, user) => new { Doctor = doc, User = user })
                    .Select(result => new DoctorAccessReqDto
                    {
                        Doc_meta_Id = result.Doctor.Doc_meta_Id
                    })
                    .CountAsync();

                var result = await query
                    .Join(dbContext.DS_User_Account,
                        doc => doc.UserDetailsUser_Id,
                        user => user.User_Id,
                        (doc, user) => new { Doctor = doc, User = user })
                    .Select(result => new DoctorAccessReqDto
                    {
                        Doc_meta_Id = result.Doctor.Doc_meta_Id,
                        UserDetailsUser_Id = result.Doctor.UserDetailsUser_Id,
                        License = result.Doctor.License,
                        RequestDoctorApprove = result.Doctor.RequestDoctorApprove,
                        Specialist = result.Doctor.Specialist,
                        FeesAmount = result.Doctor.FeesAmount,
                        First_Name = result.User.First_Name,
                        Last_Name = result.User.Last_Name,
                        Email = result.User.Email,
                        profile_Img = result.User.profile_Img,
                        PhoneNumber = result.User.PhoneNumber,
                        Age = CalculateAge(result.User.DOB)
                    })
                    .OrderByDescending(dto => dto.Doc_meta_Id)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (result,totalData);
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception(ex.Message);

            }
        }
        static int? CalculateAge(DateTime? dob)
        {
            if (dob == null)
                return null;

            DateTime today = DateTime.Today;
            DateTime birthDate = dob.Value;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--; // Adjust age if birthday hasn't occurred yet this year
            return age;
        }


        //----------Get All Doctor details which is Approved by Admin ----------------
        public async Task<List<DoctorAccessReqDto>> GetApprovedDoctorDetails()
        {
            try
            {
                var query = dbContext.DS_Doctor_Metadata
            .Where(doc => doc.RequestDoctorApprove)
            .Join(dbContext.DS_User_Account,
                  doc => doc.UserDetailsUser_Id,
                  user => user.User_Id,
                  (doc, user) => new { Doctor = doc, User = user })
            .Select(result => new DoctorAccessReqDto
            {
                Doc_meta_Id = result.Doctor.Doc_meta_Id,
                UserDetailsUser_Id = result.Doctor.UserDetailsUser_Id,
                License = result.Doctor.License,
                RequestDoctorApprove = result.Doctor.RequestDoctorApprove,
                Specialist = result.Doctor.Specialist,
                FeesAmount = result.Doctor.FeesAmount,
                First_Name = result.User.First_Name,
                Last_Name = result.User.Last_Name,
                Email = result.User.Email,
                profile_Img = result.User.profile_Img,
                PhoneNumber = result.User.PhoneNumber
            })
            .OrderByDescending(dto => dto.Doc_meta_Id);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception(ex.Message);

            }
        }

        //-----------------Send Request to Admin Base On RequestDoctorApprove is false---------------------------
        public async Task<(List<DoctorAccessReqDto>,int)> GetDoctorWithRequest(int pageIndex, int pageSize)
        {
            try
            {
                int totalData = await dbContext.DS_Doctor_Metadata
                    .Where(doc => doc.RequestDoctorApprove == false) // Filtering based on RequestDoctorApprove
                    .Join(dbContext.DS_User_Account,
                doc => doc.UserDetailsUser_Id,
                user => user.User_Id,
                (doc, user) => new { Doctor = doc, User = user })
                .CountAsync();

                var data = await dbContext.DS_Doctor_Metadata
                    .Where(doc => doc.RequestDoctorApprove == false) // Filtering based on RequestDoctorApprove
                    .Join(dbContext.DS_User_Account,
                doc => doc.UserDetailsUser_Id,
                user => user.User_Id,
                (doc, user) => new { Doctor = doc, User = user }) // Joining DS_User_Account table
          .Select(result => new DoctorAccessReqDto
          {
              Doc_meta_Id = result.Doctor.Doc_meta_Id,
              UserDetailsUser_Id = result.Doctor.UserDetailsUser_Id,
              Specialist = result.Doctor.Specialist,
              License = result.Doctor.License,
              FeesAmount = result.Doctor.FeesAmount,
              First_Name = result.User.First_Name,
              Last_Name = result.User.Last_Name,
              Email = result.User.Email,
              profile_Img = result.User.profile_Img,
              PhoneNumber = result.User.PhoneNumber
          })
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize)
          .ToListAsync();

                return (data, totalData);
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  Get User Details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ClientDetailDto> GetUserDetailsById(int id)
        {
            try
            {
                return await dbContext.DS_User_Account
                    .Where(doc => doc.User_Id == id)
                    .Select(doc => new ClientDetailDto
                    {
                        User_Id = doc.User_Id,
                        First_Name = doc.First_Name,
                        Last_Name = doc.Last_Name,
                        Email = doc.Email,
                        profile_Img = doc.profile_Img,
                        PhoneNumber = doc.PhoneNumber,
                        CreatedOn = doc.CreatedOn,
                        roles_UserRole_Id = doc.roles_UserRole_Id,
                        Gender = doc.Gender,
                        DOB = doc.DOB,
                        MedicalHistorDescription = doc.MedicalHistorDescription,
                        Address = doc.Address
                    }).FirstOrDefaultAsync();

            }
            catch(Exception ex) {
                Console.Write(ex.Message);
                return null;
            }
        }


        /// <summary>
        ///  Update User Details 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<User_details> UpdateDetailsClient(int id, ClientDetailDto user)
        {
            try
            {
                if (id == null)
                {
                    throw new InvalidOperationException("Id Mismatch");
                }
                var userDetails = await dbContext.DS_User_Account.FindAsync(id);
                if(userDetails.User_Id != null)
                {
                    userDetails.First_Name = user.First_Name;
                    userDetails.Last_Name = user.Last_Name;
                    userDetails.profile_Img = user.profile_Img;
                    userDetails.PhoneNumber = user.PhoneNumber;
                    userDetails.UpdatedOn = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync();
                   
                }
                return userDetails;
                
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw new NotImplementedException(ex.Message);
            }
            
            
        }

        public Task DeleteUserById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Getting Doctor by User Id which is approved by admin 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> Doctor details and metaId</returns>
        /// <exception cref="Exception"></exception>
        public async Task<DoctorAccessReqDto> GetDoctorById(int userId)
        {
            try
            {
                return await dbContext.DS_Doctor_Metadata
                    .Where(doc => doc.UserDetailsUser_Id == userId)
                    .Join(dbContext.DS_User_Account,
                doc => doc.UserDetailsUser_Id,
                user => user.User_Id,
                (doc, user) => new { Doctor = doc, User = user }) 
               .Select(result => new DoctorAccessReqDto
               {
                   Doc_meta_Id = result.Doctor.Doc_meta_Id,
                   UserDetailsUser_Id = result.Doctor.UserDetailsUser_Id,
                   License = result.Doctor.License,
                   RequestDoctorApprove = result.Doctor.RequestDoctorApprove,
                   Specialist = result.Doctor.Specialist,
                   FeesAmount = result.Doctor.FeesAmount,
                   First_Name = result.User.First_Name,
                   Last_Name = result.User.Last_Name,
                   Email = result.User.Email,
                   profile_Img = result.User.profile_Img,
                   PhoneNumber = result.User.PhoneNumber
               }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception(ex.Message);

            }
        }

        /// <summary>
        /// Getting all Assistant of doctor request for pending 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<AssistantDetailDto>> GetAssistantWithRequest()
        {
            try
            {
                var data = await dbContext.DS_Assistant_Invitation
                    .Where(doc => doc.IsEmailVerified == false)
                    .OrderByDescending(dbo => dbo.AssistaId)
                    .Select(dbo => new AssistantDetailDto
                    {
                        AssistaId = dbo.AssistaId,
                        Email = dbo.Email,
                        First_Name = dbo.First_Name,
                        Last_Name = dbo.Last_Name,
                        PhoneNumber = dbo.PhoneNumber,
                        CreatedOn = dbo.CreatedOn,
                        profile_Img = dbo.profile_Img,
                        IsEmailVerified = dbo.IsEmailVerified,
                        roles_UserRole_Id = dbo.roles_UserRole_Id,
                        MetadataDoc_meta_Id = dbo.MetadataDoc_meta_Id
                    }).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw new Exception(ex.Message);//Return An empty
            }
        }

        //----------------------------------- Getting ALL Client details ----------------------
        public async Task<(List<ClientDetailDto>,int)> GetClientDetials(int pageIndex, int pageSize)
        {
            try
            {
                var size = await dbContext.DS_User_Account
                    .Where(dbo => dbo.roles_UserRole_Id == 3 && dbo.IsEmailVerified)
                    .OrderByDescending(dbo => dbo.User_Id)
                    .Select(dbo => new ClientDetailDto
                    {
                        Email = dbo.Email,
                        First_Name = dbo.First_Name,
                        Last_Name = dbo.Last_Name,
                        PhoneNumber = dbo.PhoneNumber,
                        CreatedOn = dbo.CreatedOn,
                        UpdatedOn = dbo.UpdatedOn,
                        profile_Img = dbo.profile_Img,
                        User_Id = dbo.User_Id
                    })
                    .CountAsync();

                var data= await dbContext.DS_User_Account
                    .Where(dbo => dbo.roles_UserRole_Id == 3 && dbo.IsEmailVerified)
                    .OrderByDescending(dbo => dbo.User_Id)
                    .Select(dbo => new ClientDetailDto
                    {
                        Email = dbo.Email,
                        First_Name = dbo.First_Name,
                        Last_Name = dbo.Last_Name,
                        PhoneNumber = dbo.PhoneNumber,
                        CreatedOn = dbo.CreatedOn,
                        UpdatedOn = dbo.UpdatedOn,
                        profile_Img = dbo.profile_Img,
                        User_Id = dbo.User_Id
                    })
                    .Skip(pageIndex*pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (data, size);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Getting all approved Assistant details with doctor name 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>AssistantDoctorDetailsMapDto, count value </returns>
        /// <exception cref="Exception"></exception>
        public async Task<(List<AssistantDoctorDetailsMapDto>,int)> GetApprovedAssistantDetails(int pageIndex, int pageSize)
        {
            try
            {
                int totalCount = await (from map in dbContext.DS_Doctor_Map_Assistant
                                        join user in dbContext.DS_Assistant_Invitation
                                        on map.AssistantAssistaId equals user.AssistaId
                                        join doct in dbContext.DS_Doctor_Metadata
                                        on map.DoctorMetadataDoc_meta_Id equals doct.Doc_meta_Id
                                        join tuser in dbContext.DS_User_Account
                                        on doct.UserDetailsUser_Id equals tuser.User_Id
                                        select map).CountAsync();

                var data =  await (from map in dbContext.DS_Doctor_Map_Assistant
                            join user in dbContext.DS_Assistant_Invitation
                            on map.AssistantAssistaId equals user.AssistaId
                            join doct in dbContext.DS_Doctor_Metadata
                            on map.DoctorMetadataDoc_meta_Id equals doct.Doc_meta_Id
                            join tuser in dbContext.DS_User_Account 
                            on doct.UserDetailsUser_Id equals tuser.User_Id
                            orderby map.DoctorAssistantMapId
                            select new AssistantDoctorDetailsMapDto
                            {
                                Assistant_F_Name = user.First_Name,
                                Assistant_L_Name = user.Last_Name,
                                Doc_F_Name = tuser.First_Name,
                                Doc_L_Name = tuser.Last_Name,
                                FeesAmount = doct.FeesAmount,
                                Specialist = doct.Specialist,
                                Doc_meta_Id = map.DoctorMetadataDoc_meta_Id,
                                Email = map.Email
                            })
                            .Skip(pageIndex*pageSize)                            
                            .Take(pageSize)
                            .ToListAsync();

                return (data,totalCount);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}