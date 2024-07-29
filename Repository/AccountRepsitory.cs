using AutoMapper;
using DoctorAppointment.Dbcontext;
using DoctorAppointment.Dto;
using DoctorAppointment.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DoctorAppointment.Models;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace DoctorAppointment.Repository
{
    public class AccountRepsitory : IAccountRepository
    {
        private readonly AppDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IMapper _mapper;
        private readonly TimeZoneInfo IndianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public AccountRepsitory(AppDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;

        }

        //------------------Register Account 
        public async Task<RegisterDto> User_Register(RegisterDto registerDto)
        {
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);

            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password));

            }
            try
            {
                User_details user_Details = new User_details();
                user_Details.Email = registerDto.Email;
                user_Details.First_Name = registerDto.First_Name;
                user_Details.Last_Name = registerDto.Last_Name;
                user_Details.Password = passwordHash;
                user_Details.PasswordKey = passwordKey;
                user_Details.profile_Img = @"assets/images/avatar/01.jpg";
                user_Details.PhoneNumber = registerDto.PhoneNumber;
                user_Details.CreatedOn = currentTime;
                user_Details.UpdatedOn = null;//update in this line
                user_Details.MedicalHistorDescription = null;
                user_Details.Address = null;
                user_Details.roles_UserRole_Id = registerDto.roles_UserRole_Id;
                dbContext.DS_User_Account.Add(user_Details);
                dbContext.SaveChanges();

                //Genrate Token For sending mail
                var Token = EmailCreateJWT(registerDto.Email);

                string subject = "Account Activation";
                string body = "Your account has been successfully registered. Click the link below to activate your account: ";
                bool emailSent = SendVerifyEmail(registerDto.Email, subject, body, Token);
                if (emailSent)
                {
                    Console.WriteLine("Token is send successfully");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            await dbContext.SaveChangesAsync();

            return registerDto;
            //return user_details;

        }

        // Check while user Resiter with same email or not 
        public async Task<bool> UserAlreadyExists(string user_Email)
        {
            return await dbContext.DS_User_Account.AnyAsync(s => s.Email == user_Email);
        }


        //Authenticate User for Login ------------- Login -----------
        public async Task<LoginRespoDto> Authenticate(string email, string password)
        {
            var user = await dbContext.DS_User_Account.FirstOrDefaultAsync(s => s.Email == email);

            if (user == null || user.PasswordKey == null)
                return null;
            if (!MatchPasswordHash(password, user.Password, user.PasswordKey))
                return null;

            var loginRes = new LoginRespoDto();
            loginRes.Email = user.Email;
            //loginRes.Token = CreateJWT(user);
            if (user.IsEmailVerified == false)
            {
                var Token = EmailCreateJWT(user.Email);
                string subject = "Account Activation";
                string body = "Your account has been successfully registered. Click the link below to activate your account: ";
                bool emailSent = SendVerifyEmail(user.Email, subject, body, Token);
                if (emailSent)
                {
                    Console.WriteLine("Token is send successfully");
                    return null;
                }
                else
                {
                    return null;
                }
            }
            loginRes.Token = CreateJWT(user);
            return loginRes;
        }

        //Match Password
        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using (var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));

                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != password[i]) return false;

                }
                return true;
            }
        }

        //Email Verify Update and true IsEmailVerified 
        public async Task<bool> VerifyEmailUpdate(string email)
        {
            var user = await dbContext.DS_User_Account.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                user.IsEmailVerified = true;

                await dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }


        //Sending Verifcation Mail
        public bool SendVerifyEmail(string user_Email, string subject, string body, string token)
        {
            try
            {
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "mailsenderboxa@gmail.com"; // Your sender email address
                string smtpPassword = "iwfa oerp mjvi gojn"; // Your sender email password or app-specific password
                string baseurl = $"https://localhost:7133/Account/verify?token={token}";
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    body += $"<a href='{baseurl}'>Click here to verify your account</a>";
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

        //---------------------------Create a token for send email to verify user
        public string EmailCreateJWT(string username)
        {
            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            //JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(secretKey));

            var claims = new Claim[] {

                new Claim(ClaimTypes.Name,username)

            };

            var signingCredentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);

            var takenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(1000),
                SigningCredentials = signingCredentials
            };

            //Token Handler this is reponsible to gen JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(takenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        //----------------------------------Token Create After Login 
        public string CreateJWT(User_details user)
        {
            var role = dbContext.DS_User_Role
                     .Where(s => s.Role_Id == user.roles_UserRole_Id)
                     .Select(s => s.Role_Name)
                     .FirstOrDefault();

            var secretKey = configuration.GetSection("AppSettings:Key").Value;
            //JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(secretKey));

            var claims = new Claim[] {

                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,role),
                new Claim(ClaimTypes.Name,user.First_Name,user.Last_Name),
                new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)

            };

            var signingCredentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);

            var takenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(300),
                SigningCredentials = signingCredentials
            };

            //Token Handler this is reponsible to gen JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(takenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> UpdateUserDetails(ClientDetailDto clientDetailDto)
        {
            try
            {
                DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianTimeZone);
                if (clientDetailDto.User_Id != 0)
                {
                    var userData = await dbContext.DS_User_Account.FindAsync(clientDetailDto.User_Id);
                    if (userData != null)
                    {
                        userData.First_Name = clientDetailDto.First_Name;
                        userData.Address = clientDetailDto.Address;
                        userData.Last_Name = clientDetailDto.Last_Name;
                        userData.PhoneNumber = clientDetailDto.PhoneNumber;
                        if (clientDetailDto.MedicalHistorDescription != null)
                        {                            
                            if(userData.MedicalHistorDescription == null)
                            {
                                userData.MedicalHistorDescription = clientDetailDto.MedicalHistorDescription;
                            }
                            else
                            {
                                userData.MedicalHistorDescription += Environment.NewLine + clientDetailDto.MedicalHistorDescription;
                            }

                        }
                        else
                        {
                            clientDetailDto.MedicalHistorDescription = null;
                        }
                        userData.Gender = clientDetailDto.Gender;
                        userData.UpdatedOn = currentTime;
                        userData.DOB = clientDetailDto.DOB;
                        userData.profile_Img = clientDetailDto.profile_Img;
                        await dbContext.SaveChangesAsync();

                        return "User Update";
                    }
                    else
                    {
                        return "User Not Updated";
                    }                    
                }
                else
                {
                    return "Data Not Found";
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> DeleteUserByEmail(string user_Email)
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                
            }
            throw new NotImplementedException();
        }
    }
}
