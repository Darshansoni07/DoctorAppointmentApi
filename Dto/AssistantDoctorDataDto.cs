namespace DoctorAppointment.Dto
{
    public class AssistantDoctorDataDto
    {
        public int Doc_meta_Id { get; set; }
        public string Specialist { get; set; } = string.Empty;
        public int FeesAmount { get; set; }
        public int UserDetailsUser_Id { get; set; }
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
