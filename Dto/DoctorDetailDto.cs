namespace DoctorAppointment.Dto
{
    public class DoctorDetailDto
    {
        public string Specialist { get; set; } = string.Empty;
        public string License { get; set; } = string.Empty;
        public int FeesAmount { get; set; }
        public int UserDetailsUser_Id { get; set; }
    }
}
