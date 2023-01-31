// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;
using RemoraDiscordBot.Plugins.PersonalVocal.Queries;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Responders;

public class JoinPossibleVocalCreationResponder
    : IResponder<IVoiceStateUpdate>
{
    private readonly ILogger<JoinPossibleVocalCreationResponder> _logger;
    private readonly IMediator _mediator;

    public JoinPossibleVocalCreationResponder(
        IMediator mediator,
        ILogger<JoinPossibleVocalCreationResponder> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Result> RespondAsync(
        IVoiceStateUpdate gatewayEvent,
        CancellationToken ct = default)
    {
        if (gatewayEvent.ChannelID is null)
        {
            var personalVocalBootstrap = await _mediator.Send(new GetUniqueGuildVocalChannelRequest(gatewayEvent.GuildID.Value), ct);

            if (personalVocalBootstrap is null) return Result.FromSuccess();
            
            await _mediator.Send(new LeavePossibleUserPersonalVocalRequest(gatewayEvent), ct);
            
            return Result.FromSuccess();
        }
        
        await _mediator.Send(new JoinPossibleVocalCreationRequest(gatewayEvent), ct);
        
        return Result.FromSuccess();
    }
}