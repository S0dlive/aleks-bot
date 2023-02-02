// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Rest.Core;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data.Domain.PersonalVocal;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;
using RemoraDiscordBot.Plugins.PersonalVocal.Model;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class CreatePersonalUserVocalChannelRequestHandler
    : IRequestHandler<CreatePersonalUserVocalChannelRequest, Tuple<UserVocalChannel, Snowflake>>
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly IDiscordRestGuildAPI _guildApi;
    private readonly IDiscordRestUserAPI _userApi;
    private readonly ILogger<CreatePersonalUserVocalChannelRequestHandler> _logger;

    public CreatePersonalUserVocalChannelRequestHandler(
        IDiscordRestChannelAPI channelApi,
        IDiscordRestGuildAPI guildApi,
        IDiscordRestUserAPI userApi, 
        ILogger<CreatePersonalUserVocalChannelRequestHandler> logger)
    {
        _channelApi = channelApi;
        _guildApi = guildApi;
        _userApi = userApi;
        _logger = logger;
    }

    public async Task<Tuple<UserVocalChannel, Snowflake>> Handle(CreatePersonalUserVocalChannelRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to create a channel for user {UserId} in guild {GuildId}", request.UserId, request.GuildId);
        
        var user = await _userApi.GetUserAsync(request.UserId, cancellationToken);

        if (user.Entity.IsBot is {HasValue: true, Value: true})
            throw new InvalidOperationException("Cannot create a channel for a bot.");

        var channel = await _guildApi.CreateGuildChannelAsync(
            request.GuildId,
            $"Salon de {user.Entity.Username}",
            ChannelType.GuildVoice,
            parentID: request.CategoryId,
            ct: cancellationToken);

        if (!channel.IsSuccess)
        {
            throw new InvalidOperationException("Cannot create a channel for reason: " + channel.Inner?.Error?.Message);
        }

        var editChannelPermissionsAsync = await _channelApi.EditChannelPermissionsAsync(
            channel.Entity.ID,
            request.UserId,
            new DiscordPermissionSet(DiscordPermission.Administrator),
            type: PermissionOverwriteType.Member,
            ct: cancellationToken);

        if (!editChannelPermissionsAsync.IsSuccess)
        {
            throw new InvalidOperationException("Cannot create a channel for reason: " + editChannelPermissionsAsync.Error.Message);
        }

        var modifyGuildMemberAsync = await _guildApi.ModifyGuildMemberAsync(
            request.GuildId,
            request.UserId,
            channelID: channel.Entity.ID,
            ct: cancellationToken);
            
        if (!modifyGuildMemberAsync.IsSuccess)
        {
            throw new InvalidOperationException("Cannot create a channel for reason: " + modifyGuildMemberAsync.Error.Message);
        }
        
        var userVocalChannel = new UserVocalChannel
        {
            UserId = request.UserId,
            GuildId = request.GuildId
        };
        
        return new Tuple<UserVocalChannel, Snowflake>(userVocalChannel, channel.Entity.ID);
    }
}