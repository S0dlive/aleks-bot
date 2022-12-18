// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;

namespace RemoraDiscordBot.Data;

public class RemoraDiscordBotDbContext
    : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string not found.");

        optionsBuilder.UseSqlite(connectionString);
    }
}