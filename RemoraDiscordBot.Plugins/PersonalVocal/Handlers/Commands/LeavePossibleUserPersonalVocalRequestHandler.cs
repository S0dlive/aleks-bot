// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
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

    public LeavePossibleUserPersonalVocalRequestHandler(
        IDiscordRestChannelAPI channelApi,
        ILogger<LeavePossibleUserPersonalVocalRequestHandler> logger,
        IMediator mediator,
        RemoraDiscordBotDbContext dbContext)
    {
        _channelApi = channelApi;
        _logger = logger;
        _mediator = mediator;
        _dbContext = dbContext;
    }

    protected override async Task Handle(
        LeavePossibleUserPersonalVocalRequest request,
        CancellationToken cancellationToken)
    {
        var isPersonalVocal = await _dbContext.UserPersonalVocals.AnyAsync(
            x => x.ChannelId == request.FromChannelId.ToLong(),
            cancellationToken);

        if (!isPersonalVocal)
        {
            _logger.LogInformation(
                "User {UserId} left channel {ChannelId} but it's not a personal vocal",
                request.UserId,
                request.FromChannelId);
            
            return;
        }

        var personalVocal = await _dbContext.UserPersonalVocals.SingleAsync(
            x => x.ChannelId.ToSnowflake() == request.FromChannelId,
            cancellationToken);
        
        var personalVocalChannel = await _channelApi.GetChannelAsync(
            personalVocal.ChannelId.ToSnowflake(),
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