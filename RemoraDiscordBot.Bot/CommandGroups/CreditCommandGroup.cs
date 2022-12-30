// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;

namespace RemoraDiscordBot.Core.CommandGroups;

public class CreditCommandGroup
    : CommandGroup
{
    private readonly FeedbackService _feedbackService;

    public CreditCommandGroup(FeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [Command("credit")]
    [Description("Shows all the contributors to the bot.")]
    public async Task<Result> CreditCommandAync()
    {
        return (Result) await _feedbackService.SendContextualEmbedAsync(
            new Embed
            {
                Title = "Credits",
                Description = "This bot was made by Alexis Chân Gridel. " +
                              "It is licensed under the GNU General Public License v3.0. ",
                },
            ct: CancellationToken
        );
    }
}