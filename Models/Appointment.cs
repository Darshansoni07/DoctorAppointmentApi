using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Models
{
    public class Appointment
    {
        [Key]
        public int Appointment_Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ApproveStatus { get; set; } = string.Empty;
        public string? ReportFile { get; set; } = string.Empty;
        public DateTime? AppointmentTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int SlotId { get; set; }
        public int UserDetailsUser_Id { get; set; }
        public int DoctorMetadataDoc_meta_Id { get; set; }
        public Slot Slot { get; set; }
        public User_details UserDetails { get; set; }
        public DoctorMetadata DoctorMetadata { get; set; }

    }
}
