using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RemoraDiscordBot.Data.Migrations
{
    /// <inheritdoc />
    public partial class AutoRoleDomainMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoRoleChannel",
                columns: table => new
                {
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    GuildId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoRoleChannel", x => new { x.MessageId, x.GuildId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AutoRoleReaction",
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
                    table.PrimaryKey("PK_AutoRoleReaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutoRoleReaction_AutoRoleChannel_InstigatorMessageId_Instiga~",
                        columns: x => new { x.InstigatorMessageId, x.InstigatorGuildId },
                        principalTable: "AutoRoleChannel",
                        principalColumns: new[] { "MessageId", "GuildId" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AutoRoleReaction_InstigatorMessageId_InstigatorGuildId",
                table: "AutoRoleReaction",
                columns: new[] { "InstigatorMessageId", "InstigatorGuildId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoRoleReaction");

            migrationBuilder.DropTable(
                name: "AutoRoleChannel");
        }
    }
}
