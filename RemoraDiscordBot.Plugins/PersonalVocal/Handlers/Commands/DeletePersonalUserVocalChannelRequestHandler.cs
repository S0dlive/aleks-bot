// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Rest;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class DeletePersonalUserVocalChannelRequestHandler
    : AsyncRequestHandler<DeletePersonalUserVocalChannelRequest>
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly ILogger<DeletePersonalUserVocalChannelRequestHandler> _logger;

    public DeletePersonalUserVocalChannelRequestHandler(
        IDiscordRestChannelAPI channelApi,
        ILogger<DeletePersonalUserVocalChannelRequestHandler> logger)
    {
        _channelApi = channelApi;
        _logger = logger;
    }

    protected override async Task Handle(
        DeletePersonalUserVocalChannelRequest request,
        CancellationToken cancellationToken)
    {
        var deleteChannelAsync = await _channelApi.DeleteChannelAsync(
            request.ChannelId,
            ct: cancellationToken);

        if (!deleteChannelAsync.IsSuccess)
        {
            throw new InvalidOperationException("Cannot delete user vocal channel, reason: " +
                                                deleteChannelAsync.Error.Message);
        }

        _logger.LogInformation("Deleted personal vocal channel {ChannelId}", request.ChannelId);
    }
}