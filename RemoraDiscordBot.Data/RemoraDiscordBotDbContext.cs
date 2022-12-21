// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RemoraDiscordBot.Data.Domain.Xp;

namespace RemoraDiscordBot.Data;

public class RemoraDiscordBotDbContext
    : DbContext
{
    private readonly ILogger<RemoraDiscordBotDbContext> _logger;
    
    private readonly ILoggerFactory _loggerFactory;

    public RemoraDiscordBotDbContext(
        DbContextOptions options,
        ILogger<RemoraDiscordBotDbContext> logger, 
        ILoggerFactory loggerFactory)
        : base(options)
    {
        _logger = logger;
        _loggerFactory = new LoggerFactory();
    }

    public DbSet<UserGuildXp> UserGuildXps { get; set; } = null!;
}