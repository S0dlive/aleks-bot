// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Experience;
using RemoraDiscordBot.Plugins.Experience.Queries;

namespace RemoraDiscordBot.Plugins.Experience.Handlers;

public sealed record GetUserBySnowflakeHandler(RemoraDiscordBotDbContext DbContext)
    : IRequestHandler<GetUserBySnowflakeQuery, UserGuildXp?>
{
    public async Task<UserGuildXp?> Handle(GetUserBySnowflakeQuery request, CancellationToken cancellationToken)
    {
        var user = await DbContext.UserGuildXps.FirstOrDefaultAsync(x => x.UserId == request.UserSnowflake.ToLong(), cancellationToken);

        ArgumentNullException.ThrowIfNull(user);
        
        return user;
    }
}
