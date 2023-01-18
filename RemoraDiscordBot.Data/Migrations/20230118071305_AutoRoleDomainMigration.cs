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
            migrationBuilder.DropForeignKey(
                name: "FK_AutoRoleReaction_AutoRoleChannel_InstigatorMessageId_Instiga~",
                table: "AutoRoleReaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutoRoleReaction",
                table: "AutoRoleReaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutoRoleChannel",
                table: "AutoRoleChannel");

            migrationBuilder.RenameTable(
                name: "AutoRoleReaction",
                newName: "AutoRoleReactions");

            migrationBuilder.RenameTable(
                name: "AutoRoleChannel",
                newName: "AutoRoleChannels");

            migrationBuilder.RenameIndex(
                name: "IX_AutoRoleReaction_InstigatorMessageId_InstigatorGuildId",
                table: "AutoRoleReactions",
                newName: "IX_AutoRoleReactions_InstigatorMessageId_InstigatorGuildId");

            migrationBuilder.AddColumn<long>(
                name: "ChannelId",
                table: "AutoRoleChannels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutoRoleReactions",
                table: "AutoRoleReactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutoRoleChannels",
                table: "AutoRoleChannels",
                columns: new[] { "MessageId", "GuildId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AutoRoleReactions_AutoRoleChannels_InstigatorMessageId_Insti~",
                table: "AutoRoleReactions",
                columns: new[] { "InstigatorMessageId", "InstigatorGuildId" },
                principalTable: "AutoRoleChannels",
                principalColumns: new[] { "MessageId", "GuildId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutoRoleReactions_AutoRoleChannels_InstigatorMessageId_Insti~",
                table: "AutoRoleReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutoRoleReactions",
                table: "AutoRoleReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AutoRoleChannels",
                table: "AutoRoleChannels");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "AutoRoleChannels");

            migrationBuilder.RenameTable(
                name: "AutoRoleReactions",
                newName: "AutoRoleReaction");

            migrationBuilder.RenameTable(
                name: "AutoRoleChannels",
                newName: "AutoRoleChannel");

            migrationBuilder.RenameIndex(
                name: "IX_AutoRoleReactions_InstigatorMessageId_InstigatorGuildId",
                table: "AutoRoleReaction",
                newName: "IX_AutoRoleReaction_InstigatorMessageId_InstigatorGuildId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutoRoleReaction",
                table: "AutoRoleReaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AutoRoleChannel",
                table: "AutoRoleChannel",
                columns: new[] { "MessageId", "GuildId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AutoRoleReaction_AutoRoleChannel_InstigatorMessageId_Instiga~",
                table: "AutoRoleReaction",
                columns: new[] { "InstigatorMessageId", "InstigatorGuildId" },
                principalTable: "AutoRoleChannel",
                principalColumns: new[] { "MessageId", "GuildId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
