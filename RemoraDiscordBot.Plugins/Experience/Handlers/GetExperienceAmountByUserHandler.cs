// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Xp;
using RemoraDiscordBot.Plugins.Experience.Queries;

namespace RemoraDiscordBot.Plugins.Experience.Handlers;

public sealed record GetExperienceAmountByUserHandler(RemoraDiscordBotDbContext DbContext, ILogger<GetExperienceAmountByUserHandler> Logger)
    : IRequestHandler<GetExperienceAmountByUserQuery, ulong>
{
    public async Task<ulong> Handle(GetExperienceAmountByUserQuery request, CancellationToken cancellationToken)
    {
        var user = await DbContext.UserGuildXps.SingleOrDefaultAsync(
            x => x.UserId == request.UserId.AsT1 && x.GuildId == request.GuildId.AsT1, cancellationToken);

        Logger.LogInformation(user is null
            ? $"User {request.UserId.AsT1} not found in guild {request.GuildId.AsT1}"
            : $"User {request.UserId.AsT1} found in guild {request.GuildId.AsT1}");
        
        if (user is null)
        {
            user = new UserGuildXp(request.UserId.AsT1, request.GuildId.AsT1);

            await DbContext.UserGuildXps.AddAsync(user, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        return user.XpAmount;
    }
}