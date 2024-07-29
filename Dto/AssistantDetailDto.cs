using DoctorAppointment.Models;
using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Dto
{
    public class AssistantDetailDto
    {
        public int AssistaId { get; set; }
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? profile_Img { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int roles_UserRole_Id { get; set; }
        public int MetadataDoc_meta_Id { get; set; }
    }
}
