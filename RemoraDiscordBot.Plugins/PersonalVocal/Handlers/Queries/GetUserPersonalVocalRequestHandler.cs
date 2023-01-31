// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.PersonalVocal;
using RemoraDiscordBot.Plugins.PersonalVocal.Queries;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Queries;

public sealed record GetUserPersonalVocalRequestHandler(RemoraDiscordBotDbContext DbContext)
    : IRequestHandler<GetUserPersonalVocalRequest, UserPersonalVocal?>
{
    public async Task<UserPersonalVocal?> Handle(
        GetUserPersonalVocalRequest request,
        CancellationToken cancellationToken)
    {
        return await DbContext.UserPersonalVocals
            .FirstOrDefaultAsync(
                x => x.UserId == request.UserId.ToLong()
                     && x.GuildId == request.GuildId.ToLong(),
                cancellationToken);
    }
}