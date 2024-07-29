using DoctorAppointment.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointment.Dbcontext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Roles_user> DS_User_Role {  get; set; }
        public DbSet<User_details> DS_User_Account { get; set; }
        public DbSet<DoctorMetadata> DS_Doctor_Metadata { get; set; }
        public DbSet<Assistant> DS_Assistant_Invitation { get; set; }
        public DbSet<DoctorMapAssistant> DS_Doctor_Map_Assistant { get; set; }
        public DbSet<Slot> DS_slots {  get; set; }
        public DbSet<Appointment> DS_Appointment { get; set; }
        
    }
}
