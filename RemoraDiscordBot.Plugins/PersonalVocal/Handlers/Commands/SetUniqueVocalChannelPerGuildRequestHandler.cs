// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class SetUniqueVocalChannelPerGuildRequestHandler
    : AsyncRequestHandler<SetUniqueVocalChannelPerGuildRequest>
{
    private readonly RemoraDiscordBotDbContext _dbContext;

    public SetUniqueVocalChannelPerGuildRequestHandler(
        RemoraDiscordBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task Handle(
        SetUniqueVocalChannelPerGuildRequest request,
        CancellationToken cancellationToken)
    {
        await _dbContext.PersonalVocals.AddOrUpdateAsync(
            new Data.Domain.PersonalVocal.PersonalVocal
            {
                GuildId = request.GuildId.ToLong(),
                ChannelId = request.ChannelId.ToLong()
            },
            x => x.GuildId == request.GuildId.ToLong(),
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}