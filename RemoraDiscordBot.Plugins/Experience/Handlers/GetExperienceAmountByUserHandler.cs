// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Xp;
using RemoraDiscordBot.Plugins.Experience.Queries;

namespace RemoraDiscordBot.Plugins.Experience.Handlers;

public class GetExperienceAmountByUserHandler
    : IRequestHandler<GetExperienceAmountByUserQuery, long>
{
    private readonly RemoraDiscordBotDbContext _context;
    private readonly ILogger<GetExperienceAmountByUserHandler> _logger;

    public GetExperienceAmountByUserHandler(
        ILogger<GetExperienceAmountByUserHandler> logger,
        RemoraDiscordBotDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<long> Handle(GetExperienceAmountByUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting experience amount for user {UserId}", request.UserId);

        var user = await _context.UserGuildXps.SingleOrDefaultAsync(
            x => x.UserId == request.UserId.ToLong() && x.GuildId == request.GuildId.ToLong(), cancellationToken);

        _logger.LogInformation(user is null
            ? $"User {request.UserId} not found in guild {request.GuildId}"
            : $"User {request.UserId} found in guild {request.GuildId}");

        if (user is null)
        {
            _logger.LogInformation("Creating new user {UserId} in guild {GuildId}", request.UserId, request.GuildId);
            
            user = new UserGuildXp(request.UserId.ToLong(), request.GuildId.ToLong());

            await _context.UserGuildXps.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return user.XpAmount;
    }
}