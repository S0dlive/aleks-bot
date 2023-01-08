// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Welcomer;
using RemoraDiscordBot.Plugins.Welcomer.Commands;

namespace RemoraDiscordBot.Plugins.Welcomer.Handlers.Commands;

public class CreateOrSetWelcomeChannelHandler
    : AsyncRequestHandler<CreateOrSetWelcomeChannelCommand>
{
    private readonly RemoraDiscordBotDbContext _dbContext;

    public CreateOrSetWelcomeChannelHandler(RemoraDiscordBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task Handle(
        CreateOrSetWelcomeChannelCommand request,
        CancellationToken cancellationToken)
    {
        var guild = await _dbContext.WelcomerGuilds
            .FirstOrDefaultAsync(g => g.GuildId == request.GuildID.ToLong(), cancellationToken);

        if (guild is null)
        {
            _dbContext.WelcomerGuilds.Add(new WelcomerGuild
            {
                GuildId = request.GuildID.ToLong(),
                WelcomeChannelId = request.ChannelID.ToLong()
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return;
        }

        guild.WelcomeChannelId = request.ChannelID.ToLong();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}