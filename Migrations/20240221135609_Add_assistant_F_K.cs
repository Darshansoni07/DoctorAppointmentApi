using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class Add_assistant_F_K : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MetadataDoc_meta_Id",
                table: "DS_Assistant_Invitation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "roles_UserRole_Id",
                table: "DS_Assistant_Invitation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Invitation_MetadataDoc_meta_Id",
                table: "DS_Assistant_Invitation",
                column: "MetadataDoc_meta_Id");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Invitation_roles_UserRole_Id",
                table: "DS_Assistant_Invitation",
                column: "roles_UserRole_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Assistant_Invitation_DS_Doctor_Metadata_MetadataDoc_meta_Id",
                table: "DS_Assistant_Invitation",
                column: "MetadataDoc_meta_Id",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Assistant_Invitation_DS_User_Role_roles_UserRole_Id",
                table: "DS_Assistant_Invitation",
                column: "roles_UserRole_Id",
                principalTable: "DS_User_Role",
                principalColumn: "Role_Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Assistant_Invitation_DS_Doctor_Metadata_MetadataDoc_meta_Id",
                table: "DS_Assistant_Invitation");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Assistant_Invitation_DS_User_Role_roles_UserRole_Id",
                table: "DS_Assistant_Invitation");

            migrationBuilder.DropIndex(
                name: "IX_DS_Assistant_Invitation_MetadataDoc_meta_Id",
                table: "DS_Assistant_Invitation");

            migrationBuilder.DropIndex(
                name: "IX_DS_Assistant_Invitation_roles_UserRole_Id",
                table: "DS_Assistant_Invitation");

            migrationBuilder.DropColumn(
                name: "MetadataDoc_meta_Id",
                table: "DS_Assistant_Invitation");

            migrationBuilder.DropColumn(
                name: "roles_UserRole_Id",
                table: "DS_Assistant_Invitation");
        }
    }
}
