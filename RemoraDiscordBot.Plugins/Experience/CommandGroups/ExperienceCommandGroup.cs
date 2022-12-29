// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using MediatR;
using Microsoft.Extensions.Logging;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;
using RemoraDiscordBot.Business.Colors;
using RemoraDiscordBot.Business.Infrastructure.Attributes;
using RemoraDiscordBot.Plugins.Experience.Queries;

namespace RemoraDiscordBot.Plugins.Experience.CommandGroups;

[Group("experience")]
[Description("Plugin to manage the experience feature.")]
public class ExperienceCommandGroup
    : CommandGroup
{
    private readonly ICommandContext _commandContext;
    private readonly FeedbackService _feedbackService;
    private readonly ILogger<ExperienceCommandGroup> _logger;
    private readonly IMediator _mediator;
    private readonly IDiscordRestUserAPI _userApi;

    public ExperienceCommandGroup(
        ICommandContext commandContext,
        IMediator mediator,
        IDiscordRestUserAPI userApi,
        FeedbackService feedbackService,
        ILogger<ExperienceCommandGroup> logger)
    {
        _commandContext = commandContext;
        _mediator = mediator;
        _userApi = userApi;
        _feedbackService = feedbackService;
        _logger = logger;
    }

    [Command("amount")]
    [Description("Gets the amount of XP you have or the passed user has.")]
    public async Task<IResult> XpCommandAsync(
        [Description("Argument user to get the experience amount from")] [NoBot]
        IUser? user = null)
    {
        if (!_commandContext.TryGetUserID(out var instigatorId))
            throw new InvalidOperationException("Could not get the user ID.");

        if (!_commandContext.TryGetGuildID(out var instigatorGuildId))
            throw new InvalidOperationException("Could not get the guild ID.");

        var userToCheck = user?.ID ?? instigatorId;
        var xp = await _mediator.Send(
            new GetExperienceAmountByUserQuery(userToCheck.Value, instigatorGuildId.Value),
            CancellationToken);

        var instigatorUser = await _userApi.GetUserAsync(instigatorId.Value, CancellationToken);

        await _feedbackService.SendContextualEmbedAsync(
            new Embed
            {
                Title = $"Experience for {user?.Username ?? instigatorUser.Entity.Username}",
                Description = $"**{xp}** XP",
                Colour = DiscordTransparentColor.Value
            },
            ct: CancellationToken);

        return await Task.FromResult<IResult>(Result.FromSuccess());
    }

    [Command("profile")]
    [Description("Gets the profile of the user.")]
    public async Task<Result> ProfileCommandAsync(
        [Description("Argument user to get the experience amount from")] [NoBot]
        IUser? user = null)
    {
        if (!_commandContext.TryGetUserID(out var instigatorId))
            throw new InvalidOperationException("Could not get the user ID.");

        if (!_commandContext.TryGetGuildID(out var instigatorGuildId))
            throw new InvalidOperationException("Could not get the guild ID.");

        var userToCheck = user?.ID ?? instigatorId;
        var xp = await _mediator.Send(
            new GetExperienceAmountByUserQuery(userToCheck.Value, instigatorGuildId.Value),
            CancellationToken);
        var level = await _mediator.Send(
            new GetLevelByUserQuery(userToCheck.Value, instigatorGuildId.Value));
        var xpNeeded = await _mediator.Send(
            new GetExperienceNeededByUserQuery(userToCheck.Value, instigatorGuildId.Value));

        var instigatorUser = await _userApi.GetUserAsync(instigatorId.Value, CancellationToken);

        var embed = new Embed
        {
            Title = $"Profile for {user?.Username ?? instigatorUser.Entity.Username}",
            Colour = DiscordTransparentColor.Value,
            Thumbnail = new EmbedThumbnail
            (
                CDN.GetUserAvatarUrl(user ?? instigatorUser.Entity).Entity.AbsoluteUri
            ),
            Fields = new List<EmbedField>
            {
                new("Level", level.ToString(), true),
                new("XP", xp.ToString(), true),
                new("XP Needed", xpNeeded.ToString(), true),
            },
        };

        await _feedbackService.SendContextualEmbedAsync(embed, ct: CancellationToken);

        return Result.FromSuccess();
    }
}