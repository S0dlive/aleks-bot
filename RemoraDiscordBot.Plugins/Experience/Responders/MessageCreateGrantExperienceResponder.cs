// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using RemoraDiscordBot.Plugins.Experience.Commands;

namespace RemoraDiscordBot.Plugins.Experience.Responders;

public class MessageCreateGrantExperienceResponder
    : IResponder<IMessageCreate>
{
    private readonly ILogger<MessageCreateGrantExperienceResponder> _logger;
    private readonly IMediator _mediator;

    public MessageCreateGrantExperienceResponder(
        ILogger<MessageCreateGrantExperienceResponder> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<Result> RespondAsync(IMessageCreate gatewayEvent, CancellationToken ct = new())
    {
        var instigator = gatewayEvent.Author;
        var messageLength = gatewayEvent.Content.Length;

        _logger.LogInformation("Message from {User} with {MessageLength} characters", instigator, messageLength);
       
        await _mediator.Send(new GrantExperienceAmountToUserCommand(instigator.ID, messageLength + 1 * 2), ct);
    
        return Result.FromSuccess();
    }
}