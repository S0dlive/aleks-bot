// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Caching.Abstractions;
using Remora.Discord.Caching.Services;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Responders;

public sealed class JoinPossibleVocalCreationResponder
    : IResponder<IVoiceStateUpdate>
{
    private readonly CacheService _cacheService;
    private readonly ILogger<JoinPossibleVocalCreationResponder> _logger;
    private readonly IMediator _mediator;

    public JoinPossibleVocalCreationResponder(
        IMediator mediator,
        ILogger<JoinPossibleVocalCreationResponder> logger,
        CacheService cacheService)
    {
        _mediator = mediator;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<Result> RespondAsync(
        IVoiceStateUpdate gatewayEvent,
        CancellationToken ct = default)
    {
        var key = CacheKey.StringKey($"VoiceState:{gatewayEvent.GuildID}:{gatewayEvent.UserID}");

        var value = await _cacheService.TryGetValueAsync<IVoiceStateUpdate>(key, ct);
        IVoiceState previousState = null;
        if (value.IsSuccess)
        {
            previousState = value.Entity;
        }

        switch (gatewayEvent)
        {
            case not null when gatewayEvent.ChannelID is null:
                _logger.LogInformation(
                    "User {UserId} left the channel {ChannelId}",
                    gatewayEvent.UserID,
                    previousState?.ChannelID);

                await _mediator.Send(
                    new LeavePossibleUserPersonalVocalRequest(previousState.ChannelID.Value, gatewayEvent.UserID), ct);

                break;
            case not null when gatewayEvent.ChannelID is not null && previousState is null:
                _logger.LogInformation(
                    "User {UserId} joined the channel {ChannelId}",
                    gatewayEvent.UserID,
                    gatewayEvent.ChannelID);

                await _mediator.Send(
                    new JoinPossibleVocalCreationRequest(gatewayEvent.ChannelID.Value, gatewayEvent.UserID,
                        gatewayEvent.GuildID.Value), ct);

                break;
            case not null when gatewayEvent.ChannelID is not null && previousState is not null:
                _logger.LogInformation(
                    "User {UserId} moved from channel {OldChannelId} to channel {NewChannelId}",
                    gatewayEvent.UserID,
                    previousState.ChannelID,
                    gatewayEvent.ChannelID);

                await _mediator.Send(
                    new MoveFromPossibleGhostChannelToPossibleGhostChannelRequest(
                        previousState.ChannelID.Value,
                        gatewayEvent.ChannelID.Value,
                        gatewayEvent.UserID,
                        gatewayEvent.GuildID.Value), ct);

                break;
        }

        await _cacheService.CacheAsync<IVoiceStateUpdate>(key, gatewayEvent, ct);

        return Result.FromSuccess();
    }
}