using DoctorAppointment.Dto;
using DoctorAppointment.Models;

namespace DoctorAppointment.IRepository
{
    public interface IAccountRepository
    {
        Task<LoginRespoDto> Authenticate(string email, string password);
        Task<RegisterDto> User_Register(RegisterDto registerDto);
        Task<string> UpdateUserDetails(ClientDetailDto clientDetailDto);
        Task<bool> UserAlreadyExists(string user_Email);
        Task<bool> VerifyEmailUpdate(string username);
        public bool SendVerifyEmail(string user_Email, string subject, string body, string token);
        public string CreateJWT(User_details user);
        Task<bool> DeleteUserByEmail(string user_Email);
    }
}
