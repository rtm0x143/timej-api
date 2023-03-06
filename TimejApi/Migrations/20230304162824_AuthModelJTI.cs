using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimejApi.Migrations
{
    /// <inheritdoc />
    public partial class AuthModelJTI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_FacultyId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_AuthenticationModels_RefreshTokenId",
                table: "AuthenticationModels");

            migrationBuilder.AddColumn<Guid>(
                name: "AssociatedAccessTokenId",
                table: "AuthenticationModels",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Groups_FacultyId_GroupNumber",
                table: "Groups",
                columns: new[] { "FacultyId", "GroupNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationModels_AssociatedAccessTokenId",
                table: "AuthenticationModels",
                column: "AssociatedAccessTokenId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_FacultyId_GroupNumber",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_AuthenticationModels_AssociatedAccessTokenId",
                table: "AuthenticationModels");

            migrationBuilder.DropColumn(
                name: "AssociatedAccessTokenId",
                table: "AuthenticationModels");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_FacultyId",
                table: "Groups",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationModels_RefreshTokenId",
                table: "AuthenticationModels",
                column: "RefreshTokenId",
                unique: true);
        }
    }
}
