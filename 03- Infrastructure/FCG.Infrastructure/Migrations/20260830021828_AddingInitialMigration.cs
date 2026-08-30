using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FCG.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: false),
                    Salt = table.Column<string>(type: "character varying(64)", unicode: false, maxLength: 64, nullable: false),
                    UserRoleId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGame",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    PurchasedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGame_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGame_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1L, false, "User" },
                    { 2L, false, "Administrator" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserRoleId",
                table: "User",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGame_GameId",
                table: "UserGame",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGame_UserId_GameId",
                table: "UserGame",
                columns: new[] { "UserId", "GameId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Name",
                table: "UserRole",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGame");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserRole");
        }
    }
}
