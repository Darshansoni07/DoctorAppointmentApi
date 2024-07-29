using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class addmetaidappointmenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentTime",
                table: "DS_Appointment",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "DoctorMetadataDoc_meta_Id",
                table: "DS_Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DS_Appointment_DoctorMetadataDoc_meta_Id",
                table: "DS_Appointment",
                column: "DoctorMetadataDoc_meta_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Appointment_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Appointment",
                column: "DoctorMetadataDoc_meta_Id",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Appointment_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Appointment");

            migrationBuilder.DropIndex(
                name: "IX_DS_Appointment_DoctorMetadataDoc_meta_Id",
                table: "DS_Appointment");

            migrationBuilder.DropColumn(
                name: "DoctorMetadataDoc_meta_Id",
                table: "DS_Appointment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentTime",
                table: "DS_Appointment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
