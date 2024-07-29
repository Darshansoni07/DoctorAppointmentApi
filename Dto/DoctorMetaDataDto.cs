using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Dto
{
    public class DoctorMetaDataDto
    {
        public int Doc_meta_Id { get; set; }
        public string Specialist { get; set; } = string.Empty;
        public string License { get; set; } = string.Empty;
        public int FeesAmount { get; set; }
        public bool RequestDoctorApprove { get; set; }
        public int UserDetailsUser_Id { get; set; }
    }
}
