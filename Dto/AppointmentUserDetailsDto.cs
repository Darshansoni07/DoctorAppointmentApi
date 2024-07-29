using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Dto
{
    public class AppointmentUserDetailsDto
    {
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int Appointment_Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ApproveStatus { get; set; } = string.Empty;
        public DateTime? AppointmentTime { get; set; }
        public string? ReportFile { get; set; } = string.Empty;
        public int SlotId { get; set; }
        public int UserDetailsUser_Id { get; set; }
        public int DoctorMetadataDoc_meta_Id { get; set; }
    }
}