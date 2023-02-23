using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimejApi.Migrations
{
    /// <inheritdoc />
    public partial class DbSet_UserEditFacultyPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEditFacultyPermission");

            migrationBuilder.CreateTable(
                name: "UserEditFacultyPermissions",
                columns: table => new
                {
                    EditorId = table.Column<Guid>(type: "uuid", nullable: false),
                    AllowedFacultyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEditFacultyPermissions", x => new { x.EditorId, x.AllowedFacultyId });
                    table.ForeignKey(
                        name: "FK_UserEditFacultyPermissions_Faculties_AllowedFacultyId",
                        column: x => x.AllowedFacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEditFacultyPermissions_Users_EditorId",
                        column: x => x.EditorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEditFacultyPermissions_AllowedFacultyId",
                table: "UserEditFacultyPermissions",
                column: "AllowedFacultyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEditFacultyPermissions");

            migrationBuilder.CreateTable(
                name: "UserEditFacultyPermission",
                columns: table => new
                {
                    EditorId = table.Column<Guid>(type: "uuid", nullable: false),
                    AllowedFacultyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEditFacultyPermission", x => new { x.EditorId, x.AllowedFacultyId });
                    table.ForeignKey(
                        name: "FK_UserEditFacultyPermission_Faculties_AllowedFacultyId",
                        column: x => x.AllowedFacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEditFacultyPermission_Users_EditorId",
                        column: x => x.EditorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEditFacultyPermission_AllowedFacultyId",
                table: "UserEditFacultyPermission",
                column: "AllowedFacultyId");
        }
    }
}
