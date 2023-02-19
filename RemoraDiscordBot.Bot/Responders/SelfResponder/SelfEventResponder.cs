// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Discord.API.Gateway.Events;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using RemoraDiscordBot.Business.Colors;

namespace RemoraDiscordBot.Core.Responders.SelfResponder;

public sealed class SelfEventResponder
    : IResponder<MessageCreate>
{
    private readonly FeedbackService _feedbackService;

    public SelfEventResponder(FeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    public async Task<Result> RespondAsync(
        MessageCreate gatewayEvent,
        CancellationToken ct = default)
    {
        var message = gatewayEvent.Content;

        if (gatewayEvent.Author.IsBot is {Value: true, HasValue: true})
        {
            return Result.FromSuccess();
        }

        if (message.Equals($"<@!{gatewayEvent.Author.ID}>"))
        {
            return (Result) await _feedbackService.SendContentAsync
            (
                gatewayEvent.ChannelID,
                "Hey there! I am Aleks!",
                DiscordTransparentColor.Value,
                ct: ct
            );
        }

        return Result.FromSuccess();
    }
}