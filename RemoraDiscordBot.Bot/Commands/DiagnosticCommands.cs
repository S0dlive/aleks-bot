// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Drawing;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;

namespace RemoraDiscordBot.Core.Commands;

public class DiagnosticCommands
    : CommandGroup
{
    private readonly IDiscordRestChannelAPI _channelAPI;
    private readonly FeedbackService _feedbackService;

    public DiagnosticCommands(
        IDiscordRestChannelAPI channelApi,
        FeedbackService feedbackService)
    {
        _channelAPI = channelApi;
        _feedbackService = feedbackService;
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
}