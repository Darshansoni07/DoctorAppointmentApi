using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Models
{
    public class DoctorMapAssistant
    {
        [Key]
        public int DoctorAssistantMapId { get; set; }
        public bool IsAssistantApprove { get; set; }        
        public string Email { get; set; }
        public int AssistantAssistaId { get; set; }
        public int DoctorMetadataDoc_meta_Id {  get; set; }
        public DoctorMetadata DoctorMetadata { get; set; }
        public Assistant Assistant { get; set; }
    }
}
