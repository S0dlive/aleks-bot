// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Drawing;
using MediatR;
using Microsoft.Extensions.Logging;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;
using RemoraDiscordBot.Plugins.Experience.Queries;

namespace RemoraDiscordBot.Core.Commands;

public sealed class DiagnosticCommands
    : CommandGroup
{
    private readonly ICommandContext _commandContext;
    private readonly FeedbackService _feedbackService;
    private readonly IMediator _mediator;
    private readonly ILogger<DiagnosticCommands> _logger;

    public DiagnosticCommands(
        FeedbackService feedbackService,
        ICommandContext commandContext,
        IMediator mediator, 
        ILogger<DiagnosticCommands> logger)
    {
        _feedbackService = feedbackService;
        _commandContext = commandContext;
        _mediator = mediator;
        _logger = logger;
    }

    [Command("hello")]
    [Description("Says hello to the user with a provided message argument.")]
    public async Task<IResult> HelloCommand(
        [Description("The following message that the bot will send")]
        string message)
    {
        return (Result) await _feedbackService.SendContextualEmbedAsync(
            new Embed
            {
                Title = "Hello!",
                Description = message,
                Colour = Color.Green
            },
            ct: CancellationToken);
    }

    [Command("xp")]
    [Description("Gets the amount of XP you have.")]
    public async Task<IResult> XpCommandAsync()
    {
        var guildId = _commandContext.TryGetGuildID(out var id)
            ? id
            : throw new InvalidOperationException("This command can only be used in a guild (guildId).");

        var userId = _commandContext.TryGetUserID(out var uid)
            ? uid
            : throw new InvalidOperationException("This command can only be used in a guild (userId).");
        
        _logger.LogInformation("Getting XP for user {UserId} in guild {GuildId}", userId, guildId);

        var xp = await _mediator.Send(
            new GetExperienceAmountByUserQuery(userId.Value, guildId.Value), CancellationToken);
        
        return (Result) await _feedbackService.SendContextualEmbedAsync(
            new Embed
            {
                Title = "Hello!",
                Description = guildId.Value.ToString(),
                Colour = Color.Green
            },
            ct: CancellationToken);
    }
}