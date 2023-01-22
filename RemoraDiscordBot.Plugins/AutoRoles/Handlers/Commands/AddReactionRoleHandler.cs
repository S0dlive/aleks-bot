// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Results;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.AutoRoles.Commands;

namespace RemoraDiscordBot.Plugins.AutoRoles.Handlers.Commands;

public sealed class AddReactionRoleHandler 
    : AsyncRequestHandler<AddReactionRoleRequest>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly IDiscordRestInteractionAPI _interactionApi;
    private readonly IDiscordRestChannelAPI _channelAPI;

    public AddReactionRoleHandler(
        RemoraDiscordBotDbContext dbContext, 
        IDiscordRestChannelAPI channelApi, 
        IDiscordRestInteractionAPI interactionApi)
    {
        _dbContext = dbContext;
        _channelAPI = channelApi;
        _channelApi = channelApi;
        _interactionApi = interactionApi;
    }

    protected override async Task Handle(AddReactionRoleRequest request, CancellationToken cancellationToken)
    {
        var autoRole = await _dbContext.AutoRoleChannels
            .FirstOrDefaultAsync(x => x.ChannelId == request.ChannelId.ToLong() && x.GuildId == request.GuildId.ToLong(), cancellationToken);

        if (autoRole is null)
        {
            throw new InvalidOperationException("AutoRoleChannel not found");
        }
        
        var message = await _channelApi.GetChannelMessageAsync(request.ChannelId, request.MessageId, ct: cancellationToken);
        if (!message.IsSuccess)
        {
            throw new InvalidOperationException("Message not found");
        }
        
        // edit the message to add select menu interaction
    }
}