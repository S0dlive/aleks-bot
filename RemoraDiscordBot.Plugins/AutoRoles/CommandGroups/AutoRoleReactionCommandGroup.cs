// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Rest.Core;
using Remora.Results;

namespace RemoraDiscordBot.Plugins.AutoRoles.CommandGroups;

[Group("reaction")]
public class AutoRoleReactionCommandGroup
    : AutoRoleCommandGroup
{

    [Command("add")]
    [Description("Add a reaction to an auto-role message.")]
    public async Task<Result> AddReactionAsync
    (
        [Description("The message ID of the auto-role message.")]
        Snowflake messageID,
        [Description("The emoji to react with.")]
        IEmoji emoji,
        [Description("The role to give when the reaction is added.")]
        Snowflake roleID,
        [Description("The label to add to the reaction.")]
        string label
    )
    {
        return Result.FromSuccess();
    }

    public AutoRoleReactionCommandGroup(IMediator mediator, IDiscordRestChannelAPI channelApi, ICommandContext context, FeedbackService feedbackService) : base(mediator, channelApi, context, feedbackService)
    {
    }
}