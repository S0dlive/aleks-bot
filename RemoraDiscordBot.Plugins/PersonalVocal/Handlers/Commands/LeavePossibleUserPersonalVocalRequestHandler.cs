// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Caching.Abstractions;
using Remora.Discord.Caching.Services;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;
using RemoraDiscordBot.Plugins.PersonalVocal.Services;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class LeavePossibleUserPersonalVocalRequestHandler
    : AsyncRequestHandler<LeavePossibleUserPersonalVocalRequest>
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly ILogger<LeavePossibleUserPersonalVocalRequestHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IPersonalVocalService _personalVocalService;

    public LeavePossibleUserPersonalVocalRequestHandler(
        IDiscordRestChannelAPI channelApi,
        ILogger<LeavePossibleUserPersonalVocalRequestHandler> logger,
        IMediator mediator, 
        IPersonalVocalService personalVocalService)
    {
        _channelApi = channelApi;
        _logger = logger;
        _mediator = mediator;
        _personalVocalService = personalVocalService;
    }

    protected override async Task Handle(
        LeavePossibleUserPersonalVocalRequest request,
        CancellationToken cancellationToken)
    {
        if (!request.FromChannelId.HasValue)
        {
            return;
        }
        
        if (!_personalVocalService.HasVoiceChannel(request.UserId, request.GatewayEvent.GuildID.Value))
        {
            _personalVocalService.LeaveVoiceChannel(request.UserId);
            return;
        }
        
        var personalVocalChannel = await _channelApi.GetChannelAsync(
            request.FromChannelId.Value,
            ct: cancellationToken);

        if (!personalVocalChannel.IsSuccess)
        {
            throw new InvalidOperationException("Cannot get user vocal channel, reason: " + personalVocalChannel.Error.Message);
        }

        if (personalVocalChannel.Entity is
            {
                Type: ChannelType.GuildVoice,
                Recipients:
                {
                    HasValue: false
                }
            })
        {
            _personalVocalService.LeaveVoiceChannel(request.UserId);
            await _mediator.Send(
                new DeletePersonalUserVocalChannelRequest(personalVocalChannel.Entity.ID), cancellationToken);
        }
    }
}