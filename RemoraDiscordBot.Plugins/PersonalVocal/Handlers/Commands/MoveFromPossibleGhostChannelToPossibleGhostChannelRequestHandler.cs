// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Rest.Core;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class MoveFromPossibleGhostChannelToPossibleGhostChannelRequestHandler
    : AsyncRequestHandler<MoveFromPossibleGhostChannelToPossibleGhostChannelRequest>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly IMediator _mediator;

    public MoveFromPossibleGhostChannelToPossibleGhostChannelRequestHandler(
        RemoraDiscordBotDbContext dbContext,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    protected override async Task Handle(
        MoveFromPossibleGhostChannelToPossibleGhostChannelRequest request,
        CancellationToken cancellationToken)
    {
        switch (request.FromChannelId.HasValue)
        {
            case false:
                await MoveFromNoChannelToPossibleGhostChannelAsync(request.ToChannelId, request.UserId, request.ToGuildId, cancellationToken);
                break;
            case true:
                await MoveFromPossibleGhostChannelToPossibleGhostChannelAsync(request.FromChannelId, request.ToChannelId, request.UserId, request.ToGuildId, cancellationToken);
                break;
        }
    }

    private async Task MoveFromPossibleGhostChannelToPossibleGhostChannelAsync(
        Snowflake? requestFromChannelId,
        Snowflake requestToChannelId,
        Snowflake requestUserId,
        Snowflake requestToGuildId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new LeavePossibleUserPersonalVocalRequest(requestFromChannelId.Value, requestUserId), cancellationToken);
        await _mediator.Send(new JoinPossibleVocalCreationRequest(requestToChannelId, requestUserId, requestToGuildId), cancellationToken);
    }

    private async Task MoveFromNoChannelToPossibleGhostChannelAsync(
        Snowflake? toChannelId,
        Snowflake userId,
        Snowflake toGuildId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new JoinPossibleVocalCreationRequest(toChannelId.Value, userId, toGuildId), cancellationToken);
    }
}