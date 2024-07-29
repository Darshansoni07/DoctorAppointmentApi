using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class remove_Doctor_access : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequestDoctorAccess",
                table: "DS_User_Account");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequestDoctorAccess",
                table: "DS_User_Account",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
