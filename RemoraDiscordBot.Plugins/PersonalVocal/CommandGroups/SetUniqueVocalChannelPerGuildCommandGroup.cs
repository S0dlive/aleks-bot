// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Rest.Core;
using Remora.Results;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;

namespace RemoraDiscordBot.Plugins.PersonalVocal.CommandGroups;

[Group("personal-vocal")]
[Description("Personal Vocal Commands")]
public class SetUniqueVocalChannelPerGuildCommandGroup
    : CommandGroup
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly ICommandContext _commandContext;
    private readonly FeedbackService _feedbackService;
    private readonly IMediator _mediator;

    public SetUniqueVocalChannelPerGuildCommandGroup(
        IMediator mediator,
        ICommandContext commandContext,
        FeedbackService feedbackService,
        IDiscordRestChannelAPI channelApi)
    {
        _mediator = mediator;
        _commandContext = commandContext;
        _feedbackService = feedbackService;
        _channelApi = channelApi;
    }

    [Command("set")]
    [Description("Set the unique vocal channel for the current guild")]
    [Ephemeral]
    public async Task<Result> SetUniqueVocalChannelPerGuildAsync(Snowflake channelId)
    {
        if (!_commandContext.TryGetGuildID(out var guildId))
            throw new InvalidOperationException("The command must be executed in a guild");

        var channel = await _channelApi.GetChannelAsync(channelId, CancellationToken.None);

        await _mediator.Send(new SetUniqueVocalChannelPerGuildRequest(channelId, guildId.Value));

        return (Result) await _feedbackService.SendContextualSuccessAsync(
            $"The unique vocal channel has been set to **{channel.Entity.Name}**");
    }
}