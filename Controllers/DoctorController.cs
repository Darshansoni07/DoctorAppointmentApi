using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Controllers
{
    //[Authorize(Roles ="Doctor")]
    public class DoctorController : BaseController
    {
        private readonly IUnitOfWork uow;
        public DoctorController(IUnitOfWork uow) 
        {
            this.uow = uow;
        }

        //-------------------------Doctor Checking Approve or not 
        [HttpGet("checkDoctor/{userId}")]
        public async Task<IActionResult> CheckDoctorAvail(int userId)
        {
            var value = await uow.DoctorRepository.CheckDoctorFilled(userId);
            if(value == null)
            {
                return Ok("Wrong data");
            }
            else
            {
                if(value == "doctor Not Found")
                {
                    return Ok("not Found");
                }
                else if(value == "doctor found and approved")
                {
                    return Ok("approved");
                }
                else if (value == "doctor found but not approved")
                {
                    return Ok("doctor found but not approved");
                }
            }
            return Ok("Somting went wrong");
        }

        
    
        //------------------------------Doctor register 
        [HttpPost("doctorRegister")]
        public async Task<IActionResult> DoctorRegisterDetails(DoctorDetailDto doctorDetailDto)
        {
            if (doctorDetailDto != null)
            {
                var value = await uow.DoctorRepository.DoctorfCreateAsync(doctorDetailDto);
                if (value != null)
                {
                    return Ok("SuccessFully send to doctor Request");
                }
                return Ok("User Can Not Register Multiple Time");
            }
            else
            {
                return BadRequest("Null Data is Getting");
            }
        }


        /// <summary>
        ///  getting doctor by id  
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("getDoctorMetaDetail/{userId}")]
        public async Task<IActionResult> GetDoctorMetaData(int userId)
        {
            var metadata = await uow.DoctorRepository.DoctorGetMetaDataAsync(userId);
            if (metadata != null)
            {
                return Ok(metadata);
            }
            else
            {
                return Ok("Doctor is not filled form");
            }
        }


        [HttpGet("doctorGetAllOwnAssistantById/{metaId}")]
        public async Task<IActionResult> DoctorGetAllOwnAssistantByMetaId(int metaId, int pageIndex=0, int pageSize=10)
        {
            try
            {
                var data = await uow.AssistantRepository.DoctorGetAllOwnAssistant(metaId, pageIndex, pageSize);
                var response = new 
                {
                    data = data.Item1,
                    totalAssistant = data.Item2
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
