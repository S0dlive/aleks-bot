using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoraDiscordBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class PersonalVocalUpdatePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AutoRoleChannels",
                columns: table => new
                {
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    GuildId = table.Column<long>(type: "bigint", nullable: false),
                    ChannelId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoRoleChannels", x => new { x.MessageId, x.GuildId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PersonalVocals",
                columns: table => new
                {
                    GuildId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChannelId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalVocals", x => x.GuildId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserGuildXps",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GuildId = table.Column<long>(type: "bigint", nullable: false),
                    XpAmount = table.Column<long>(type: "bigint", nullable: false),
                    Level = table.Column<long>(type: "bigint", nullable: false),
                    XpNeededToLevelUp = table.Column<long>(type: "bigint", nullable: false),
                    CreatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGuildXps", x => new { x.UserId, x.GuildId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserPersonalVocals",
                columns: table => new
                {
                    GuildId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ChannelId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPersonalVocals", x => new { x.UserId, x.GuildId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WelcomerGuilds",
                columns: table => new
                {
                    GuildId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsWelcomerEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    WelcomeMessage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WelcomeChannelId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelcomerGuilds", x => x.GuildId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AutoRoleReactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Label = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Emoji = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    InstigatorMessageId = table.Column<long>(type: "bigint", nullable: false),
                    InstigatorGuildId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoRoleReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutoRoleReactions_AutoRoleChannels_InstigatorMessageId_Insti~",
                        columns: x => new { x.InstigatorMessageId, x.InstigatorGuildId },
                        principalTable: "AutoRoleChannels",
                        principalColumns: new[] { "MessageId", "GuildId" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserGuildCreatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatureType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsEgg = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    PossessorId = table.Column<long>(type: "bigint", nullable: false),
                    PossessorGuildId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGuildCreatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGuildCreatures_UserGuildXps_PossessorId_PossessorGuildId",
                        columns: x => new { x.PossessorId, x.PossessorGuildId },
                        principalTable: "UserGuildXps",
                        principalColumns: new[] { "UserId", "GuildId" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AutoRoleReactions_InstigatorMessageId_InstigatorGuildId",
                table: "AutoRoleReactions",
                columns: new[] { "InstigatorMessageId", "InstigatorGuildId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserGuildCreatures_PossessorId_PossessorGuildId",
                table: "UserGuildCreatures",
                columns: new[] { "PossessorId", "PossessorGuildId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoRoleReactions");

            migrationBuilder.DropTable(
                name: "PersonalVocals");

            migrationBuilder.DropTable(
                name: "UserGuildCreatures");

            migrationBuilder.DropTable(
                name: "UserPersonalVocals");

            migrationBuilder.DropTable(
                name: "WelcomerGuilds");

            migrationBuilder.DropTable(
                name: "AutoRoleChannels");

            migrationBuilder.DropTable(
                name: "UserGuildXps");
        }
    }
}
