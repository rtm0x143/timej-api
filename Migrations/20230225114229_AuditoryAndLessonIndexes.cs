using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimejApi.Migrations
{
    /// <inheritdoc />
    public partial class AuditoryAndLessonIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Auditories_AuditoryNumber_AuditoryBuildingNumber",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_AuditoryNumber_AuditoryBuildingNumber",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "AuditoryBuildingNumber",
                table: "Lessons");

            migrationBuilder.AlterColumn<long>(
                name: "AuditoryNumber",
                table: "Lessons",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuditoryBuilding",
                table: "Lessons",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "ReplicaId",
                table: "Lessons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Auditories",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_AuditoryBuilding_AuditoryNumber",
                table: "Lessons",
                columns: new[] { "AuditoryBuilding", "AuditoryNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Date_Number_AuditoryNumber_AuditoryBuilding",
                table: "Lessons",
                columns: new[] { "Date", "Number", "AuditoryNumber", "AuditoryBuilding" },
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

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Auditories_AuditoryBuilding_AuditoryNumber",
                table: "Lessons",
                columns: new[] { "AuditoryBuilding", "AuditoryNumber" },
                principalTable: "Auditories",
                principalColumns: new[] { "AuditoryNumber", "BuildingNumber" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Auditories_AuditoryBuilding_AuditoryNumber",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_AuditoryBuilding_AuditoryNumber",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_Date_Number_AuditoryNumber_AuditoryBuilding",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_Date_Number_TeacherId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ReplicaId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "AuditoryBuilding",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ReplicaId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Auditories");

            migrationBuilder.AlterColumn<long>(
                name: "AuditoryNumber",
                table: "Lessons",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "AuditoryBuildingNumber",
                table: "Lessons",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_AuditoryNumber_AuditoryBuildingNumber",
                table: "Lessons",
                columns: new[] { "AuditoryNumber", "AuditoryBuildingNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Auditories_AuditoryNumber_AuditoryBuildingNumber",
                table: "Lessons",
                columns: new[] { "AuditoryNumber", "AuditoryBuildingNumber" },
                principalTable: "Auditories",
                principalColumns: new[] { "AuditoryNumber", "BuildingNumber" });
        }
    }
}
