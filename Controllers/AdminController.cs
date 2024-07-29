using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Reflection.Metadata.Ecma335;

namespace DoctorAppointment.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private IUnitOfWork uow;
        private IConfiguration configuration;

        public AdminController(IUnitOfWork uow, IConfiguration configuration) {
        
            this.uow = uow;
            this.configuration = configuration;
        }

        /// <summary>
        /// Getting Details of doctor which is not approve by admin 
        /// </summary>
        /// <returns>Doctor details with metadata</returns>
        
        [HttpGet("doctorApproveRequest")]
        public async Task<IActionResult> AdminAccessRequest(int pageIndex, int pageSize)
        {
            var details = await uow.AdminRepository.GetDoctorWithRequest(pageIndex, pageSize);
            var response = new { 
                data = details.Item1,
                totalData = details.Item2
            };

            return Ok(response);
        }

        //---------------------------- get all doctor details which is approved 
        [AllowAnonymous]
        [HttpGet("getAllDoctorDetail")]
        public async Task<IActionResult> AdminGetAllDoctorApproved(int pageIndex = 0, int pageSize = 10, string? filter = null)
        {
            try
            {
                var adminDetails = await uow.AdminRepository.GetApprovedDoctorDetails(pageIndex, pageSize, filter);
                var response = new {
                data = adminDetails.Item1,
                totalData = adminDetails.Item2
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //---------------------------- get all doctor details which is approved 
        [AllowAnonymous]
        [HttpGet("getAllDoctorDetails")]
        public async Task<IActionResult> AdminGetAllDoctorsApproved()
        {
            try
            {
                var adminDetails = await uow.AdminRepository.GetApprovedDoctorDetails();
                return Ok(adminDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //----------------------------Approve Doctor API
        [HttpPost("approveDoctorById/{doctorId}")]
        public async Task<IActionResult> AdminApproveByDoctorId(int doctorId)
        {
            var approve = await uow.AdminRepository.DoctorApprove(doctorId);
            if (approve)
            {
                return Ok("Approved");
            }
            return Ok("Not Found");
        }

        [AllowAnonymous]
        [HttpGet("getDocById/{userId}")]
        public async Task<IActionResult> GetAllDoctorDetailById(int userId)
        {
            var docDetail = await uow.AdminRepository.GetDoctorById(userId);
            return Ok(docDetail);
        }

        [HttpGet("getAllClient")]
        public async Task<IActionResult> GetAllClientDetails(int pageIndex=0, int pageSize=10)
        {
            var clientDetils = await uow.AdminRepository.GetClientDetials(pageIndex,pageSize);
            var response = new
            {
                Data = clientDetils.Item1,
                TotalCount = clientDetils.Item2
            };
            if (response == null)
                return Ok("Empty");
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPut("approveAssistantById/{assistantId}")]
        public async Task<IActionResult> AssistantApproveByAdmin(int assistantId)
        {
            var approve = await uow.AdminRepository.AssistantApprove(assistantId);
            if(approve)
            {
                return Ok("Approved");
            }
            return Ok("Not Found");
        }

        [AllowAnonymous]
        [HttpGet("getClientDetailsById/{clientId}")]
        public async Task<IActionResult> GetClientDetailsById(int clientId)
        {
            if (clientId == 0)
            {
                return Ok("Id Not Entered");
            }
            else
            {
                var clientDetails = await uow.AdminRepository.GetUserDetailsById(clientId);
                return Ok(clientDetails);
            }
        }

        [AllowAnonymous]
        [HttpPut("updateDetailsClient/{clientId}")]
        public async Task<IActionResult> UpdateClientDetailsById(int clientId, ClientDetailDto detailDto)
        {
            try
            {
                if(clientId == 0)
                {
                    return Ok("Id Not Entered");
                }
                else
                {
                    var clientDetails = await uow.AdminRepository.UpdateDetailsClient(clientId, detailDto);
                    return Ok("ClientUpdate");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Ok(ex.ToString());
            }
            
        }

        [AllowAnonymous]
        [HttpGet("getAllAssistantWithRequest")]
        public async Task<IActionResult> GetAllAssistantWithReq()
        {
            var assistantDetail = await uow.AdminRepository.GetAssistantWithRequest();
            if (assistantDetail == null)
                return Ok("Empty");
            return Ok(assistantDetail);
        }


        [AllowAnonymous]
        [HttpGet("getAllApproveAssistant")]
        public async Task<IActionResult> AdminGetAllAssitantApproved(int pageIndex, int pageSize)
        {
            try
            {
                var adminDetails = await uow.AdminRepository.GetApprovedAssistantDetails(pageIndex, pageSize);
                var response = new
                {
                    Data = adminDetails.Item1,
                    TotalCount = adminDetails.Item2
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpGet("adminGetAppointmentDetails")]
        public async Task<IActionResult> AdimGetAllAppointment(int pageIndex = 0, int pageSize = 5)
        {
            try
            {
                var data = await uow.AppointmentRepository.AdminGetAllBookedAppoinments(pageIndex, pageSize);
                var response = new
                {
                    data = data.Item1,
                    count = data.Item2,
                    totalIncome = data.Item3

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
