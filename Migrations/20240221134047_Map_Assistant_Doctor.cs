using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class Map_Assistant_Doctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DS_Assistant_Map",
                columns: table => new
                {
                    DoctorAssistantMapId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAssistantApprove = table.Column<bool>(type: "bit", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    AssistantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Assistant_Map", x => x.DoctorAssistantMapId);
                    table.ForeignKey(
                        name: "FK_DS_Assistant_Map_DS_Assistant_Invitation_AssistantId",
                        column: x => x.AssistantId,
                        principalTable: "DS_Assistant_Invitation",
                        principalColumn: "AssistaId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DS_Assistant_Map_DS_Doctor_Metadata_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DS_Doctor_Metadata",
                        principalColumn: "Doc_meta_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Map_AssistantId",
                table: "DS_Assistant_Map",
                column: "AssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Map_DoctorId",
                table: "DS_Assistant_Map",
                column: "DoctorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DS_Assistant_Map");
        }
    }
}
