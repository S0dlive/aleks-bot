// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Drawing;
using MediatR;
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

namespace RemoraDiscordBot.Plugins.Experience.CommandGroups;

[Group("experience")]
[Description("Plugin to manage the experience feature.")]
public class ExperienceCommandGroup
    : CommandGroup
{
    private readonly ICommandContext _commandContext;
    private readonly FeedbackService _feedbackService;
    private readonly IMediator _mediator;
    private readonly IDiscordRestUserAPI _userApi;

    public ExperienceCommandGroup(
        ICommandContext commandContext,
        IMediator mediator,
        IDiscordRestUserAPI userApi,
        FeedbackService feedbackService)
    {
        _commandContext = commandContext;
        _mediator = mediator;
        _userApi = userApi;
        _feedbackService = feedbackService;
    }

    [Command("amount")]
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