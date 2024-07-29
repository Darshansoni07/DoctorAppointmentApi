using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class Slots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "slots",
                columns: table => new
                {
                    SlotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTimeslot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTimeslot = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MetadataDoc_meta_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_slots", x => x.SlotId);
                    table.ForeignKey(
                        name: "FK_slots_DS_Doctor_Metadata_MetadataDoc_meta_Id",
                        column: x => x.MetadataDoc_meta_Id,
                        principalTable: "DS_Doctor_Metadata",
                        principalColumn: "Doc_meta_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_slots_MetadataDoc_meta_Id",
                table: "slots",
                column: "MetadataDoc_meta_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "slots");
        }
    }
}
