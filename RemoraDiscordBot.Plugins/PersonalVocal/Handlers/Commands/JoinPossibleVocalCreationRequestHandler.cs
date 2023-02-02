// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Caching.Abstractions;
using Remora.Discord.Caching.Services;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;
using RemoraDiscordBot.Plugins.PersonalVocal.Queries;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class JoinPossibleVocalCreationRequestHandler
    : AsyncRequestHandler<JoinPossibleVocalCreationRequest>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly CacheService _cacheService;
    
    public JoinPossibleVocalCreationRequestHandler(
        RemoraDiscordBotDbContext dbContext,
        IMediator mediator, 
        CacheService cacheService)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _cacheService = cacheService;
    }

    protected override async Task Handle(
        JoinPossibleVocalCreationRequest request,
        CancellationToken cancellationToken)
    {
        var vocalChannelBootstrap = await _mediator.Send(
            new GetUniqueGuildVocalChannelRequest(request.GuildId),
            cancellationToken);

        if (vocalChannelBootstrap is null)
        {
            return;
        }

        if (request.ToChannelId != vocalChannelBootstrap?.ChannelId.ToSnowflake())
        {
            return;
        }

        var newVocal = await _mediator.Send(
            new CreatePersonalUserVocalChannelRequest(
                request.UserId,
                request.GuildId,
                vocalChannelBootstrap.CategoryId.ToSnowflake()),
            cancellationToken);

        var key = CacheKey.StringKey($"VoiceState:{request.GatewayEvent.GuildID}:{request.GatewayEvent.UserID}");
        await _cacheService.CacheAsync(key, request.GatewayEvent, cancellationToken);

        await _mediator.Send(
            new PersistUserVocalChannelRequest(newVocal.ChannelId.ToSnowflake(), request.UserId, request.GuildId),
            cancellationToken);
    }
}