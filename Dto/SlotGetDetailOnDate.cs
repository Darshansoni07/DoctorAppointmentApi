namespace DoctorAppointment.Dto
{
    public class SlotGetDetailOnDate
    {
        public int SlotId { get; set; }
        public DateTime? StartTimeslot { get; set; }
        public string? Status { get; set; } = string.Empty;
        public int MetadataDoc_meta_Id { get; set; }
    }
}
