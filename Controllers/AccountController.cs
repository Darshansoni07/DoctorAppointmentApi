using AutoMapper;
using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DoctorAppointment.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;
        private readonly IMapper _mapper;

        public AccountController(IUnitOfWork uow, IConfiguration configuration, IMapper mapper)
        {
            this.uow = uow;
            this.configuration = configuration;
            _mapper = mapper;
        }


        //-----------------Login 
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequDto loginReqDto)
        {
            var user = await uow.AccountRepository.Authenticate(loginReqDto.Email, loginReqDto.Password);
            if (user == null)
            {
                return Ok("Your email is not Verify or email id and passward wrong");
            }
            return Ok(user);
            
        }



         //Register User Doctor, Assitent, Client 
        [HttpPost("Register")]
        public async  Task<IActionResult> Register(RegisterDto registerDto)
        {
            
            if (await uow.AccountRepository.UserAlreadyExists(registerDto.Email))
            {
                return Ok("This Email of User Already Exsist");
            }
            if (registerDto.roles_UserRole_Id == 1)
            {
                return Ok("You do not have access to create for admin account");
            }
            else 
            {
                try
                {
                    var response = uow.AccountRepository.User_Register(registerDto);
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

            //return Ok(registerDto);
        }


        //--------------------------------Editing in this code is continous 
        [HttpGet("verify")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false, 
                    ValidateAudience = false, 
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    //Accessing claims from the validated token
                    var nameClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
                    var result = await uow.AccountRepository.VerifyEmailUpdate(nameClaim);
                    return Ok("Email verified successfully. You can now login.");
                }
                else
                {
                    return BadRequest("Invalid or expired token.");
                }
                //return Ok("Email verified successfully. You can now login.");
            }
            catch (Exception)
            {
                return BadRequest("Invalid or expired token."); // Token validation failed
            }
        }


        //-------------------------------------- Client Update 
        [HttpPut("userUpdate")]
        public async Task<IActionResult> UpdateUserProfile(ClientDetailDto client)
        {
            try
            {
                string base64ImageData = client.profile_Img;

                string base64String = base64ImageData.Split(',')[1];
                byte[] bytes = Convert.FromBase64String(base64String);

                string directoryPath = @"D:\CISProject\DoctorAppointment Front\DoctorAppointment\src\assets\images\avatar";
                string fileName = $"image_{DateTime.Now.Ticks}.png";
                client.profile_Img = @"assets/images/avatar/" + fileName;

                string filePath = Path.Combine(directoryPath, fileName);
                System.IO.File.WriteAllBytes(filePath, bytes);
                var value = await uow.AccountRepository.UpdateUserDetails(client);
                return Ok(value);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


    }
}
