using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimejApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Lessons",
                newName: "LessonNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_Date_Number_TeacherId",
                table: "Lessons",
                newName: "IX_Lessons_Date_LessonNumber_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_Date_Number_AuditoryId",
                table: "Lessons",
                newName: "IX_Lessons_Date_LessonNumber_AuditoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LessonNumber",
                table: "Lessons",
                newName: "Number");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_Date_LessonNumber_TeacherId",
                table: "Lessons",
                newName: "IX_Lessons_Date_Number_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_Date_LessonNumber_AuditoryId",
                table: "Lessons",
                newName: "IX_Lessons_Date_Number_AuditoryId");
        }
    }
}
