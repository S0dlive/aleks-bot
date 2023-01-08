// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Plugins.Welcomer.Queries;

namespace RemoraDiscordBot.Plugins.Welcomer.Responders;

public class UserJoinGuildNotifierResponder
    : IResponder<IGuildMemberAdd>
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly IMediator _mediator;

    public UserJoinGuildNotifierResponder(
        IDiscordRestChannelAPI channelApi,
        IMediator mediator)
    {
        _channelApi = channelApi;
        _mediator = mediator;
    }

    public async Task<Result> RespondAsync(
        IGuildMemberAdd gatewayEvent,
        CancellationToken ct = default)
    {
        var welcomer = await _mediator.Send(new GetsIfGuildAlreadyRegisteredQuery(gatewayEvent.GuildID), ct);

        if (welcomer.WelcomeChannelId is null)
            return Result.FromSuccess();

        var message = welcomer.WelcomeMessage
            .Replace("%user%", gatewayEvent.User.Value.ID.ToMention());

        var channel = await _channelApi.GetChannelAsync(welcomer.WelcomeChannelId.Value.ToSnowflake(), ct);

        return (Result) await _channelApi.CreateMessageAsync(
            channel.Entity.ID,
            message,
            ct: ct);
    }
}