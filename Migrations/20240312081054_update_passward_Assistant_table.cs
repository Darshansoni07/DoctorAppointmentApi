using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class update_passward_Assistant_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_DS_Appointment_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Appointment");*/

            /*migrationBuilder.DropForeignKey(
                name: "FK_DS_Appointment_DS_slots_SlotId",
                table: "DS_Appointment");*/

            /*migrationBuilder.DropForeignKey(
                name: "FK_DS_Appointment_DS_User_Account_UserDetailsUser_Id",
                table: "DS_Appointment");
*/
            /*migrationBuilder.DropForeignKey(
                name: "FK_DS_Assistant_Map_DS_Doctor_Metadata_DoctorId",
                table: "DS_Assistant_Map");*/

            /*migrationBuilder.DropIndex(
                name: "IX_DS_Assistant_Map_DoctorId",
                table: "DS_Assistant_Map");*/

            migrationBuilder.DropColumn(
                name: "PasswordKey",
                table: "DS_Assistant_Invitation");

            /*migrationBuilder.AddColumn<int>(
                name: "DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map",
                type: "int",
                nullable: false,
                defaultValue: 0);*/

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "DS_Assistant_Invitation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            /*migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Map_DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map",
                column: "DoctorMetadataDoc_meta_Id");*/

            /*migrationBuilder.AddForeignKey(
                name: "FK_DS_Appointment_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Appointment",
                column: "DoctorMetadataDoc_meta_Id",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id",
                onDelete: ReferentialAction.Cascade);*/

            /*migrationBuilder.AddForeignKey(
                name: "FK_DS_Appointment_DS_slots_SlotId",
                table: "DS_Appointment",
                column: "SlotId",
                principalTable: "DS_slots",
                principalColumn: "SlotId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Appointment_DS_User_Account_UserDetailsUser_Id",
                table: "DS_Appointment",
                column: "UserDetailsUser_Id",
                principalTable: "DS_User_Account",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Assistant_Map_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map",
                column: "DoctorMetadataDoc_meta_Id",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id",
                onDelete: ReferentialAction.Cascade);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_DS_Appointment_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Appointment_DS_slots_SlotId",
                table: "DS_Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Appointment_DS_User_Account_UserDetailsUser_Id",
                table: "DS_Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Assistant_Map_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map");*/

            /*migrationBuilder.DropIndex(
                name: "IX_DS_Assistant_Map_DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map");

            migrationBuilder.DropColumn(
                name: "DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map");*/

            migrationBuilder.AlterColumn<byte[]>(
                name: "Password",
                table: "DS_Assistant_Invitation",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordKey",
                table: "DS_Assistant_Invitation",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Map_DoctorId",
                table: "DS_Assistant_Map",
                column: "DoctorId");

           /* migrationBuilder.AddForeignKey(
                name: "FK_DS_Appointment_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Appointment",
                column: "DoctorMetadataDoc_meta_Id",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Appointment_DS_slots_SlotId",
                table: "DS_Appointment",
                column: "SlotId",
                principalTable: "DS_slots",
                principalColumn: "SlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Appointment_DS_User_Account_UserDetailsUser_Id",
                table: "DS_Appointment",
                column: "UserDetailsUser_Id",
                principalTable: "DS_User_Account",
                principalColumn: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Assistant_Map_DS_Doctor_Metadata_DoctorId",
                table: "DS_Assistant_Map",
                column: "DoctorId",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id",
                onDelete: ReferentialAction.Cascade);*/
        }
    }
}
