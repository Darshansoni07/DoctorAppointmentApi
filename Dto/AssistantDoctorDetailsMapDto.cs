namespace DoctorAppointment.Dto
{
    public class AssistantDoctorDetailsMapDto
    {
        public int Doc_meta_Id { get; set; }
        public string Specialist { get; set; } = string.Empty;
        public int FeesAmount { get; set; }
        public string Assistant_F_Name { get; set; } = string.Empty;
        public string Assistant_L_Name { get; set; } = string.Empty;
        public string Doc_F_Name { get; set; } = string.Empty;
        public string Doc_L_Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
