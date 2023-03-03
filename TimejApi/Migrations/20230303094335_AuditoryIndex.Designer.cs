﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TimejApi.Data;

#nullable disable

namespace TimejApi.Migrations
{
    [DbContext(typeof(ScheduleDbContext))]
    [Migration("20230303094335_AuditoryIndex")]
    partial class AuditoryIndex
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TimejApi.Data.Entities.Auditory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("AuditoryNumber")
                        .HasColumnType("bigint");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.HasIndex("AuditoryNumber", "BuildingId")
                        .IsUnique();

                    b.ToTable("Auditories");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.AuthenticationModel", b =>
                {
                    b.Property<Guid>("RefreshTokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("RefreshTokenExpiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("RefreshTokenId");

                    b.HasIndex("RefreshTokenId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("AuthenticationModels");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.Building", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<long?>("Number")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.Faculty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("uuid");

                    b.Property<long>("GroupNumber")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.Lesson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AuditoryId")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<Guid>("LessonTypeId")
                        .HasColumnType("uuid");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AuditoryId");

                    b.HasIndex("LessonTypeId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.LessonGroup", b =>
                {
                    b.Property<Guid>("LessonId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<long?>("SubgroupNumber")
                        .HasColumnType("bigint");

                    b.HasKey("LessonId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("LessonGroup");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.LessonType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LessonTypes");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.Subject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<Guid?>("StudentGroupId")
                        .HasColumnType("uuid");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("StudentGroupId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.UserEditFacultyPermission", b =>
                {
                    b.Property<Guid>("EditorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AllowedFacultyId")
                        .HasColumnType("uuid");

                    b.HasKey("EditorId", "AllowedFacultyId");

                    b.HasIndex("AllowedFacultyId");

                    b.ToTable("UserEditFacultyPermissions");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.UserRole", b =>
                {
                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Role", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.Auditory", b =>
                {
                    b.HasOne("TimejApi.Data.Entities.Building", "Building")
                        .WithMany()
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Building");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.AuthenticationModel", b =>
                {
                    b.HasOne("TimejApi.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.Group", b =>
                {
                    b.HasOne("TimejApi.Data.Entities.Faculty", "Faculty")
                        .WithMany("Groups")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.Lesson", b =>
                {
                    b.HasOne("TimejApi.Data.Entities.Auditory", "Auditory")
                        .WithMany()
                        .HasForeignKey("AuditoryId");

                    b.HasOne("TimejApi.Data.Entities.LessonType", "LessonType")
                        .WithMany()
                        .HasForeignKey("LessonTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TimejApi.Data.Entities.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TimejApi.Data.Entities.User", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auditory");

                    b.Navigation("LessonType");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.LessonGroup", b =>
                {
                    b.HasOne("TimejApi.Data.Entities.Group", "Group")
                        .WithMany("Lessons")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TimejApi.Data.Entities.Lesson", "Lesson")
                        .WithMany("AttendingGroups")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Lesson");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.User", b =>
                {
                    b.HasOne("TimejApi.Data.Entities.Group", "StudentGroup")
                        .WithMany()
                        .HasForeignKey("StudentGroupId");

                    b.Navigation("StudentGroup");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.UserEditFacultyPermission", b =>
                {
                    b.HasOne("TimejApi.Data.Entities.Faculty", "AllowedFaculty")
                        .WithMany()
                        .HasForeignKey("AllowedFacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TimejApi.Data.Entities.User", "Editor")
                        .WithMany()
                        .HasForeignKey("EditorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AllowedFaculty");

                    b.Navigation("Editor");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.UserRole", b =>
                {
                    b.HasOne("TimejApi.Data.Entities.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.Faculty", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.Group", b =>
                {
                    b.Navigation("Lessons");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.Lesson", b =>
                {
                    b.Navigation("AttendingGroups");
                });

            modelBuilder.Entity("TimejApi.Data.Entities.User", b =>
                {
                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
