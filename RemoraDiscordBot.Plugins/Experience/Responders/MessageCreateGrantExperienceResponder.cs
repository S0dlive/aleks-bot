// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.Collections.Immutable;
using MediatR;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using RemoraDiscordBot.Plugins.Experience.Commands;

namespace RemoraDiscordBot.Plugins.Experience.Responders;

public class MessageCreateGrantExperienceResponder
    : IResponder<IMessageCreate>
{
    private readonly IMediator _mediator;

    public MessageCreateGrantExperienceResponder(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result> RespondAsync(IMessageCreate gatewayEvent, CancellationToken ct = new())
    {
        if (gatewayEvent.Author.IsBot is {HasValue: true, Value: true}) return Result.FromSuccess();

        var instigator = gatewayEvent.Author;
        var messageLength = gatewayEvent.Content.Length;
        var words = gatewayEvent.Content.Split(' ').Distinct().ToImmutableArray();
        var xpEarned = (int)(Math.Pow(words.Length + messageLength, 2) / 1000);

        await _mediator.Send(new GrantExperienceAmountToUserCommand(instigator.ID, xpEarned), ct);

        return Result.FromSuccess();
    }
}