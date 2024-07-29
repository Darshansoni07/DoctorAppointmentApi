using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class update_F_K : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Doctor_Metadata_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_Doctor_Metadata");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_User_Role_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_User_Role");

            migrationBuilder.DropIndex(
                name: "IX_DS_User_Role_AssistantAssistaId",
                table: "DS_User_Role");

            migrationBuilder.DropIndex(
                name: "IX_DS_Doctor_Metadata_AssistantAssistaId",
                table: "DS_Doctor_Metadata");

            migrationBuilder.DropColumn(
                name: "AssistantAssistaId",
                table: "DS_User_Role");

            migrationBuilder.DropColumn(
                name: "AssistantAssistaId",
                table: "DS_Doctor_Metadata");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssistantAssistaId",
                table: "DS_User_Role",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssistantAssistaId",
                table: "DS_Doctor_Metadata",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DS_User_Role_AssistantAssistaId",
                table: "DS_User_Role",
                column: "AssistantAssistaId");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Doctor_Metadata_AssistantAssistaId",
                table: "DS_Doctor_Metadata",
                column: "AssistantAssistaId");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Doctor_Metadata_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_Doctor_Metadata",
                column: "AssistantAssistaId",
                principalTable: "DS_Assistant_Invitation",
                principalColumn: "AssistaId");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_User_Role_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_User_Role",
                column: "AssistantAssistaId",
                principalTable: "DS_Assistant_Invitation",
                principalColumn: "AssistaId");
        }
    }
}
