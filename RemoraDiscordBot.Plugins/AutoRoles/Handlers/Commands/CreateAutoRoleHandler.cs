// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using RemoraDiscordBot.Business.Colors;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.AutoRoles;
using RemoraDiscordBot.Plugins.AutoRoles.Commands;

namespace RemoraDiscordBot.Plugins.AutoRoles.Handlers.Commands;

public sealed class CreateAutoRoleHandler
    : IRequestHandler<CreateAutoRoleCommand, bool>
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly ILogger<CreateAutoRoleHandler> _logger;

    public CreateAutoRoleHandler(
        RemoraDiscordBotDbContext dbContext,
        ILogger<CreateAutoRoleHandler> logger,
        IDiscordRestChannelAPI channelApi)
    {
        _dbContext = dbContext;
        _logger = logger;
        _channelApi = channelApi;
    }

    public async Task<bool> Handle(CreateAutoRoleCommand request, CancellationToken cancellationToken)
    {
        var alreadyCreated = await _dbContext.AutoRoleChannels
            .FirstOrDefaultAsync(
                x => x.GuildId == request.GuildId.ToLong() && x.ChannelId == request.ChannelId.ToLong(),
                cancellationToken);

        if (alreadyCreated is not null)
        {
            _logger.LogWarning("AutoRoleChannel already created for guild {GuildId} and channel {ChannelId}",
                request.GuildId, request.ChannelId);
            return false;
        }

        var embed = new Embed(Description: request.Message, Colour: DiscordTransparentColor.Value);
        var message =
            await _channelApi.CreateMessageAsync(request.ChannelId, embeds: new[] {embed}, ct: cancellationToken);

        var autoRoleChannel = new AutoRoleChannel(message.Entity.ID.ToLong(), request.ChannelId.ToLong(),
            request.GuildId.ToLong());

        await _dbContext.AutoRoleChannels.AddAsync(autoRoleChannel, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}