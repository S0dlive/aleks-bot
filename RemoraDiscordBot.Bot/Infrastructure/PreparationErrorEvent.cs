// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Messages;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Commands.Services;
using Remora.Results;

namespace RemoraDiscordBot.Core.Infrastructure;

public class PreparationErrorEvent
    : IPreparationErrorEvent
{
    private readonly FeedbackService _feedbackService;
    private readonly ILogger<PreparationErrorEvent> _logger;

    public PreparationErrorEvent(
        ILogger<PreparationErrorEvent> logger,
        FeedbackService feedbackService)
    {
        _logger = logger;
        _feedbackService = feedbackService;
    }

    public async Task<Result> PreparationFailed(
        IOperationContext context,
        IResult preparationResult,
        CancellationToken ct = default)
    {
        if (preparationResult.IsSuccess) return Result.FromSuccess();
        if (!context.TryGetUserID(out var userId)) return Result.FromSuccess();

        return (Result) await _feedbackService.SendContextualErrorAsync(
            preparationResult.Inner?.Inner?.Error?.Message,
            userId.Value,
            new FeedbackMessageOptions
            {
                MessageFlags = MessageFlags.Ephemeral
            },
            ct);
    }
}