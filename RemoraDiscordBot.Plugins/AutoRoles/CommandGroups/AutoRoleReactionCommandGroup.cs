// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Rest.Core;
using Remora.Results;
using RemoraDiscordBot.Plugins.AutoRoles.Commands;

namespace RemoraDiscordBot.Plugins.AutoRoles.CommandGroups;

[Group("reaction")]
[Description("Commands to manage reaction roles.")]
public class AutoRoleReactionCommandGroup
    : AutoRoleCommandGroup
{
    private readonly IMediator _mediator;
    private readonly ITextCommandContext _context;

    public AutoRoleReactionCommandGroup(
        IMediator mediator,
        IDiscordRestChannelAPI channelApi,
        ICommandContext context,
        FeedbackService feedbackService, 
        ITextCommandContext context1)
        : base(mediator, channelApi, context, feedbackService)
    {
        _mediator = mediator;
        _context = context1;
    }

    [Command("add")]
    [Description("Add a reaction to an auto-role message.")]
    public async Task<Result> AddReactionAsync(
        [Description("The message ID of the auto-role message.")]
        Snowflake messageID,
        [Description("The emoji to react with.")]
        IEmoji emoji,
        [Description("The role to give when the reaction is added.")]
        Snowflake roleID,
        [Description("The label to add to the reaction.")]
        string label)
    {
        if(!_context.TryGetChannelID(out var channelId))
            throw new InvalidOperationException("Could not get channel ID from context.");
        
        if (!_context.TryGetGuildID(out var guildId))
            throw new InvalidOperationException("Could not get guild ID from context.");
        
        await _mediator.Send(new AddReactionRoleRequest(channelId.Value, messageID, guildId.Value, emoji, roleID, label));
        
        return Result.FromSuccess();
    }
}