using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Dto
{
    public class ClientDetailDto
    {
        public int User_Id { get; set; }
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? profile_Img { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Gender { get; set; } = string.Empty;
        public DateTime? DOB { get; set; }
        public string? MedicalHistorDescription { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? roles_UserRole_Id { get; set; }
    }
}
