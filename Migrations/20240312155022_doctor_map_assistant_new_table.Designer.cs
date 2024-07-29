﻿// <auto-generated />
using System;
using DoctorAppointment.Dbcontext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DoctorAppointment.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240312155022_doctor_map_assistant_new_table")]
    partial class doctor_map_assistant_new_table
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DoctorAppointment.Models.Appointment", b =>
                {
                    b.Property<int>("Appointment_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Appointment_Id"), 1L, 1);

                    b.Property<DateTime?>("AppointmentTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ApproveStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoctorMetadataDoc_meta_Id")
                        .HasColumnType("int");

                    b.Property<int>("SlotId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserDetailsUser_Id")
                        .HasColumnType("int");

                    b.HasKey("Appointment_Id");

                    b.HasIndex("DoctorMetadataDoc_meta_Id");

                    b.HasIndex("SlotId");

                    b.HasIndex("UserDetailsUser_Id");

                    b.ToTable("DS_Appointment");
                });

            modelBuilder.Entity("DoctorAppointment.Models.Assistant", b =>
                {
                    b.Property<int>("AssistaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AssistaId"), 1L, 1);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("First_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<string>("Last_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MetadataDoc_meta_Id")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("profile_Img")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("roles_UserRole_Id")
                        .HasColumnType("int");

                    b.HasKey("AssistaId");

                    b.HasIndex("MetadataDoc_meta_Id");

                    b.HasIndex("roles_UserRole_Id");

                    b.ToTable("DS_Assistant_Invitation");
                });

            modelBuilder.Entity("DoctorAppointment.Models.DoctorMapAssistant", b =>
                {
                    b.Property<int>("DoctorAssistantMapId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DoctorAssistantMapId"), 1L, 1);

                    b.Property<int>("AssistantAssistaId")
                        .HasColumnType("int");

                    b.Property<int>("DoctorMetadataDoc_meta_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsAssistantApprove")
                        .HasColumnType("bit");

                    b.HasKey("DoctorAssistantMapId");

                    b.HasIndex("AssistantAssistaId");

                    b.HasIndex("DoctorMetadataDoc_meta_Id");

                    b.ToTable("DS_Doctor_Map_Assistant");
                });

            modelBuilder.Entity("DoctorAppointment.Models.DoctorMetadata", b =>
                {
                    b.Property<int>("Doc_meta_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Doc_meta_Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("FeesAmount")
                        .HasColumnType("int");

                    b.Property<string>("License")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RequestDoctorApprove")
                        .HasColumnType("bit");

                    b.Property<string>("Specialist")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserDetailsUser_Id")
                        .HasColumnType("int");

                    b.HasKey("Doc_meta_Id");

                    b.HasIndex("UserDetailsUser_Id");

                    b.ToTable("DS_Doctor_Metadata");
                });

            modelBuilder.Entity("DoctorAppointment.Models.Roles_user", b =>
                {
                    b.Property<int>("Role_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Role_Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Role_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Role_Id");

                    b.ToTable("DS_User_Role");
                });

            modelBuilder.Entity("DoctorAppointment.Models.Slot", b =>
                {
                    b.Property<int>("SlotId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SlotId"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndTimeslot")
                        .HasColumnType("datetime2");

                    b.Property<int>("MetadataDoc_meta_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTimeslot")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("SlotId");

                    b.HasIndex("MetadataDoc_meta_Id");

                    b.ToTable("DS_slots");
                });

            modelBuilder.Entity("DoctorAppointment.Models.User_details", b =>
                {
                    b.Property<int>("User_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("User_Id"), 1L, 1);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("First_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<string>("Last_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("profile_Img")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("roles_UserRole_Id")
                        .HasColumnType("int");

                    b.HasKey("User_Id");

                    b.HasIndex("roles_UserRole_Id");

                    b.ToTable("DS_User_Account");
                });

            modelBuilder.Entity("DoctorAppointment.Models.Appointment", b =>
                {
                    b.HasOne("DoctorAppointment.Models.DoctorMetadata", "DoctorMetadata")
                        .WithMany()
                        .HasForeignKey("DoctorMetadataDoc_meta_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DoctorAppointment.Models.Slot", "Slot")
                        .WithMany()
                        .HasForeignKey("SlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DoctorAppointment.Models.User_details", "UserDetails")
                        .WithMany()
                        .HasForeignKey("UserDetailsUser_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DoctorMetadata");

                    b.Navigation("Slot");

                    b.Navigation("UserDetails");
                });

            modelBuilder.Entity("DoctorAppointment.Models.Assistant", b =>
                {
                    b.HasOne("DoctorAppointment.Models.DoctorMetadata", "Metadata")
                        .WithMany()
                        .HasForeignKey("MetadataDoc_meta_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DoctorAppointment.Models.Roles_user", "roles_User")
                        .WithMany()
                        .HasForeignKey("roles_UserRole_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Metadata");

                    b.Navigation("roles_User");
                });

            modelBuilder.Entity("DoctorAppointment.Models.DoctorMapAssistant", b =>
                {
                    b.HasOne("DoctorAppointment.Models.Assistant", "Assistant")
                        .WithMany()
                        .HasForeignKey("AssistantAssistaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DoctorAppointment.Models.DoctorMetadata", "DoctorMetadata")
                        .WithMany()
                        .HasForeignKey("DoctorMetadataDoc_meta_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assistant");

                    b.Navigation("DoctorMetadata");
                });

            modelBuilder.Entity("DoctorAppointment.Models.DoctorMetadata", b =>
                {
                    b.HasOne("DoctorAppointment.Models.User_details", "UserDetails")
                        .WithMany()
                        .HasForeignKey("UserDetailsUser_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserDetails");
                });

            modelBuilder.Entity("DoctorAppointment.Models.Slot", b =>
                {
                    b.HasOne("DoctorAppointment.Models.DoctorMetadata", "Metadata")
                        .WithMany()
                        .HasForeignKey("MetadataDoc_meta_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Metadata");
                });

            modelBuilder.Entity("DoctorAppointment.Models.User_details", b =>
                {
                    b.HasOne("DoctorAppointment.Models.Roles_user", "roles_User")
                        .WithMany()
                        .HasForeignKey("roles_UserRole_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("roles_User");
                });
#pragma warning restore 612, 618
        }
    }
}
