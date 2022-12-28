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

public sealed record GetLevelByUserHandler(RemoraDiscordBotDbContext DbContext, ILogger<GetLevelByUserHandler> Logger)
    : IRequestHandler<GetLevelByUserQuery, long>
{
    public async Task<long> Handle(GetLevelByUserQuery request, CancellationToken cancellationToken)
    {
        var user = await DbContext.UserGuildXps.FirstOrDefaultAsync(
            x => x.UserId == request.UserId.ToLong()
                 && x.GuildId == request.GuildId.ToLong(),
            cancellationToken);

        if (user is null)
        {
            Logger.LogInformation("Creating new user {UserId} in guild {GuildId}", request.UserId, request.GuildId);

            user = new UserGuildXp(request.UserId.ToLong(), request.GuildId.ToLong());

            await DbContext.UserGuildXps.AddAsync(user, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        
        return user.Level;
    }
}