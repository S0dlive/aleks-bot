// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Data.Domain.Welcomer;
using RemoraDiscordBot.Data.Domain.Xp;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserGuildXp>()
            .HasKey(x => new {x.UserId, x.GuildId});
    }
}