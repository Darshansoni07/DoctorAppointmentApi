using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.Controllers
{    
    public class AssistantController : BaseController
    {
        private readonly IUnitOfWork uow;
        public AssistantController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpPost("AssistantRegister")]
        public async Task<IActionResult> Register(AssistantRegisterDto registerDto)
        {

            if (await uow.AccountRepository.UserAlreadyExists(registerDto.Email))
            {
                return Ok("This Email is Already Exsist");
            }
            else
            {
                try
                {
                    var response = uow.AssistantRepository.User_Register(registerDto);
                    await uow.SaveAsync();
                    if (response != null)
                    {
                        return Ok("SuccessFull Register");
                    }
                    else
                    {
                        return Ok("null");
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("assistantmapdetails/{Email}")]
        public async Task<IActionResult> GetAssistantDetailById(string Email)
        {
            try
            {
                var response = await uow.AssistantRepository.AssistantMapDetails(Email);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return Ok("No value found");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("assistantgetdoctor/{metaId}")]
        public async Task<IActionResult> GetDoctorDetailById(int metaId)
        {
            try
            {
                var response = await uow.AssistantRepository.DoctorDetailsByAssistantId(metaId);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return Ok("No value found");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
