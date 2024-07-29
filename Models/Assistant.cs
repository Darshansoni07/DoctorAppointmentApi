using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointment.Models
{
    public class Assistant
    {
        [Key]
        public int AssistaId { get; set; }
        [Required]
        public string First_Name { get; set; } = string.Empty;
        [Required]
        public string Last_Name { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? profile_Img { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int roles_UserRole_Id { get; set; }
        public int MetadataDoc_meta_Id { get; set; }
        public DoctorMetadata Metadata { get; set; }
        public Roles_user roles_User { get; set; }
    }
}
