using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Dto
{
    public class DoctorAssistantMapRegisterDto
    {
        public int AssistantAssistaId { get; set; }
        public int DoctorMetadataDoc_meta_Id { get; set; }
        public bool IsAssistantApprove { get; set; }     
        public string Email { get; set; }
    }
}
