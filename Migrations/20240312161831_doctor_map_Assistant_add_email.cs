using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class doctor_map_Assistant_add_email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_DS_Doctor_Map_Assistant_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_Doctor_Map_Assistant");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Doctor_Map_Assistant_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Doctor_Map_Assistant");
*/
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "DS_Doctor_Map_Assistant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            /*migrationBuilder.AddForeignKey(
                name: "FK_DS_Doctor_Map_Assistant_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_Doctor_Map_Assistant",
                column: "AssistantAssistaId",
                principalTable: "DS_Assistant_Invitation",
                principalColumn: "AssistaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Doctor_Map_Assistant_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Doctor_Map_Assistant",
                column: "DoctorMetadataDoc_meta_Id",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id",
                onDelete: ReferentialAction.Cascade);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_DS_Doctor_Map_Assistant_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_Doctor_Map_Assistant");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Doctor_Map_Assistant_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Doctor_Map_Assistant");
*/
            migrationBuilder.DropColumn(
                name: "Email",
                table: "DS_Doctor_Map_Assistant");

            /*migrationBuilder.AddForeignKey(
                name: "FK_DS_Doctor_Map_Assistant_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_Doctor_Map_Assistant",
                column: "AssistantAssistaId",
                principalTable: "DS_Assistant_Invitation",
                principalColumn: "AssistaId");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Doctor_Map_Assistant_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Doctor_Map_Assistant",
                column: "DoctorMetadataDoc_meta_Id",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id");*/
        }
    }
}
