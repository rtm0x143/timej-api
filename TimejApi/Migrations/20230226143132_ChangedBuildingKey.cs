using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TimejApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangedBuildingKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auditories_Buildings_BuildingNumber",
                table: "Auditories");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Auditories_AuditoryNumber_AuditoryBuildingNumber",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_AuditoryNumber_AuditoryBuildingNumber",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Buildings",
                table: "Buildings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Auditories",
                table: "Auditories");

            migrationBuilder.DropIndex(
                name: "IX_Auditories_BuildingNumber",
                table: "Auditories");

            migrationBuilder.DropColumn(
                name: "AuditoryBuildingNumber",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "AuditoryNumber",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "BuildingNumber",
                table: "Auditories");

            migrationBuilder.AddColumn<Guid>(
                name: "AuditoryId",
                table: "Lessons",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Number",
                table: "Buildings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Buildings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Buildings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Auditories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "Auditories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Auditories",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Buildings",
                table: "Buildings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Auditories",
                table: "Auditories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_AuditoryId",
                table: "Lessons",
                column: "AuditoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditories_BuildingId",
                table: "Auditories",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auditories_Buildings_BuildingId",
                table: "Auditories",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Auditories_AuditoryId",
                table: "Lessons",
                column: "AuditoryId",
                principalTable: "Auditories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auditories_Buildings_BuildingId",
                table: "Auditories");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Auditories_AuditoryId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_AuditoryId",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Buildings",
                table: "Buildings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Auditories",
                table: "Auditories");

            migrationBuilder.DropIndex(
                name: "IX_Auditories_BuildingId",
                table: "Auditories");

            migrationBuilder.DropColumn(
                name: "AuditoryId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Auditories");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Auditories");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Auditories");

            migrationBuilder.AddColumn<long>(
                name: "AuditoryBuildingNumber",
                table: "Lessons",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuditoryNumber",
                table: "Lessons",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Number",
                table: "Buildings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "BuildingNumber",
                table: "Auditories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Buildings",
                table: "Buildings",
                column: "Number");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Auditories",
                table: "Auditories",
                columns: new[] { "AuditoryNumber", "BuildingNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_AuditoryNumber_AuditoryBuildingNumber",
                table: "Lessons",
                columns: new[] { "AuditoryNumber", "AuditoryBuildingNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Auditories_BuildingNumber",
                table: "Auditories",
                column: "BuildingNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Auditories_Buildings_BuildingNumber",
                table: "Auditories",
                column: "BuildingNumber",
                principalTable: "Buildings",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Auditories_AuditoryNumber_AuditoryBuildingNumber",
                table: "Lessons",
                columns: new[] { "AuditoryNumber", "AuditoryBuildingNumber" },
                principalTable: "Auditories",
                principalColumns: new[] { "AuditoryNumber", "BuildingNumber" });
        }
    }
}
