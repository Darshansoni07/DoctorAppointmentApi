using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class Assistant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "DS_Assistant_Invitation",
                columns: table => new
                {
                    AssistaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    First_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Last_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    profile_Img = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DS_Assistant_Invitation", x => x.AssistaId);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Doctor_Metadata_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_Doctor_Metadata");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_User_Role_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_User_Role");

            migrationBuilder.DropTable(
                name: "DS_Assistant_Invitation");

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
    }
}
