// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.PersonalVocal;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class PersistUserVocalChannelRequestHandler
    : IRequestHandler<PersistUserVocalChannelRequest, UserPersonalVocal>
{
    private readonly RemoraDiscordBotDbContext _dbContext;

    public PersistUserVocalChannelRequestHandler(
        RemoraDiscordBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserPersonalVocal> Handle(
        PersistUserVocalChannelRequest request,
        CancellationToken cancellationToken)
    {
        var userPersonalVocal = await _dbContext.UserPersonalVocals
            .FirstOrDefaultAsync(
                x => x.UserId == request.UserId.ToLong()
                     && x.GuildId == request.GuildId.ToLong(),
                cancellationToken) ?? new UserPersonalVocal
        {
            ChannelId = request.ChannelId.ToLong(),
            GuildId = request.GuildId.ToLong(),
            UserId = request.UserId.ToLong()
        };

        _dbContext.AddOrUpdate(userPersonalVocal);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return userPersonalVocal;
    }
}