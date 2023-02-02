// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Caching.Abstractions;
using Remora.Discord.Caching.Services;
using Remora.Rest.Core;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class MoveFromPossibleGhostChannelToPossibleGhostChannelRequestHandler
    : AsyncRequestHandler<MoveFromPossibleGhostChannelToPossibleGhostChannelRequest>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly CacheService _cacheService;

    public MoveFromPossibleGhostChannelToPossibleGhostChannelRequestHandler(
        RemoraDiscordBotDbContext dbContext,
        IMediator mediator, 
        CacheService cacheService)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _cacheService = cacheService;
    }

    protected override async Task Handle(
        MoveFromPossibleGhostChannelToPossibleGhostChannelRequest request,
        CancellationToken cancellationToken)
    {
        var key = CacheKey.StringKey($"VoiceState:{request.ToGuildId}:{request.UserId}");

        switch (request.FromChannelId.HasValue)
        {
            case false:
                await MoveFromNoChannelToPossibleGhostChannelAsync(request.ToChannelId, request.UserId, request.ToGuildId, request.GatewayEvent, cancellationToken);
                break;
            case true:
                await MoveFromPossibleGhostChannelToPossibleGhostChannelAsync(request.FromChannelId, request.ToChannelId, request.UserId, request.ToGuildId, request.GatewayEvent, cancellationToken);
                break;
        }
    }

    private async Task MoveFromPossibleGhostChannelToPossibleGhostChannelAsync(
        Snowflake? requestFromChannelId,
        Snowflake requestToChannelId,
        Snowflake requestUserId,
        Snowflake requestToGuildId,
        IVoiceStateUpdate gatewayEvent,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new LeavePossibleUserPersonalVocalRequest(requestFromChannelId.Value, requestUserId, gatewayEvent), cancellationToken);
        await _mediator.Send(new JoinPossibleVocalCreationRequest(requestToChannelId, requestUserId, requestToGuildId, gatewayEvent), cancellationToken);
    }

    private async Task MoveFromNoChannelToPossibleGhostChannelAsync(
        Snowflake? toChannelId,
        Snowflake userId,
        Snowflake toGuildId,
        IVoiceStateUpdate gatewayEvent,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new JoinPossibleVocalCreationRequest(toChannelId.Value, userId, toGuildId, gatewayEvent), cancellationToken);
    }
}