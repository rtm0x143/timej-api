using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimejApi.Migrations
{
    /// <inheritdoc />
    public partial class AuditoryIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Auditories_AuditoryNumber_BuildingId",
                table: "Auditories",
                columns: new[] { "AuditoryNumber", "BuildingId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Auditories_AuditoryNumber_BuildingId",
                table: "Auditories");
        }
    }
}
