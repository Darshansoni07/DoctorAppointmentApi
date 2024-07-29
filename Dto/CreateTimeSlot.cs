namespace DoctorAppointment.Dto
{
    public class CreateTimeSlot
    {
        public DateTime StartTimeslot { get; set; }
        public DateTime EndTimeslot { get; set; }
        public string Status { get; set; } = string.Empty;
        public TimeSpan interval { get; set; }
        public int MetadataDoc_meta_Id { get; set; }
    }
}
