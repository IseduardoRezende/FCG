using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingDefaultAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "Email", "IsDeleted", "Name", "Password", "Salt", "UserRoleId" },
                values: new object[] { -1L, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Utc), "fcg@admin.com", false, "Admin", "OxFPEaZRtmloJcAIHMItyJfep3S4tc5/ViQaZxtiiDQ=", "7f3c9a2e1b4d6f8a0c2e4a6b8d0f2a4c", 2L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1L);
        }
    }
}
