using DoctorAppointment.Dbcontext;
using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using DoctorAppointment.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace DoctorAppointment.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly TimeZoneInfo IndianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        private readonly AppDbContext dbContext;
        private readonly IReportRepository reportRepository;

        public AppointmentRepository(AppDbContext dbContext, IReportRepository reportRepository)
        {

            this.dbContext = dbContext;
            this.reportRepository = reportRepository;
        }

        //----------------------------------------- Approve Appointment By Doctor or Assistant
        public async Task<string> AppointmentApprove(int appointmentId)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            try
            {
                if(appointmentId == 0)
                {
                    return "Id Not Found";
                }
                else
                {
                    var data = await dbContext.DS_Appointment.FindAsync(appointmentId);
                    if (data == null)
                    {
                        return "data is not available";
                    }
                    else
                    {
                        if(data.SlotId != 0) {
                        
                            var slotdata = await dbContext.DS_slots.FindAsync(data.SlotId);
                            if(slotdata != null)
                            {
                                slotdata.Status = "Booked";
                                slotdata.UpdatedOn = currentTime;
                            }
                        }
                        data.ApproveStatus = "1";
                        data.Status = "Booked";
                        data.ReportFile = null;
                        data.UpdatedOn = currentTime;
                        dbContext.DS_Appointment.Update(data);
                        await dbContext.SaveChangesAsync();
                        return "Appointment Approved";
                    }
                }
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }

        
        //--------------------------------------- Create Appointment 
        public async Task<string> CreateAsync(AppointmentCreateDto appointmentCreateDto)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            try
            {
                if(appointmentCreateDto == null)
                {
                    return null;
                }
                else
                {
                    var slot = await dbContext.DS_slots.FindAsync(appointmentCreateDto.SlotId);
                    if (slot == null)
                    {
                        return "Slot not found";
                    }
                    if (slot.StartTimeslot >= currentTime)
                    {
                        if(slot.Status == "Booked" || slot.Status=="Cancel" || slot.Status=="Reserved") 
                        {
                            return "Slot Booked";
                        }
                        var exist = await (from appoint in dbContext.DS_Appointment
                                           where appoint.AppointmentTime.Value.Date == slot.StartTimeslot.Date && appoint.UserDetailsUser_Id == appointmentCreateDto.UserDetailsUser_Id
                                           select appoint
                                           ).AnyAsync();
                        if (exist)
                        {
                            return "You Have Already Booked for this Day";
                        }
                        Appointment appointment = new Appointment();
                        appointment.SlotId = appointmentCreateDto.SlotId;
                        appointment.UserDetailsUser_Id = appointmentCreateDto.UserDetailsUser_Id;
                        appointment.ApproveStatus = "0";
                        appointment.AppointmentTime = slot.StartTimeslot;
                        appointment.Status = "Pending";
                        appointment.DoctorMetadataDoc_meta_Id = slot.MetadataDoc_meta_Id ;
                        appointment.CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
                        dbContext.DS_Appointment.Add(appointment);
                        if (slot != null)
                        {
                            slot.Status = "Reserved";
                            slot.UpdatedOn = currentTime;
                        }
                        dbContext.SaveChanges();
                        return "Appointment Booked";
                    }
                    else
                    {
                        return "This Time Session End";
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       
        //------------------------------------- Get Appintment Details By Appointment Id
        public async Task<Object> GetAppointmentDetailsById(int id)
        {           
            try
            {
                var data = await dbContext.DS_Appointment.FirstOrDefaultAsync(doc => doc.Appointment_Id == id);
                if (data.ReportFile != null &&  data.ReportFile != "")
                {
                    var reportdata = await reportRepository.GetReportData(data.ReportFile);
                    string heartRate = reportdata.ContainsKey("Heart Rate") ? reportdata["Heart Rate"] : null;
                    string Booldpressure = reportdata.ContainsKey("BP") ? reportdata["BP"] : null;
                    string Sugar = reportdata.ContainsKey("Sugar") ? reportdata["Sugar"] : null;
                    string Description = reportdata.ContainsKey("Description") ? reportdata["Description"] : null;
                    string Medicine = reportdata.ContainsKey("Medicine") ? reportdata["Medicine"] : null;
                    return new AppointReportDto
                    {
                        Appointment_Id = data.Appointment_Id,
                        Status = data.Status,
                        ApproveStatus = data.ApproveStatus,
                        AppointmentTime = data.AppointmentTime,
                        CreatedOn = data.CreatedOn,
                        UpdatedOn = data.UpdatedOn,
                        SlotId = data.SlotId,
                        UserDetailsUser_Id = data.UserDetailsUser_Id,
                        BP = Booldpressure,
                        HeartRate = heartRate,
                        Sugar = Sugar,
                        Description = Description,
                        Medicine = Medicine
                    };
                }
                else
                {
                    return new AppointmentDetailDto
                    {
                        Appointment_Id = data.Appointment_Id,
                        Status = data.Status,
                        ApproveStatus = data.ApproveStatus,
                        AppointmentTime = data.AppointmentTime,
                        CreatedOn = data.CreatedOn,
                        UpdatedOn = data.UpdatedOn,
                        SlotId = data.SlotId,
                        UserDetailsUser_Id = data.UserDetailsUser_Id
                    };                
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        
        //------------------------------------- Get All Appointment where requested
        public async Task<(List<AppointmentUserDetailsDto>,int)> GetAllAppointmentWithRequest(int metaId, int pageIndex, int pageSize)
        {            
            try
            {
                var totaldata = await dbContext.DS_Appointment
                    .Where(appointment => appointment.ApproveStatus == "0" && appointment.DoctorMetadataDoc_meta_Id == metaId)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_User_Account,
                    appointment => appointment.UserDetailsUser_Id,
                    user => user.User_Id,
                    (appointment, user) => new { Appointment = appointment, User = user })
                    .CountAsync();

                var appointmentsWithUsers = await dbContext.DS_Appointment
                    .Where(appointment => appointment.ApproveStatus == "0" && appointment.DoctorMetadataDoc_meta_Id == metaId)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_User_Account,
                    appointment => appointment.UserDetailsUser_Id,
                    user => user.User_Id,
                    (appointment, user) => new { Appointment = appointment, User = user })
                    .Select(res => new AppointmentUserDetailsDto
                    {
                        Appointment_Id = res.Appointment.Appointment_Id,
                        Status = res.Appointment.Status,
                        ApproveStatus = res.Appointment.ApproveStatus,
                        AppointmentTime = res.Appointment.AppointmentTime,
                        SlotId = res.Appointment.SlotId,
                        UserDetailsUser_Id = res.Appointment.UserDetailsUser_Id,
                        First_Name = res.User.First_Name,
                        Last_Name = res.User.Last_Name,
                        PhoneNumber = res.User.PhoneNumber,
                    })
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (appointmentsWithUsers,totaldata);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        
        /// <summary>
        /// Updating User Appointment details and adding there medical history details 
        /// </summary>
        /// <param name="appointId"></param>
        /// <param name="appoint"></param>
        /// <returns></returns>
        public async Task<string> UpdateAppointmentDetail(int appointId, AppointmentUpdateDto appoint)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            try
            {
                if(appointId==0 && appoint == null)
                {
                    return "Invalid data";
                }
                var data = await dbContext.DS_Appointment.FindAsync(appointId);
                var userAppoint = await (from apoint in dbContext.DS_Appointment
                                         where apoint.Appointment_Id == appointId
                                         join userDoc in dbContext.DS_Doctor_Metadata
                                         on apoint.DoctorMetadataDoc_meta_Id equals userDoc.Doc_meta_Id
                                         join userDetail in dbContext.DS_User_Account
                                         on userDoc.UserDetailsUser_Id equals userDetail.User_Id
                                         select new AppointmentUserDetailsDto 
                                         { 
                                             UserDetailsUser_Id = apoint.UserDetailsUser_Id,
                                             DoctorMetadataDoc_meta_Id = apoint.DoctorMetadataDoc_meta_Id,
                                             SlotId = apoint.SlotId,
                                             First_Name = userDetail.First_Name,
                                             Last_Name = userDetail.Last_Name,
                                             Status = apoint.Status,
                                             ApproveStatus = apoint.ApproveStatus,
                                             AppointmentTime = apoint.AppointmentTime,
                                             ReportFile = apoint.ReportFile,
                                             Appointment_Id = apoint.Appointment_Id,

                                         }).FirstOrDefaultAsync();

                if (data != null)
                {
                    data.Status = appoint.Status;
                    data.UpdatedOn = currentTime;
                    if (appoint.AppointmentTime != null)
                    {
                        data.AppointmentTime = appoint.AppointmentTime;
                    }
                    var medicalhistor = await dbContext.DS_User_Account.FindAsync(data.UserDetailsUser_Id);
                    if(data.Status == "Cancel" || data.Status == "Booked" || data.Status == "Checked")
                    {
                        string subject = "Appointment Status " + data.Status;
                        string body = "Your Appointment status is " + data.Status + ". <br> Date:"+ appoint.AppointmentTime + ". <br>Check your booking details ";
                        SendVerifyEmail(medicalhistor.Email, subject, body);
                    }
                    if (appoint != null)
                    {
                        appoint.Address = medicalhistor.Address;
                        appoint.PhoneNumber = medicalhistor.PhoneNumber;
                        appoint.Gender = medicalhistor.Gender;
                        appoint.First_Name = medicalhistor.First_Name;
                        appoint.Last_Name = medicalhistor.Last_Name;
                        appoint.DOB = medicalhistor.DOB;
                        appoint.Email = medicalhistor.Email;
                        appoint.Doctor_Name = "Dr. "+ userAppoint.First_Name + " " + userAppoint.Last_Name;
                    }
                    var url = await reportRepository.CreateReport(appoint);
                    if(url != null)
                    {
                        data.ReportFile = url;
                        if (medicalhistor.MedicalHistorDescription == null)
                        {
                            medicalhistor.MedicalHistorDescription = url;
                        }
                        else
                        {
                            medicalhistor.MedicalHistorDescription += "," + url;

                        }
                        await dbContext.SaveChangesAsync();
                        return "Update Successfully";
                    }
                    return "Report Not Genrate";                                      
                }
                else
                {
                    return "Appointment not found";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //--------------------------------------- GetAllAppointments
        public async Task<List<AppointmentUserDetailsDto>> GetAllAppointments(int metaId)
        {
            try
            {
                var appointments = await dbContext.DS_Appointment
        .Join(
            dbContext.DS_User_Account,
            appointment => appointment.UserDetailsUser_Id,
            user => user.User_Id,
            (appointment, user) => new
            {
                Appointment = appointment,
                User = user
            })
        .Where(joinResult => joinResult.Appointment.DoctorMetadataDoc_meta_Id == metaId)
        .Select(joinResult => new AppointmentUserDetailsDto
        {
            Appointment_Id = joinResult.Appointment.Appointment_Id,
            Status = joinResult.Appointment.Status,
            ApproveStatus = joinResult.Appointment.ApproveStatus,
            AppointmentTime = joinResult.Appointment.AppointmentTime,
            SlotId = joinResult.Appointment.SlotId,
            UserDetailsUser_Id = joinResult.Appointment.UserDetailsUser_Id,
            First_Name = joinResult.User.First_Name,
            Last_Name = joinResult.User.Last_Name,
            PhoneNumber = joinResult.User.PhoneNumber
        })
        .ToListAsync();
                return appointments;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);

            }            
        }

        /// <summary>
        ///  Get All Approved Appointment Sort By Current Date Equal and Greater 
        /// </summary>
        /// <param name="metaId"></param>
        /// <returns> Appointment Data Of User </returns>
        /// <exception cref="Exception"></exception>
        public async Task<(List<AppointmentUserDetailsDto>, int)> GetAllApprovedAppointments(int metaId, int pageIndex, int pageSize)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            try
            {
                int totalAppointment = await (from data in dbContext.DS_Appointment
                                              where data.ApproveStatus == "1" && data.DoctorMetadataDoc_meta_Id == metaId  && data.AppointmentTime.Value.Date >= currentTime.Date
                                              select data).CountAsync();

                var appointmentsWithUsers = await dbContext.DS_Appointment
                    .Where(appointment => appointment.ApproveStatus == "1" && appointment.AppointmentTime.Value.Date >= currentTime.Date && appointment.DoctorMetadataDoc_meta_Id == metaId)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_User_Account,
                    appointment => appointment.UserDetailsUser_Id,
                    user => user.User_Id,
                    (appointment, user) => new { Appointment = appointment, User = user })
                    .Select(res => new AppointmentUserDetailsDto
                    {
                        Appointment_Id = res.Appointment.Appointment_Id,
                        Status = res.Appointment.Status,
                        ApproveStatus = res.Appointment.ApproveStatus,
                        AppointmentTime = res.Appointment.AppointmentTime,
                        SlotId = res.Appointment.SlotId,
                        UserDetailsUser_Id = res.Appointment.UserDetailsUser_Id,
                        First_Name = res.User.First_Name,
                        Last_Name = res.User.Last_Name,
                        PhoneNumber = res.User.PhoneNumber,
                        ReportFile = res.Appointment.ReportFile
                    })
                    .Skip(pageIndex*pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (appointmentsWithUsers,totalAppointment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //-------------------------------------- Get All Today Appointment  
        public async Task<List<AppointmentUserDetailsDto>> GetAllTodayAppointments(int metaId)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            try
            {
                var appointmentsWithUsers = await dbContext.DS_Appointment
                    .Where(appointment => appointment.ApproveStatus == "1" && appointment.AppointmentTime.Value.Date == currentTime.Date && appointment.DoctorMetadataDoc_meta_Id == metaId)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_User_Account,
                    appointment => appointment.UserDetailsUser_Id,
                    user => user.User_Id,
                    (appointment, user) => new { Appointment = appointment, User = user })
                    .Select(res => new AppointmentUserDetailsDto
                    {
                        Appointment_Id = res.Appointment.Appointment_Id,
                        Status = res.Appointment.Status,
                        ApproveStatus = res.Appointment.ApproveStatus,
                        AppointmentTime = res.Appointment.AppointmentTime,
                        SlotId = res.Appointment.SlotId,
                        UserDetailsUser_Id = res.Appointment.UserDetailsUser_Id,
                        First_Name = res.User.First_Name,
                        Last_Name = res.User.Last_Name,
                        PhoneNumber = res.User.PhoneNumber,
                    })
                    .Take(5)
                    .ToListAsync();

                return appointmentsWithUsers;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Getting Old Appointments of user Details 
        /// </summary>
        /// <param name="metaId"></param> 
        /// <returns> User details of appointment </returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<AppointmentUserDetailsDto>> GetAllAppointmentPervious(int metaId)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            try
            {
                var appointmentsWithUsers = await dbContext.DS_Appointment
                    .Where(appointment => appointment.ApproveStatus == "1" && appointment.AppointmentTime.Value.Date <= currentTime.Date && appointment.DoctorMetadataDoc_meta_Id == metaId)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_User_Account,
                    appointment => appointment.UserDetailsUser_Id,
                    user => user.User_Id,
                    (appointment, user) => new { Appointment = appointment, User = user })
                    .Select(res => new AppointmentUserDetailsDto
                    {
                        Appointment_Id = res.Appointment.Appointment_Id,
                        Status = res.Appointment.Status,
                        ApproveStatus = res.Appointment.ApproveStatus,
                        AppointmentTime = res.Appointment.AppointmentTime,
                        SlotId = res.Appointment.SlotId,
                        UserDetailsUser_Id = res.Appointment.UserDetailsUser_Id,
                        First_Name = res.User.First_Name,
                        Last_Name = res.User.Last_Name,
                        PhoneNumber = res.User.PhoneNumber,
                    })
                    .Take(5)
                    .ToListAsync();

                return appointmentsWithUsers;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// GET ALL Detail of Users where selected date appointments 
        /// </summary>
        /// <param name="metaId"></param>
        /// <param name="date"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>  User data of appointment </returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<AppointmentUserDetailsDto>> GetAllSelectedDateAppointment(int metaId, DateTime date, int pageIndex, int pageSize)
        {
            try
            {
                var appointmentsWithUsers = await dbContext.DS_Appointment
                    .Where(appointment => appointment.ApproveStatus == "1" && appointment.AppointmentTime.Value.Date == date.Date && appointment.DoctorMetadataDoc_meta_Id == metaId)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_User_Account,
                    appointment => appointment.UserDetailsUser_Id,
                    user => user.User_Id,
                    (appointment, user) => new { Appointment = appointment, User = user })
                    .Select(res => new AppointmentUserDetailsDto
                    {
                        Appointment_Id = res.Appointment.Appointment_Id,
                        Status = res.Appointment.Status,
                        ApproveStatus = res.Appointment.ApproveStatus,
                        AppointmentTime = res.Appointment.AppointmentTime,
                        SlotId = res.Appointment.SlotId,
                        UserDetailsUser_Id = res.Appointment.UserDetailsUser_Id,
                        First_Name = res.User.First_Name,
                        Last_Name = res.User.Last_Name,
                        PhoneNumber = res.User.PhoneNumber,
                    })
                    .Skip(pageIndex*pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return appointmentsWithUsers;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Getting All Approve Appointment where pervious and upcoming value both are Come
        /// </summary>
        /// <param name="metaId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns> AppointmentUserDetailsDto </returns>
        /// <exception cref="Exception"></exception>
        public async Task<(List<AppointmentUserDetailsDto>,int,int,int)> GetAllApproveAppointment(int metaId, int pageIndex, int pageSize)
        {
            try
            {
                int totalAppointment = await (from data in dbContext.DS_Appointment
                                             where data.ApproveStatus == "1" && data.DoctorMetadataDoc_meta_Id == metaId && data.Status == "Checked"
                                             select data).CountAsync();

                int feesamount = await (from data in dbContext.DS_Appointment
                                        join meta in dbContext.DS_Doctor_Metadata
                                        on data.DoctorMetadataDoc_meta_Id == metaId equals meta.Doc_meta_Id == metaId
                                        select meta.FeesAmount).FirstAsync();

                int totalData = await dbContext.DS_Appointment
                    .Where(appointment => appointment.ApproveStatus == "1" && appointment.DoctorMetadataDoc_meta_Id == metaId)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_User_Account,
                    appointment => appointment.UserDetailsUser_Id,
                    user => user.User_Id,
                    (appointment, user) => new { Appointment = appointment, User = user })
                    .CountAsync();


                var appointmentsWithUsers = await dbContext.DS_Appointment
                    .Where(appointment => appointment.ApproveStatus == "1" && appointment.DoctorMetadataDoc_meta_Id == metaId)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_User_Account,
                    appointment => appointment.UserDetailsUser_Id,
                    user => user.User_Id,
                    (appointment, user) => new { Appointment = appointment, User = user })
                    .Select(res => new AppointmentUserDetailsDto
                    {
                        Appointment_Id = res.Appointment.Appointment_Id,
                        Status = res.Appointment.Status,
                        ApproveStatus = res.Appointment.ApproveStatus,
                        AppointmentTime = res.Appointment.AppointmentTime,
                        SlotId = res.Appointment.SlotId,
                        UserDetailsUser_Id = res.Appointment.UserDetailsUser_Id,
                        First_Name = res.User.First_Name,
                        Last_Name = res.User.Last_Name,
                        PhoneNumber = res.User.PhoneNumber,
                        DoctorMetadataDoc_meta_Id = res.Appointment.DoctorMetadataDoc_meta_Id,
                        ReportFile = res.Appointment.ReportFile
                    })
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (appointmentsWithUsers, totalAppointment, feesamount, totalData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Getting All Upcoming Booking details of user at perticular doctor 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(List<AppointmentBookingDetailUserDto>,int)> GetAllDetailOfBookingUser(int userId, int pageIndex, int pageSize)
        {
            try
            {
                int totaldata = await dbContext.DS_Appointment
                    .Where(appointment => appointment.UserDetailsUser_Id == userId)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_Doctor_Metadata,
                        appointment => appointment.DoctorMetadataDoc_meta_Id,
                        doctor => doctor.Doc_meta_Id,
                        (appointment, doctor) => new { Appointment = appointment, Doctor = doctor })
                    .Join(dbContext.DS_User_Account,
                        appointmentDoctor => appointmentDoctor.Appointment.UserDetailsUser_Id,
                        user => user.User_Id,
                        (appointmentDoctor, user) => new { AppointmentDoctor = appointmentDoctor, User = user })
                    .Select(res => new AppointmentBookingDetailUserDto
                    {
                        Appointment_Id = res.AppointmentDoctor.Appointment.Appointment_Id,
                       
                    }).CountAsync();


                var appointmentsWithUsersAndDoctors = await dbContext.DS_Appointment
                    .Where(appointment => appointment.UserDetailsUser_Id == userId)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_Doctor_Metadata,
                        appointment => appointment.DoctorMetadataDoc_meta_Id,
                        doctor => doctor.Doc_meta_Id,
                        (appointment, doctor) => new { Appointment = appointment, Doctor = doctor })
                    .Join(dbContext.DS_User_Account,
                        appointmentDoctor => appointmentDoctor.Appointment.UserDetailsUser_Id,
                        user => user.User_Id,
                        (appointmentDoctor, user) => new { AppointmentDoctor = appointmentDoctor, User = user })
                    .Select(res => new AppointmentBookingDetailUserDto
                    {
                        Appointment_Id = res.AppointmentDoctor.Appointment.Appointment_Id,
                        Status = res.AppointmentDoctor.Appointment.Status,
                        ApproveStatus = res.AppointmentDoctor.Appointment.ApproveStatus,
                        AppointmentTime = res.AppointmentDoctor.Appointment.AppointmentTime,
                        SlotId = res.AppointmentDoctor.Appointment.SlotId,
                        UserDetailsUser_Id = res.AppointmentDoctor.Appointment.UserDetailsUser_Id,
                        Specialist = res.AppointmentDoctor.Doctor.Specialist,
                        FeesAmount = res.AppointmentDoctor.Doctor.FeesAmount,
                        ReportFile = res.AppointmentDoctor.Appointment.ReportFile
                    })
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                

                return (appointmentsWithUsersAndDoctors,totaldata);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        /// <summary>
        /// Admin gat all booking appointments details and return totalcount and total income and data 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>AppointmentUserDetailsDto</returns>
        /// <exception cref="Exception"></exception>
        public async Task<(List<AppointmentUserDetailsDto>,int, int)> AdminGetAllBookedAppoinments(int pageIndex, int pageSize)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            try
            {                
                var count = await (from appointment in dbContext.DS_Appointment
                                   where appointment.ApproveStatus=="1" && appointment.Status == "Checked"  && appointment.AppointmentTime <= currentTime
                                   select appointment).CountAsync();
                int totalIncome = count * 30;
                                

                var appointmentsWithUsers = await dbContext.DS_Appointment
                    .Where(appointment => appointment.ApproveStatus == "1" && appointment.Status == "Checked")
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_User_Account,
                    appointment => appointment.UserDetailsUser_Id,
                    user => user.User_Id,
                    (appointment, user) => new { Appointment = appointment, User = user })
                    .Select(res => new AppointmentUserDetailsDto
                    {
                        Appointment_Id = res.Appointment.Appointment_Id,
                        Status = res.Appointment.Status,
                        ApproveStatus = res.Appointment.ApproveStatus,
                        AppointmentTime = res.Appointment.AppointmentTime,
                        SlotId = res.Appointment.SlotId,
                        UserDetailsUser_Id = res.Appointment.UserDetailsUser_Id,
                        First_Name = res.User.First_Name,
                        Last_Name = res.User.Last_Name,
                        PhoneNumber = res.User.PhoneNumber,
                        DoctorMetadataDoc_meta_Id = res.Appointment.DoctorMetadataDoc_meta_Id
                    })
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();


                return (appointmentsWithUsers, count, totalIncome);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool SendVerifyEmail(string user_Email, string subject, string body)
        {
            try
            {
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "mailsenderboxa@gmail.com"; // Your sender email address
                string smtpPassword = "iwfa oerp mjvi gojn"; // Your sender email password or app-specific password
                string baseurl = $"http://localhost:4200/Login";
                
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    body += $"<a href='{baseurl}'>click here login your account</a>";
                    body += $"<br><br>regard <a href='{baseurl}'><b>docAppointment</b></a>";
                    MailMessage mailMessage = new MailMessage(smtpUsername, user_Email, subject, body);
                    mailMessage.IsBodyHtml = true;
                    smtpClient.Send(mailMessage);
                }
                return true;

            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Patient Getting All Appointment details in list form also calculated data 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(List<AppointmentBookingDetailUserDto>, int, int, int, int, int, int)> PaitentGetAllAppointmentDetails(int userId, int pageIndex, int pageSize)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
            try
            {
                int totaldata = await dbContext.DS_Appointment
                    .Where(appointment => appointment.UserDetailsUser_Id == userId)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_Doctor_Metadata,
                        appointment => appointment.DoctorMetadataDoc_meta_Id,
                        doctor => doctor.Doc_meta_Id,
                        (appointment, doctor) => new { Appointment = appointment, Doctor = doctor })
                    .Join(dbContext.DS_User_Account,
                        appointmentDoctor => appointmentDoctor.Appointment.UserDetailsUser_Id,
                        user => user.User_Id,
                        (appointmentDoctor, user) => new { AppointmentDoctor = appointmentDoctor, User = user })
                    .Select(res => new AppointmentBookingDetailUserDto
                    {
                        Appointment_Id = res.AppointmentDoctor.Appointment.Appointment_Id,

                    }).CountAsync();

                int totalBooked = await dbContext.DS_Appointment
                    .Where(appointment => appointment.UserDetailsUser_Id == userId && appointment.Status == "Booked")
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_Doctor_Metadata,
                        appointment => appointment.DoctorMetadataDoc_meta_Id,
                        doctor => doctor.Doc_meta_Id,
                        (appointment, doctor) => new { Appointment = appointment, Doctor = doctor })
                    .Join(dbContext.DS_User_Account,
                        appointmentDoctor => appointmentDoctor.Appointment.UserDetailsUser_Id,
                        user => user.User_Id,
                        (appointmentDoctor, user) => new { AppointmentDoctor = appointmentDoctor, User = user })
                    .Select(res => new AppointmentBookingDetailUserDto
                    {
                        Appointment_Id = res.AppointmentDoctor.Appointment.Appointment_Id,

                    }).CountAsync();

                int totalPending = await dbContext.DS_Appointment
                    .Where(appointment => appointment.UserDetailsUser_Id == userId && appointment.Status == "Pending")
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_Doctor_Metadata,
                        appointment => appointment.DoctorMetadataDoc_meta_Id,
                        doctor => doctor.Doc_meta_Id,
                        (appointment, doctor) => new { Appointment = appointment, Doctor = doctor })
                    .Join(dbContext.DS_User_Account,
                        appointmentDoctor => appointmentDoctor.Appointment.UserDetailsUser_Id,
                        user => user.User_Id,
                        (appointmentDoctor, user) => new { AppointmentDoctor = appointmentDoctor, User = user })
                    .Select(res => new AppointmentBookingDetailUserDto
                    {
                        Appointment_Id = res.AppointmentDoctor.Appointment.Appointment_Id,

                    }).CountAsync();

                int totalCheched = await dbContext.DS_Appointment
                    .Where(appointment => appointment.UserDetailsUser_Id == userId && appointment.Status == "Checked")
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_Doctor_Metadata,
                        appointment => appointment.DoctorMetadataDoc_meta_Id,
                        doctor => doctor.Doc_meta_Id,
                        (appointment, doctor) => new { Appointment = appointment, Doctor = doctor })
                    .Join(dbContext.DS_User_Account,
                        appointmentDoctor => appointmentDoctor.Appointment.UserDetailsUser_Id,
                        user => user.User_Id,
                        (appointmentDoctor, user) => new { AppointmentDoctor = appointmentDoctor, User = user })
                    .Select(res => new AppointmentBookingDetailUserDto
                    {
                        Appointment_Id = res.AppointmentDoctor.Appointment.Appointment_Id,

                    }).CountAsync();

                int totalCancle = await dbContext.DS_Appointment
                    .Where(appointment => appointment.UserDetailsUser_Id == userId && appointment.Status == "Cancel")
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_Doctor_Metadata,
                        appointment => appointment.DoctorMetadataDoc_meta_Id,
                        doctor => doctor.Doc_meta_Id,
                        (appointment, doctor) => new { Appointment = appointment, Doctor = doctor })
                    .Join(dbContext.DS_User_Account,
                        appointmentDoctor => appointmentDoctor.Appointment.UserDetailsUser_Id,
                        user => user.User_Id,
                        (appointmentDoctor, user) => new { AppointmentDoctor = appointmentDoctor, User = user })
                    .Select(res => new AppointmentBookingDetailUserDto
                    {
                        Appointment_Id = res.AppointmentDoctor.Appointment.Appointment_Id,

                    }).CountAsync();

                var totalUpcoming = await dbContext.DS_Appointment
                    .Where(appointment => appointment.UserDetailsUser_Id == userId && appointment.AppointmentTime.Value.Date >= currentTime.Date)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_Doctor_Metadata,
                        appointment => appointment.DoctorMetadataDoc_meta_Id,
                        doctor => doctor.Doc_meta_Id,
                        (appointment, doctor) => new { Appointment = appointment, Doctor = doctor })
                    .Join(dbContext.DS_User_Account,
                        appointmentDoctor => appointmentDoctor.Appointment.UserDetailsUser_Id,
                        user => user.User_Id,
                        (appointmentDoctor, user) => new { AppointmentDoctor = appointmentDoctor, User = user })
                    .Select(res => new AppointmentBookingDetailUserDto
                    {
                        Appointment_Id = res.AppointmentDoctor.Appointment.Appointment_Id,
                        Status = res.AppointmentDoctor.Appointment.Status,
                        ApproveStatus = res.AppointmentDoctor.Appointment.ApproveStatus,
                        AppointmentTime = res.AppointmentDoctor.Appointment.AppointmentTime,
                        SlotId = res.AppointmentDoctor.Appointment.SlotId,
                        UserDetailsUser_Id = res.AppointmentDoctor.Appointment.UserDetailsUser_Id,
                        Specialist = res.AppointmentDoctor.Doctor.Specialist,
                        FeesAmount = res.AppointmentDoctor.Doctor.FeesAmount
                    }).CountAsync();



                var appointmentsWithUsersAndDoctors = await dbContext.DS_Appointment
                    .Where(appointment => appointment.UserDetailsUser_Id == userId && appointment.AppointmentTime.Value.Date >= currentTime.Date)
                    .OrderByDescending(appointment => appointment.Appointment_Id)
                    .Join(dbContext.DS_Doctor_Metadata,
                        appointment => appointment.DoctorMetadataDoc_meta_Id,
                        doctor => doctor.Doc_meta_Id,
                        (appointment, doctor) => new { Appointment = appointment, Doctor = doctor })
                    .Join(dbContext.DS_User_Account,
                        appointmentDoctor => appointmentDoctor.Appointment.UserDetailsUser_Id,
                        user => user.User_Id,
                        (appointmentDoctor, user) => new { AppointmentDoctor = appointmentDoctor, User = user })
                    .Select(res => new AppointmentBookingDetailUserDto
                    {
                        Appointment_Id = res.AppointmentDoctor.Appointment.Appointment_Id,
                        Status = res.AppointmentDoctor.Appointment.Status,
                        ApproveStatus = res.AppointmentDoctor.Appointment.ApproveStatus,
                        AppointmentTime = res.AppointmentDoctor.Appointment.AppointmentTime,
                        SlotId = res.AppointmentDoctor.Appointment.SlotId,
                        UserDetailsUser_Id = res.AppointmentDoctor.Appointment.UserDetailsUser_Id,
                        Specialist = res.AppointmentDoctor.Doctor.Specialist,
                        FeesAmount = res.AppointmentDoctor.Doctor.FeesAmount
                    })
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();


                return (appointmentsWithUsersAndDoctors, totaldata,totalBooked,totalCheched,totalPending,totalUpcoming, totalCancle);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        /// Getting Appointment Report details by appointment Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Dictionary<string, string>> GetAppointmentReportById(int appointId) 
        {
            try
            { 
                if (appointId != 0)
                {
                    var data = await dbContext.DS_Appointment.FindAsync(appointId);
                    if(data.ReportFile  != null && data.ReportFile != "")
                    {
                        return await reportRepository.GetReportData(data.ReportFile);
                    }
                    else
                    {
                        return new Dictionary<string, string> { { "message", "No report file available." } };         
                    }
                }
                throw new Exception("Id not Found");

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get text file: {ex.Message}");
            }
        }
    }
}
