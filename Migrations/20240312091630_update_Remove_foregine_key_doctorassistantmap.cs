using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class update_Remove_foregine_key_doctorassistantmap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Assistant_Map_DS_Assistant_Invitation_AssistantId",
                table: "DS_Assistant_Map");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Assistant_Map_DS_Doctor_Metadata_DoctorId",
                table: "DS_Assistant_Map");

            migrationBuilder.DropIndex(
                name: "IX_DS_Assistant_Map_AssistantId",
                table: "DS_Assistant_Map");

            migrationBuilder.DropIndex(
                name: "IX_DS_Assistant_Map_DS_Doctor_Metadata_DoctorId",
                table: "DS_Assistant_Map");

            migrationBuilder.DropColumn(
                name: "AssistantId",
                table: "DS_Assistant_Map");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "DS_Assistant_Map");

            /*migrationBuilder.DropColumn(
                name: "DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map");*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssistantId",
                table: "DS_Assistant_Map",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "DS_Assistant_Map",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Map_AssistantId",
                table: "DS_Assistant_Map",
                column: "AssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Map_DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map",
                column: "DoctorMetadataDoc_meta_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Assistant_Map_DS_Assistant_Invitation_AssistantId",
                table: "DS_Assistant_Map",
                column: "AssistantId",
                principalTable: "DS_Assistant_Invitation",
                principalColumn: "AssistaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Assistant_Map_DS_Doctor_Metadata_DoctorId",
                table: "DS_Assistant_Map",
                column: "DoctorMetadataDoc_meta_Id",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
