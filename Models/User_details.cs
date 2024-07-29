using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Models
{
    public class User_details
    {
        [Key]
        public int User_Id { get; set; }
        [Required]
        public string First_Name { get; set; } = string.Empty;
        [Required]
        public string Last_Name { get; set; } = string.Empty;
        [Required]
        public byte[] Password { get; set; }
        public byte[] PasswordKey { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? profile_Img { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Gender {  get; set; } = string.Empty;
        public DateTime? DOB { get; set; }
        public string? MedicalHistorDescription { get; set; } = string.Empty;
        public string? Address { get;set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int roles_UserRole_Id { get; set; }
        public Roles_user roles_User { get; set; }
    }
}
