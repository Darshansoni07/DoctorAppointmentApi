using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class Appointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DS_Appointment",
                columns: table => new
                {
                    Appointment_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SlotId = table.Column<int>(type: "int", nullable: false),
                    UserDetailsUser_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Appointment", x => x.Appointment_Id);
                    table.ForeignKey(
                        name: "FK_DS_Appointment_DS_slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "DS_slots",
                        principalColumn: "SlotId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DS_Appointment_DS_User_Account_UserDetailsUser_Id",
                        column: x => x.UserDetailsUser_Id,
                        principalTable: "DS_User_Account",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DS_Appointment_SlotId",
                table: "DS_Appointment",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Appointment_UserDetailsUser_Id",
                table: "DS_Appointment",
                column: "UserDetailsUser_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DS_Appointment");
        }
    }
}
