// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.PersonalVocal;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;
using RemoraDiscordBot.Plugins.PersonalVocal.Queries;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class JoinPossibleVocalCreationRequestHandler
    : AsyncRequestHandler<JoinPossibleVocalCreationRequest>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly IMediator _mediator;

    public JoinPossibleVocalCreationRequestHandler(
        RemoraDiscordBotDbContext dbContext,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    protected override async Task Handle(
        JoinPossibleVocalCreationRequest request,
        CancellationToken cancellationToken)
    {
        var vocalChannelBootstrap = await _mediator.Send(
            new GetUniqueGuildVocalChannelRequest(request.GatewayEvent.GuildID.Value),
            cancellationToken);

        if (vocalChannelBootstrap is null)
        {
            return;
        }

        if (request.GatewayEvent.ChannelID != vocalChannelBootstrap?.ChannelId.ToSnowflake())
        {
            return;
        }
       
        var newVocal = await _mediator.Send(
            new CreatePersonalUserVocalChannelRequest(
                request.GatewayEvent.UserID,
                request.GatewayEvent.GuildID.Value,
                vocalChannelBootstrap.CategoryId.ToSnowflake()),
            cancellationToken);
        
        await _mediator.Send(new PersistUserVocalChannelRequest(request.GatewayEvent, newVocal.ChannelId.ToSnowflake()), cancellationToken);
    }
}