// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest.Core;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;
using RemoraDiscordBot.Plugins.PersonalVocal.Queries;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class LeavePossibleUserPersonalVocalRequestHandler
    : AsyncRequestHandler<LeavePossibleUserPersonalVocalRequest>
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly ILogger<LeavePossibleUserPersonalVocalRequestHandler> _logger;
    private readonly IMediator _mediator;

    public LeavePossibleUserPersonalVocalRequestHandler(
        IDiscordRestChannelAPI channelApi,
        ILogger<LeavePossibleUserPersonalVocalRequestHandler> logger,
        IMediator mediator)
    {
        _channelApi = channelApi;
        _logger = logger;
        _mediator = mediator;
    }

    protected override async Task Handle(
        LeavePossibleUserPersonalVocalRequest request,
        CancellationToken cancellationToken)
    {
        var userPersistantVocalChannel = await _mediator.Send(
            new GetUserPersonalVocalRequest(
                request.GatewayEvent.UserID,
                request.GatewayEvent.GuildID.Value),
            cancellationToken);

        if (userPersistantVocalChannel is null)
        {
            throw new InvalidOperationException("User has no personal vocal channel");
        }
        
        var channelAsync = await _channelApi.GetChannelAsync(
            userPersistantVocalChannel.ChannelId.ToSnowflake(),
            cancellationToken);
        
        if (!channelAsync.IsSuccess)
        {
            throw new InvalidOperationException("Cannot get user vocal channel, reason: " + channelAsync.Error.Message);
        }
        
        _logger.LogInformation("User {UserId} left channel {ChannelId} with Type: {ChannelType} and Recipients: {ChannelRecipients}",
            request.GatewayEvent.UserID,
            channelAsync.Entity.ID,
            channelAsync.Entity.Type,
            channelAsync.Entity.Recipients.HasValue ? channelAsync.Entity.Recipients.Value.Count : 0);
        
        if (channelAsync.Entity is
            {
                Type: ChannelType.GuildVoice,
                Recipients:
                {
                    HasValue: false
                }
            })
        {
            await _mediator.Send(
                new DeletePersonalUserVocalChannelRequest(
                    request.GatewayEvent.UserID,
                    channelAsync.Entity.ID,
                    request.GatewayEvent.GuildID.Value),
                cancellationToken);
        }
    }
}