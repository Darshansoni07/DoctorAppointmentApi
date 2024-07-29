using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Dto
{
    public class DoctorAccessReqDto
    {
        public int Doc_meta_Id { get; set; }
        public string Specialist { get; set; } = string.Empty;
        public string License { get; set; } = string.Empty;
        public int FeesAmount { get; set; }
        public bool RequestDoctorApprove { get; set; }
        public int UserDetailsUser_Id { get; set; }
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? profile_Img { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int? Age { get; set; } 
        
    }
}
