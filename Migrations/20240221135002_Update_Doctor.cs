using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class Update_Doctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Assistant_Invitation_DS_Doctor_Metadata_DoctorId",
                table: "DS_Assistant_Invitation");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Assistant_Invitation_DS_User_Role_RoleId",
                table: "DS_Assistant_Invitation");

            migrationBuilder.DropIndex(
                name: "IX_DS_Assistant_Invitation_DoctorId",
                table: "DS_Assistant_Invitation");

            migrationBuilder.DropIndex(
                name: "IX_DS_Assistant_Invitation_RoleId",
                table: "DS_Assistant_Invitation");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "DS_Assistant_Invitation");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "DS_Assistant_Invitation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "DS_Assistant_Invitation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "DS_Assistant_Invitation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Invitation_DoctorId",
                table: "DS_Assistant_Invitation",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Invitation_RoleId",
                table: "DS_Assistant_Invitation",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Assistant_Invitation_DS_Doctor_Metadata_DoctorId",
                table: "DS_Assistant_Invitation",
                column: "DoctorId",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Assistant_Invitation_DS_User_Role_RoleId",
                table: "DS_Assistant_Invitation",
                column: "RoleId",
                principalTable: "DS_User_Role",
                principalColumn: "Role_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
