using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class doctor_map_assistant_new_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "DS_Doctor_Map_Assistant",
                columns: table => new
                {
                    DoctorAssistantMapId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAssistantApprove = table.Column<bool>(type: "bit", nullable: false),
                    DoctorMetadataDoc_meta_Id = table.Column<int>(type: "int", nullable: false),
                    AssistantAssistaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Doctor_Map_Assistant", x => x.DoctorAssistantMapId);
                    table.ForeignKey(
                        name: "FK_DS_Doctor_Map_Assistant_DS_Assistant_Invitation_AssistantAssistaId",
                        column: x => x.AssistantAssistaId,
                        principalTable: "DS_Assistant_Invitation",
                        principalColumn: "AssistaId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DS_Doctor_Map_Assistant_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                        column: x => x.DoctorMetadataDoc_meta_Id,
                        principalTable: "DS_Doctor_Metadata",
                        principalColumn: "Doc_meta_Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DS_Doctor_Map_Assistant_AssistantAssistaId",
                table: "DS_Doctor_Map_Assistant",
                column: "AssistantAssistaId");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Doctor_Map_Assistant_DoctorMetadataDoc_meta_Id",
                table: "DS_Doctor_Map_Assistant",
                column: "DoctorMetadataDoc_meta_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DS_Doctor_Map_Assistant");

            migrationBuilder.CreateTable(
                name: "DS_Assistant_Map",
                columns: table => new
                {
                    DoctorAssistantMapId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Assistant_Map", x => x.DoctorAssistantMapId);
                });
        }
    }
}
