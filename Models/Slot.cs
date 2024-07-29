using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Models
{
    public class Slot
    {
        [Key]
        public int SlotId { get; set; }
        public DateTime StartTimeslot { get; set; }
        public DateTime EndTimeslot { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int MetadataDoc_meta_Id {  get; set; }
        public DoctorMetadata Metadata { get; set; }

    }
}