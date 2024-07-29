namespace DoctorAppointment.Dto
{
    public class AppointmentDetailDto
    {
        public int Appointment_Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ApproveStatus { get; set; } = string.Empty;               
        public DateTime? AppointmentTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? ReportFile { get; set; } =string.Empty;
        public int DoctorMetadataDoc_meta_ID {  get; set; }
        public int SlotId { get; set; }
        public int UserDetailsUser_Id { get; set; }
    }
}
