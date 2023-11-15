using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GothmogBot.Migrations
{
    /// <inheritdoc />
    public partial class AddDotaMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "TwitchUsername");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "SteamAccountId");

            migrationBuilder.AddColumn<string>(
                name: "DiscordUsername",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DotaMatches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DotaMatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DotaMatchUser",
                columns: table => new
                {
                    DotaMatchesId = table.Column<long>(type: "INTEGER", nullable: false),
                    UsersSteamAccountId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DotaMatchUser", x => new { x.DotaMatchesId, x.UsersSteamAccountId });
                    table.ForeignKey(
                        name: "FK_DotaMatchUser_DotaMatches_DotaMatchesId",
                        column: x => x.DotaMatchesId,
                        principalTable: "DotaMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DotaMatchUser_Users_UsersSteamAccountId",
                        column: x => x.UsersSteamAccountId,
                        principalTable: "Users",
                        principalColumn: "SteamAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DotaMatchUser_UsersSteamAccountId",
                table: "DotaMatchUser",
                column: "UsersSteamAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DotaMatchUser");

            migrationBuilder.DropTable(
                name: "DotaMatches");

            migrationBuilder.DropColumn(
                name: "DiscordUsername",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "TwitchUsername",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SteamAccountId",
                table: "Users",
                newName: "Id");
        }
    }
}
