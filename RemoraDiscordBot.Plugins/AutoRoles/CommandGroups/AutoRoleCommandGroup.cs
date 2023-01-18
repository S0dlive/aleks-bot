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
using RemoraDiscordBot.Plugins.AutoRoles.Commands;

namespace RemoraDiscordBot.Plugins.AutoRoles.CommandGroups;

[Group("autoroles")]
[Description("Commands for managing autoroles.")]
public class AutoRoleCommandGroup
    : CommandGroup
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly ICommandContext _context;
    private readonly FeedbackService _feedbackService;
    private readonly IMediator _mediator;

    public AutoRoleCommandGroup(
        IMediator mediator,
        IDiscordRestChannelAPI channelApi,
        ICommandContext context,
        FeedbackService feedbackService)
    {
        _mediator = mediator;
        _channelApi = channelApi;
        _context = context;
        _feedbackService = feedbackService;
    }

    [Command("create")]
    [Description("Creates a new autorole message.")]
    [Ephemeral]
    // create <message> [<channelId>]
    public async Task<Result> CreateAutoRoleAsync(string message, Snowflake? channelId = null)
    {
        if (!_context.TryGetChannelID(out var currentChannelId))
            throw new InvalidOperationException("Could not get channel ID.");

        if (!_context.TryGetGuildID(out var guildId))
            throw new InvalidOperationException("Could not get guild ID.");

        var targetChannelId = channelId ?? currentChannelId;

        var channelJustCreated =
            await _mediator.Send(new CreateAutoRoleCommand(message, targetChannelId.Value, guildId.Value));

        return channelJustCreated switch
        {
            true => (Result) await _feedbackService.SendContextualSuccessAsync("AutoRole message created."),
            false => (Result) await _feedbackService.SendContextualErrorAsync("AutoRole message already exists.")
        };
    }

    [Command("remove")]
    [Description("Removes an autorole message.")]
    // remove <messageId>
    public async Task<Result> RemoveAutoRoleAsync(Snowflake messageId)
    {
        return Result.FromSuccess();
    }
}