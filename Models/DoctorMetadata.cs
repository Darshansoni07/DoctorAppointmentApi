using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointment.Models
{
    public class DoctorMetadata
    {
        [Key]
        public int Doc_meta_Id { get; set; }
        [Required]
        public string Specialist { get; set; } = string.Empty;
        public string License { get; set; } = string.Empty;
        public int FeesAmount { get; set; }
        public bool RequestDoctorApprove { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int UserDetailsUser_Id { get; set; }
        public User_details UserDetails { get; set; }
    }
}
