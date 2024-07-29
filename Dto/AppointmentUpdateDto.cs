namespace DoctorAppointment.Dto
{
    public class AppointmentUpdateDto
    {
        public string Status { get; set; } = string.Empty;
        public DateTime? AppointmentTime { get; set; }   
        public string? MedicalHistorDescription { get; set; }= string.Empty;

        public string? First_Name { get; set; } = string.Empty;
        public string? Last_Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Doctor_Name { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Gender { get; set; } = string.Empty;
        public DateTime? DOB { get; set; }
        public string? Address { get; set; } = string.Empty;
        public string? BP { get; set; } = string.Empty;
        public string? HeartRate { get; set; } = string.Empty;
        public string? Sugar { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Medicine { get; set; } = string.Empty;

    }
}
