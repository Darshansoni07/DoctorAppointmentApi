namespace DoctorAppointment.Models
{
    public class PatientMetadata
    {
        public int Patient_Id { get; set; }
        public string DiseaseName { get; set; } = string.Empty;
        public string DiseaseDescription { get; set; } = string.Empty;
        public string DiseaseType { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
