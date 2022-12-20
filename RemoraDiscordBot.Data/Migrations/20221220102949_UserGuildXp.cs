using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoraDiscordBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserGuildXp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGuildXps",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GuildId = table.Column<long>(type: "INTEGER", nullable: false),
                    XpAmount = table.Column<long>(type: "INTEGER", nullable: false),
                    Level = table.Column<long>(type: "INTEGER", nullable: false),
                    XpNeededToLevelUp = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGuildXps", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGuildXps");
        }
    }
}
