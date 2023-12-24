using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GothmogBot.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDiscordIdColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ulong>(
                name: "DiscordId",
                table: "DiscordUsers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DiscordId",
                table: "DiscordUsers",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER");
        }
    }
}
