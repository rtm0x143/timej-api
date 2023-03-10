using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimejApi.Migrations
{
    /// <inheritdoc />
    public partial class LessonReplicaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReplicaId",
                table: "Lessons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<long>(
                name: "Number",
                table: "Buildings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Auditories",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Date_Number_AuditoryId",
                table: "Lessons",
                columns: new[] { "Date", "Number", "AuditoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Date_Number_TeacherId",
                table: "Lessons",
                columns: new[] { "Date", "Number", "TeacherId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ReplicaId",
                table: "Lessons",
                column: "ReplicaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lessons_Date_Number_AuditoryId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_Date_Number_TeacherId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ReplicaId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ReplicaId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Auditories");

            migrationBuilder.AlterColumn<long>(
                name: "Number",
                table: "Buildings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
