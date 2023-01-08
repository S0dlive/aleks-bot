// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Welcomer;
using RemoraDiscordBot.Plugins.Welcomer.Queries;

namespace RemoraDiscordBot.Plugins.Welcomer.Handlers.Queries;

public sealed record GetsIfGuildAlreadyRegisteredHandler(RemoraDiscordBotDbContext DbContext)
    : IRequestHandler<GetsIfGuildAlreadyRegisteredQuery, WelcomerGuild?>
{
    public async Task<WelcomerGuild?> Handle(
        GetsIfGuildAlreadyRegisteredQuery request,
        CancellationToken cancellationToken)
    {
        var guild = await DbContext.WelcomerGuilds
            .AsQueryable()
            .FirstOrDefaultAsync(g => g.GuildId == request.GuildId.ToLong(), cancellationToken);

        return guild;
    }
}