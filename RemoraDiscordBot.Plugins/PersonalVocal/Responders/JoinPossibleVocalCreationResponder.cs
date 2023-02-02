// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;
using RemoraDiscordBot.Plugins.PersonalVocal.Services;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Responders;

public sealed class JoinPossibleVocalCreationResponder
    : IResponder<IVoiceStateUpdate>
{
    private readonly ILogger<JoinPossibleVocalCreationResponder> _logger;
    private readonly IMediator _mediator;
    private readonly IPersonalVocalService _personalVocalService;

    public JoinPossibleVocalCreationResponder(
        IMediator mediator,
        ILogger<JoinPossibleVocalCreationResponder> logger,
        IPersonalVocalService personalVocalService)
    {
        _mediator = mediator;
        _logger = logger;
        _personalVocalService = personalVocalService;
    }

    public async Task<Result> RespondAsync(
        IVoiceStateUpdate gatewayEvent,
        CancellationToken ct = default)
    {
        var previousChannel = _personalVocalService.GetVoiceChannel(gatewayEvent.UserID);

        switch (gatewayEvent)
        {
            case not null when gatewayEvent.ChannelID is null:
                await _mediator.Send(
                    new LeavePossibleUserPersonalVocalRequest(previousChannel, gatewayEvent.UserID, gatewayEvent), ct);

                break;
            case not null when gatewayEvent.ChannelID is not null && previousChannel is null:
                await _mediator.Send(
                    new JoinPossibleVocalCreationRequest(
                        gatewayEvent.ChannelID.Value,
                        gatewayEvent.UserID,
                        gatewayEvent.GuildID.Value,
                        gatewayEvent),
                    ct);

                break;
            case not null when gatewayEvent.ChannelID is not null && previousChannel != null:
                await _mediator.Send(
                    new MoveFromPossibleGhostChannelToPossibleGhostChannelRequest(
                        previousChannel,
                        gatewayEvent.ChannelID.Value,
                        gatewayEvent.UserID,
                        gatewayEvent.GuildID.Value,
                        gatewayEvent),
                    ct);

                break;
        }

        return Result.FromSuccess();
    }
}