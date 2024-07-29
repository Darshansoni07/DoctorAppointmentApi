﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    public partial class doctorassistantmap_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DS_Assistant_Map_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_Assistant_Map");

            migrationBuilder.DropForeignKey(
                name: "FK_DS_Assistant_Map_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map");

            migrationBuilder.DropIndex(
                name: "IX_DS_Assistant_Map_AssistantAssistaId",
                table: "DS_Assistant_Map");

            migrationBuilder.DropIndex(
                name: "IX_DS_Assistant_Map_DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map");

            migrationBuilder.DropColumn(
                name: "AssistantAssistaId",
                table: "DS_Assistant_Map");

            migrationBuilder.DropColumn(
                name: "DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map");

            migrationBuilder.DropColumn(
                name: "IsAssistantApprove",
                table: "DS_Assistant_Map");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssistantAssistaId",
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

            migrationBuilder.AddColumn<bool>(
                name: "IsAssistantApprove",
                table: "DS_Assistant_Map",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Map_AssistantAssistaId",
                table: "DS_Assistant_Map",
                column: "AssistantAssistaId");

            migrationBuilder.CreateIndex(
                name: "IX_DS_Assistant_Map_DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map",
                column: "DoctorMetadataDoc_meta_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Assistant_Map_DS_Assistant_Invitation_AssistantAssistaId",
                table: "DS_Assistant_Map",
                column: "AssistantAssistaId",
                principalTable: "DS_Assistant_Invitation",
                principalColumn: "AssistaId");

            migrationBuilder.AddForeignKey(
                name: "FK_DS_Assistant_Map_DS_Doctor_Metadata_DoctorMetadataDoc_meta_Id",
                table: "DS_Assistant_Map",
                column: "DoctorMetadataDoc_meta_Id",
                principalTable: "DS_Doctor_Metadata",
                principalColumn: "Doc_meta_Id");
        }
    }
}
