using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class SlotName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_slots_DS_Doctor_Metadata_MetadataDoc_meta_Id",
                table: "slots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_slots",
                table: "slots");

            migrationBuilder.RenameTable(
                name: "slots",
                newName: "DS_slots");

            migrationBuilder.RenameIndex(
                name: "IX_slots_MetadataDoc_meta_Id",
                table: "DS_slots",
                newName: "IX_DS_slots_MetadataDoc_meta_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DS_slots",
                table: "DS_slots",
                column: "SlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_slots_DS_Doctor_Metadata_MetadataDoc_meta_Id",
                table: "DS_slots",
                column: "MetadataDoc_meta_Id",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_slots_DS_Doctor_Metadata_MetadataDoc_meta_Id",
                table: "DS_slots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DS_slots",
                table: "DS_slots");

            migrationBuilder.RenameTable(
                name: "DS_slots",
                newName: "slots");

            migrationBuilder.RenameIndex(
                name: "IX_DS_slots_MetadataDoc_meta_Id",
                table: "slots",
                newName: "IX_slots_MetadataDoc_meta_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_slots",
                table: "slots",
                column: "SlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_slots_DS_Doctor_Metadata_MetadataDoc_meta_Id",
                table: "slots",
                column: "MetadataDoc_meta_Id",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
