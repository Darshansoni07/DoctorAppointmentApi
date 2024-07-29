using System.ComponentModel.DataAnnotations;

namespace DoctorAppointment.Dto
{
    public class LoginRequDto
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

    }
}
