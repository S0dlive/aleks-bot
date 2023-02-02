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

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class LeavePossibleUserPersonalVocalRequestHandler
    : AsyncRequestHandler<LeavePossibleUserPersonalVocalRequest>
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly ILogger<LeavePossibleUserPersonalVocalRequestHandler> _logger;
    private readonly IMediator _mediator;
    private readonly CacheService _cacheService;

    public LeavePossibleUserPersonalVocalRequestHandler(
        IDiscordRestChannelAPI channelApi,
        ILogger<LeavePossibleUserPersonalVocalRequestHandler> logger,
        IMediator mediator,
        RemoraDiscordBotDbContext dbContext,
        CacheService cacheService)
    {
        _channelApi = channelApi;
        _logger = logger;
        _mediator = mediator;
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    protected override async Task Handle(
        LeavePossibleUserPersonalVocalRequest request,
        CancellationToken cancellationToken)
    {
        if (!request.FromChannelId.HasValue)
        {
            return;
        }
        
        var personalVocal = await _dbContext.UserPersonalVocals.FirstOrDefaultAsync(
            x => x.ChannelId == request.FromChannelId.Value.ToLong(),
            cancellationToken);
        
        if (personalVocal is null)
        {
            _logger.LogInformation(
                "User {UserId} left channel {ChannelId} but no personal vocal channel found",
                request.UserId,
                request.FromChannelId.Value);
            return;
        }
        
        var personalVocalChannel = await _channelApi.GetChannelAsync(
            request.FromChannelId.Value,
            ct: cancellationToken);

        if (!personalVocalChannel.IsSuccess)
        {
            throw new InvalidOperationException("Cannot get user vocal channel, reason: " + personalVocalChannel.Error.Message);
        }

        _logger.LogInformation(
            "User {UserId} left channel {ChannelId} with Type: {ChannelType} and Recipients: {ChannelRecipients}",
            request.UserId,
            personalVocalChannel.Entity.ID,
            personalVocalChannel.Entity.Type,
            personalVocalChannel.Entity.Recipients.HasValue ? personalVocalChannel.Entity.Recipients.Value.Count : 0);

        if (personalVocalChannel.Entity is
            {
                Type: ChannelType.GuildVoice,
                Recipients:
                {
                    HasValue: false
                }
            })
        {
            await _mediator.Send(
                new DeletePersonalUserVocalChannelRequest(personalVocalChannel.Entity.ID), cancellationToken);
        }
    }
}