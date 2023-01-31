// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Data.Domain.AutoRoles;
using RemoraDiscordBot.Data.Domain.Experience;
using RemoraDiscordBot.Data.Domain.PersonalVocal;
using RemoraDiscordBot.Data.Domain.Welcomer;

namespace RemoraDiscordBot.Data;

public class RemoraDiscordBotDbContext
    : DbContext
{
    public RemoraDiscordBotDbContext(
        DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<UserGuildXp> UserGuildXps { get; set; } = null!;

    public DbSet<WelcomerGuild> WelcomerGuilds { get; set; } = null!;

    public DbSet<UserGuildCreature> UserGuildCreatures { get; set; } = null!;

    public DbSet<AutoRoleChannel> AutoRoleChannels { get; set; } = null!;
    public DbSet<AutoRoleReaction> AutoRoleReactions { get; set; } = null!;
    public DbSet<PersonalVocal> PersonalVocals { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userGuildXp = modelBuilder.Entity<UserGuildXp>();
        var autoRole = modelBuilder.Entity<AutoRoleChannel>();
        var personalVocal = modelBuilder.Entity<PersonalVocal>();

        userGuildXp
            .HasKey(x => new {x.UserId, x.GuildId});

        userGuildXp
            .HasOne(u => u.Creature)
            .WithOne(c => c.Possessor)
            .HasForeignKey<UserGuildCreature>(c => new {c.PossessorId, c.PossessorGuildId});

        modelBuilder.Entity<UserGuildCreature>()
            .HasKey(c => c.Id);

        autoRole
            .HasKey(x => new {x.MessageId, x.GuildId});

        modelBuilder.Entity<AutoRoleReaction>()
            .HasKey(x => x.Id);

        autoRole
            .HasMany(x => x.Reactions)
            .WithOne(x => x.AutoRoleChannel)
            .HasForeignKey(x => new {x.InstigatorMessageId, x.InstigatorGuildId});

        personalVocal
            .HasKey(x => new {x.ChannelId, x.GuildId});
    }
}