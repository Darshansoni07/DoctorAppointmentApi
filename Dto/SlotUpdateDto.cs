namespace DoctorAppointment.Dto
{
    public class SlotUpdateDto
    {
        public DateTime StartTimeslot { get; set; }
        public DateTime EndTimeslot { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; }
    }
}
