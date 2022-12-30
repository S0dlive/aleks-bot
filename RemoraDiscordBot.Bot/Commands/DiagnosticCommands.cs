﻿// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Drawing;
using Microsoft.Extensions.Logging;
using OneOf;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Feedback.Messages;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;

namespace RemoraDiscordBot.Core.Commands;

public sealed class DiagnosticCommands
    : CommandGroup
{
    private readonly FeedbackService _feedbackService;
    private readonly HttpClient _httpClient;
    private readonly ILogger<DiagnosticCommands> _logger;

    public DiagnosticCommands(
        FeedbackService feedbackService,
        HttpClient httpClient,
        ILogger<DiagnosticCommands> logger)
    {
        _feedbackService = feedbackService;
        _httpClient = httpClient;
        _logger = logger;
    }

    [Command("hello")]
    [Description("Says hello to the user with a provided message argument.")]
    public async Task<IResult> HelloCommand(
        [Description("The following message that the bot will send")]
        string message)
    
    {
        //TODO: Refactor to service
        var response = await _httpClient.GetAsync("http://localhost:5106/api/v1/Egg");
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get the egg from the API");
            return Result.FromSuccess();
        }

        var content = await response.Content.ReadAsStreamAsync();

        var fileData = new FileData("egg.png", content, "image/png");

        return (Result) await _feedbackService.SendContextualEmbedAsync(
            new Embed
            {
                Title = "Hello!",
                Description = message,
                Colour = Color.Green,
                Image = new EmbedImage("attachment://egg.png")
            },
            new FeedbackMessageOptions
            {
                Attachments = new[]
                {
                    OneOf<FileData, IPartialAttachment>.FromT0(fileData)
                }
            },
            CancellationToken);
    }
}