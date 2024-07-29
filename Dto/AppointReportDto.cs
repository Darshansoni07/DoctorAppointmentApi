namespace DoctorAppointment.Dto
{
    public class AppointReportDto
    {
        public int Appointment_Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ApproveStatus { get; set; } = string.Empty;
        public DateTime? AppointmentTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int SlotId { get; set; }
        public int UserDetailsUser_Id { get; set; }
        public string? BP { get; set; } = string.Empty;
        public string? HeartRate { get; set; } = string.Empty;
        public string? Sugar { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Medicine { get; set; } = string.Empty;
    }
}
