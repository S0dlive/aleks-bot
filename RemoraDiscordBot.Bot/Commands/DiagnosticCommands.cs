// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Drawing;
using MediatR;
using Microsoft.Extensions.Logging;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;
using RemoraDiscordBot.Business.Colors;
using RemoraDiscordBot.Plugins.Experience.Queries;

namespace RemoraDiscordBot.Core.Commands;

public sealed class DiagnosticCommands
    : CommandGroup
{
    private readonly ICommandContext _commandContext;
    private readonly FeedbackService _feedbackService;
    private readonly IMediator _mediator;
    private readonly IDiscordRestUserAPI _userApi;

    public DiagnosticCommands(
        FeedbackService feedbackService,
        ICommandContext commandContext,
        IMediator mediator,
        ILogger<DiagnosticCommands> logger,
        IDiscordRestUserAPI userApi)
    {
        _feedbackService = feedbackService;
        _commandContext = commandContext;
        _mediator = mediator;
        _userApi = userApi;
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
    [Description("Gets the amount of XP you have or the passed user has.")]
    public async Task<IResult> XpCommandAsync(
        [Description("Argument user to get the experience amount from")]
        IUser? user = null)
    {
        if (!_commandContext.TryGetUserID(out var instigatorId))
            throw new InvalidOperationException("Could not get the user ID.");

        if (!_commandContext.TryGetGuildID(out var instigatorGuildId))
            throw new InvalidOperationException("Could not get the guild ID.");

        if (user is {IsBot.Value: true})
            return (Result) await _feedbackService.SendContextualEmbedAsync(
                new Embed
                {
                    Title = "Error",
                    Description = "You cannot get the XP of a bot.",
                    Colour = Color.Red
                },
                ct: CancellationToken);

        var userToCheck = user?.ID ?? instigatorId;
        var xp = await _mediator.Send(
            new GetExperienceAmountByUserQuery(userToCheck.Value, instigatorGuildId.Value),
            CancellationToken);

        var instigatorUser = await _userApi.GetUserAsync(instigatorId.Value, CancellationToken);

        return (Result) await _feedbackService.SendContextualEmbedAsync(
            new Embed
            {
                Title = $"Experience for {user?.Username ?? instigatorUser.Entity.Username}",
                Description = $"**{xp}** XP",
                Colour = DiscordTransparentColor.Value
            },
            ct: CancellationToken);
    }
}