using DoctorAppointment.Models;

namespace DoctorAppointment.Dto
{
    public class AssistantRegisterDto
    {
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string Password { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? profile_Img { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public int MetadataDoc_meta_Id { get; set; }
    }
}
