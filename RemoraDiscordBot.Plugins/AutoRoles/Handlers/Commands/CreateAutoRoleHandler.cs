// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.Collections.Immutable;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Feedback.Messages;
using Remora.Discord.Commands.Feedback.Services;
using RemoraDiscordBot.Business.Colors;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.AutoRoles;
using RemoraDiscordBot.Plugins.AutoRoles.Commands;

namespace RemoraDiscordBot.Plugins.AutoRoles.Handlers.Commands;

public sealed class CreateAutoRoleHandler
    : IRequestHandler<CreateAutoRoleCommand, bool>
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly ILogger<CreateAutoRoleHandler> _logger;
    private readonly FeedbackService _feedbackService;

    public CreateAutoRoleHandler(
        RemoraDiscordBotDbContext dbContext,
        ILogger<CreateAutoRoleHandler> logger,
        IDiscordRestChannelAPI channelApi, 
        FeedbackService feedbackService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _channelApi = channelApi;
        _feedbackService = feedbackService;
    }

    public async Task<bool> Handle(CreateAutoRoleCommand request, CancellationToken cancellationToken)
    {
        var alreadyCreated = await _dbContext.AutoRoleChannels
            .FirstOrDefaultAsync(
                x => x.GuildId == request.GuildId.ToLong() && x.ChannelId == request.ChannelId.ToLong(),
                cancellationToken);

        if (alreadyCreated is not null)
        {
            _logger.LogWarning("AutoRoleChannel already created for guild {GuildId} and channel {ChannelId}",
                request.GuildId, request.ChannelId);
        
            await _feedbackService.SendContextualErrorAsync(
                "AutoRoleChannel already created for this channel.",
                options: new FeedbackMessageOptions
                {
                    MessageFlags = MessageFlags.Ephemeral
                },
                ct: cancellationToken); 

            return false;
        }

        var embed = new Embed(Description: request.Message, Colour: DiscordTransparentColor.Value);

        var selectMenu = new StringSelectComponent(
            "test",
            new List<ISelectOption>
            {
                new SelectOption("Option 1", "1"),
            },
            "Select an option",
            1,
            1);

        var message =
            await _channelApi.CreateMessageAsync(
                request.ChannelId,
                embeds: new[] {embed},
                components: new[]
                {
                    new ActionRowComponent(new[]
                    {
                        selectMenu
                    })
                },
                ct: cancellationToken);

        if (!message.IsSuccess)
        {
            _logger.LogError(
                "Failed to create message for AutoRoleChannel for guild {GuildId} and channel {ChannelId} with Error {Error}",
                request.GuildId,
                request.ChannelId,
                message.Inner?.Error?.Message);

            return false;
        }

        var autoRoleChannel = new AutoRoleChannel(message.Entity.ID.ToLong(), request.ChannelId.ToLong(),
            request.GuildId.ToLong());

        _dbContext.AutoRoleChannels.Add(autoRoleChannel);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}