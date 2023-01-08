// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Messages;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.Welcomer.Commands;
using RemoraDiscordBot.Plugins.Welcomer.Queries;

namespace RemoraDiscordBot.Plugins.Welcomer.CommandGroups;

[Group("welcomer")]
[Description("Welcomer commands")]
public class WelcomerConfigurationCommandGroup
    : CommandGroup
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly FeedbackService _feedbackService;
    private readonly ICommandContext _commandContext;
    private readonly IMediator _mediator;

    public WelcomerConfigurationCommandGroup(
        ICommandContext commandContext,
        IMediator mediator,
        FeedbackService feedbackService,
        RemoraDiscordBotDbContext dbContext)
    {
        _mediator = mediator;
        _feedbackService = feedbackService;
        _dbContext = dbContext;
        _commandContext = commandContext;
    }

    [Command("setwelcomechannel")]
    [Description("Set the welcome channel where you are, and enable the welcomer feature.")]
    public async Task<IResult> SetWelcomeChannelAsync()
    {
        if (!_commandContext.TryGetChannelID(out var channelId))
            throw new InvalidOperationException("Cannot get channel ID from interaction context.");

        if (!_commandContext.TryGetGuildID(out var guildId))
            throw new InvalidOperationException("Cannot get guild ID from interaction context.");

        await _mediator.Send(new CreateOrSetWelcomeChannelCommand(guildId.Value, channelId.Value));

        return (Result) await _feedbackService.SendContextualSuccessAsync(
            $"Welcome channel set to <#{channelId.Value}>",
            options: new FeedbackMessageOptions
            {
                MessageFlags = MessageFlags.Ephemeral
            });
    }

    [Command("disable")]
    [Description("Disable the welcomer feature.")]
    public async Task<IResult> DisableAsync()
    {
        if (!_commandContext.TryGetGuildID(out var guildId))
            throw new InvalidOperationException("Cannot get guild ID from interaction context.");

        var guild = await _mediator.Send(new GetsIfGuildAlreadyRegisteredQuery(guildId.Value));

        guild.WelcomeChannelId = null;
        await _dbContext.SaveChangesAsync();

        return (Result) await _feedbackService.SendContextualSuccessAsync(
            "Welcomer disabled.",
            options: new FeedbackMessageOptions
            {
                MessageFlags = MessageFlags.Ephemeral
            });
    }
}