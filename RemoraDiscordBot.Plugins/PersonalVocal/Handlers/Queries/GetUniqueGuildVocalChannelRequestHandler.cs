// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.PersonalVocal.Queries;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Queries;

public sealed record GetUniqueGuildVocalChannelRequestHandler(RemoraDiscordBotDbContext DbContext)
    : IRequestHandler<GetUniqueGuildVocalChannelRequest, Data.Domain.PersonalVocal.PersonalVocal?>
{
    public async Task<Data.Domain.PersonalVocal.PersonalVocal?> Handle(
        GetUniqueGuildVocalChannelRequest request,
        CancellationToken cancellationToken)
    {
        var channel = await DbContext.PersonalVocals
            .Where(x => x.GuildId == request.GuildId.ToLong())
            .FirstOrDefaultAsync(cancellationToken);

        return channel;
    }
}