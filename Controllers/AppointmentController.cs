using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IUnitOfWork uow;
        public AppointmentController(IUnitOfWork uow) 
        {
            this.uow = uow;
        }

        [HttpGet("getAllAppointDetails/{metaId}")]
        public async Task<IActionResult> GetAllAppointmentUserDetail(int metaId)
        {
            try
            {
                var value = await uow.AppointmentRepository.GetAllAppointments(metaId);
                if (value == null)
                {
                    return Ok("No Data Available");
                }
                else
                {
                    return Ok(value);
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost("appointmentCreate")]
        public async Task<IActionResult> AppointmentCreate(AppointmentCreateDto appointment)
       {
            try
            {
                if(appointment == null)
                {
                    return Ok("Requried Data");
                }
                else
                {
                    var value = await uow.AppointmentRepository.CreateAsync(appointment);
                    if (value == null)
                    {
                        return Ok("Data Not Filled");
                    }
                    return Ok(value.ToString());
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            
        }


        [HttpPost("appointmentApproveByDoc/{apointId}")]
        public async Task<IActionResult> AppointmentApproveStatus(int apointId)
        {
            try
            {
                if(apointId == 0)
                {
                    return Ok("Please Provide Id");
                }
                else
                {
                    var value = await uow.AppointmentRepository.AppointmentApprove(apointId);
                    if(value == null)
                    {
                        return Ok("Data not filled");
                    }
                    else
                    {
                        return Ok(value);
                    }
                }
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }            
        }


        [HttpGet("appointmentDetailById/{appointId}")]
        public async Task<IActionResult> AppointmentDetailById(int appointId)
        {
            try
            {
                var value = await uow.AppointmentRepository.GetAppointmentDetailsById(appointId);
                if (value == null)
                {
                    return Ok("No Data Available");
                }
                else
                {
                    return Ok(value);
                }
            }
            catch(Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpGet("reportAppointById/{appointId}")]
        public async Task<IActionResult> GetReportByAppointId(int appointId)
        {
            
                var value = await uow.AppointmentRepository.GetAppointmentReportById(appointId);
                if (value == null)
                {
                    return Ok("No Data Available");
                }
                else
                {
                    return Ok(value);
                }
            
        }



        [HttpGet("appointmentGetAll/{metaId}")]
        public async Task<IActionResult> GetAllAppointment(int metaId, int pageIndex=0, int pageSize = 5)
        {
            try
            {
                var data = await uow.AppointmentRepository.GetAllAppointmentWithRequest(metaId, pageIndex, pageSize);
                var response = new
                {
                    data = data.Item1,
                    totalData = data.Item2
                };
                
                    return Ok(response);
                
            }
            catch( Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpPut("appointUpdate/{appointId}")]
        public async Task<IActionResult> UpdateAppointment(int appointId, AppointmentUpdateDto updateDto)
        {
            try
            {
                var data = await uow.AppointmentRepository.UpdateAppointmentDetail(appointId, updateDto);
                if(data == null)
                {
                    return Ok("Data not available");
                }
                else
                {
                    return Ok(data);
                }
            }
            catch(Exception e)
            {
                return Ok(e.Message);
            }
        }


        [HttpGet("getAllApprovedAppointment/{metaId}")]
        public async Task<IActionResult> GetAllApprovedAppointment(int metaId, int pageIndex = 0, int pageSize = 5)
        {
            try
            {
                var data = await uow.AppointmentRepository.GetAllApprovedAppointments(metaId, pageIndex, pageSize);
                var respose = new { 
                data = data.Item1,
                totaldata = data.Item2
                };

                return Ok(respose);                
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpGet("getAllTodayAppointment/{metaId}")]
        public async Task<IActionResult> GetAllTodayAppointment(int metaId)
        {
            try
            {
                var data = await uow.AppointmentRepository.GetAllTodayAppointments(metaId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("getAllPerviousAppointment/{metaId}")]
        public async Task<IActionResult> GetAllPerviousAppointment(int metaId)
        {
            
                try
                {
                    var data = await uow.AppointmentRepository.GetAllAppointmentPervious(metaId);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
            

        }

        /// <summary>
        /// This Api is get only selected date data of particular date 
        /// </summary>
        /// <param name="metaId"></param>
        /// <returns> It is returning Appointment details </returns>
        [HttpGet("getAllDateSelectedAppointment/{metaId}")]
        public async Task<IActionResult> GetAllDateSelectedAppointments(int metaId, DateTime date, int pageIndex = 0)
        {
            try
            {
                int pageSize = 5;
                var data = await uow.AppointmentRepository.GetAllSelectedDateAppointment(metaId, date, pageIndex, pageSize);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpGet("getAllApproveAppointments/{metaId}")]
        public async Task<IActionResult> GetAllApproveAppointments(int metaId, int pageIndex = 0,int pageSize=5)
        {
            try
            {
                var data = await uow.AppointmentRepository.GetAllApproveAppointment(metaId, pageIndex, pageSize);
                var respose = new {
                data = data.Item1,
                totalAppointment = data.Item2,
                feesAmount = data.Item3,
                totalData = data.Item4
                };

                return Ok(respose);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpGet("getUserBooking/{userId}")]
        public async Task<IActionResult> GetAllBooking(int userId, int pageIndex = 0, int pageSize = 5)
        {
            try
            {
                var data = await uow.AppointmentRepository.GetAllDetailOfBookingUser(userId, pageIndex, pageSize);
                var response = new { 
                
                    data = data.Item1,
                    totalBooking = data.Item2
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpGet("getPaitentUpcomingAppointment/{userId}")]
        public async Task<IActionResult> GetPaitentUpcomingAppointment(int userId, int pageIndex = 0, int pageSize = 5)
        {
            try
            {
                var data = await uow.AppointmentRepository.PaitentGetAllAppointmentDetails(userId, pageIndex, pageSize);
                var response = new
                {

                    data = data.Item1,
                    totalData = data.Item2,
                    totalBooked = data.Item3,
                    totalChecked = data.Item4,
                    totalPending = data.Item5,
                    totalUpcomingData = data.Item6,
                    totalCancel = data.Item7
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

    }
}
