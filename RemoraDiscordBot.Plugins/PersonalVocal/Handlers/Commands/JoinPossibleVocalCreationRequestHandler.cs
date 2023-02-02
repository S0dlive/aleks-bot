﻿// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;
using RemoraDiscordBot.Plugins.PersonalVocal.Queries;
using RemoraDiscordBot.Plugins.PersonalVocal.Services;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class JoinPossibleVocalCreationRequestHandler
    : AsyncRequestHandler<JoinPossibleVocalCreationRequest>
{
    private readonly IMediator _mediator;
    private readonly IPersonalVocalService _personalVocalService;

    public JoinPossibleVocalCreationRequestHandler(
        IMediator mediator,
        IPersonalVocalService personalVocalService)
    {
        _mediator = mediator;
        _personalVocalService = personalVocalService;
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

        if (request.ToChannelId != vocalChannelBootstrap.ChannelId.ToSnowflake())
        {
            return;
        }

        var newVocal = await _mediator.Send(
            new CreatePersonalUserVocalChannelRequest(
                request.UserId,
                request.GuildId,
                vocalChannelBootstrap.CategoryId.ToSnowflake()),
            cancellationToken);

        _personalVocalService.LeaveVoiceChannel(request.UserId);
        _personalVocalService.JoinVoiceChannel(request.UserId, newVocal.ChannelId.ToSnowflake());

        await _mediator.Send(
            new PersistUserVocalChannelRequest(newVocal.ChannelId.ToSnowflake(), request.UserId, request.GuildId),
            cancellationToken);
    }
}