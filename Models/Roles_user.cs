using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Models
{
    public class Roles_user
    {
        [Key]
        public int Role_Id { get; set; }
        public string Role_Name { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
}
