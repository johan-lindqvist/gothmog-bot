using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GothmogBot.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscordIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscordId",
                table: "DiscordUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordId",
                table: "DiscordUsers");
        }
    }
}
