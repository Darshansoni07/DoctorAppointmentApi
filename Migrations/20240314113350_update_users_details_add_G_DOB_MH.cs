using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class update_users_details_add_G_DOB_MH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "DS_User_Account",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "DS_User_Account",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalHistorDescription",
                table: "DS_User_Account",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOB",
                table: "DS_User_Account");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "DS_User_Account");

            migrationBuilder.DropColumn(
                name: "MedicalHistorDescription",
                table: "DS_User_Account");
        }
    }
}
