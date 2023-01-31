// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Plugins.PersonalVocal.Queries;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Responders;

public class JoinPossibleVocalCreationResponder
    : IResponder<IVoiceStateUpdate>
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly IDiscordRestGuildAPI _guildApi;
    private readonly ILogger<JoinPossibleVocalCreationResponder> _logger;
    private readonly IMediator _mediator;
    private readonly IDiscordRestUserAPI _userApi;

    public JoinPossibleVocalCreationResponder(
        IMediator mediator,
        ILogger<JoinPossibleVocalCreationResponder> logger,
        IDiscordRestChannelAPI channelApi,
        IDiscordRestUserAPI userApi,
        IDiscordRestGuildAPI guildApi)
    {
        _mediator = mediator;
        _logger = logger;
        _channelApi = channelApi;
        _userApi = userApi;
        _guildApi = guildApi;
    }

    public async Task<Result> RespondAsync(
        IVoiceStateUpdate gatewayEvent,
        CancellationToken ct = default)
    {

        if (gatewayEvent.ChannelID == null)
        {
            var channelAsync = await _channelApi.GetChannelAsync(gatewayEvent.ChannelID.Value, ct);

            if (channelAsync.IsSuccess && channelAsync.Entity.Name is { HasValue: true } name && name.Value.Contains("'s channel"))
            {
                var guildAsync = await _guildApi.GetGuildAsync(gatewayEvent.GuildID.Value, ct:ct);

                if (guildAsync.IsSuccess)
                {
                    var guildAsyncEntity = guildAsync.Entity;

                    var userAsync = await _userApi.GetCurrentUserAsync(ct);

                    if (userAsync.IsSuccess)
                    {
                        var userAsyncEntity = userAsync.Entity;

                        var isOwner = guildAsyncEntity.OwnerID == userAsyncEntity.ID;

                        if (isOwner)
                        {
                            var deleteChannelAsync = await _channelApi.DeleteChannelAsync(gatewayEvent.ChannelID.Value, ct: ct);

                            if (deleteChannelAsync.IsSuccess)
                            {
                                _logger.LogInformation("Deleted channel {channelName} ({channelId}) because the owner left it", name, gatewayEvent.ChannelID.Value);
                            }
                            else
                            {
                                _logger.LogError("Failed to delete channel {channelName} ({channelId}) because the owner left it", name, gatewayEvent.ChannelID.Value);
                            }
                        }
                        else
                        {
                            var isOwnerOfChannel = name.Value.StartsWith(userAsyncEntity.Username);

                            if (isOwnerOfChannel)
                            {
                                var deleteChannelAsync = await _channelApi.DeleteChannelAsync(gatewayEvent.ChannelID.Value, ct: ct);

                                if (deleteChannelAsync.IsSuccess)
                                {
                                    _logger.LogInformation("Deleted channel {channelName} ({channelId}) because the owner left it", name, gatewayEvent.ChannelID.Value);
                                }
                                else
                                {
                                    _logger.LogError("Failed to delete channel {channelName} ({channelId}) because the owner left it", name, gatewayEvent.ChannelID.Value);
                                }
                            }
                        }
                    }
                }
            }
            {
                var deleteChannelAsync = await _channelApi.DeleteChannelAsync(channelAsync.Entity.ID, ct:ct);
                if (deleteChannelAsync.IsSuccess)
                {
                    _logger.LogInformation("Deleted channel {channelName} because it was empty", channelAsync.Entity.Name);
                }
                else
                {
                    _logger.LogError("Failed to delete channel {channelName} because it was empty", channelAsync.Entity.Name);
                }
            }
            
        }
        
        var channel = gatewayEvent.ChannelID.GetValueOrDefault();
        var _ = gatewayEvent.GuildID.TryGet(out var guild)
            ? guild
            : throw new InvalidOperationException("Guild ID is null");
        var isVocalChannelCreation = await _mediator.Send(new GetUniqueGuildVocalChannelRequest(guild), ct);

        if (isVocalChannelCreation is null || isVocalChannelCreation.ChannelId.ToSnowflake() != channel)
            return Result.FromSuccess();

        _logger.LogInformation("Vocal channel creation detected, moving user to it");

        var user = await _userApi.GetUserAsync(gatewayEvent.UserID, ct: ct);

        if (!user.IsSuccess)
            throw new InvalidOperationException("Unable to get current user because of an error : " + user.Error.Message);

        var channelName = $"{user.Entity.Username}'s channel";

        var channelCreation = await _guildApi.CreateGuildChannelAsync(
            guild,
            channelName,
            ChannelType.GuildVoice,
            ct: ct);

        if (!channelCreation.IsSuccess) throw new InvalidOperationException("Unable to create channel with exception : " + channelCreation.Error.Message);

        var moveUser = await _guildApi.ModifyGuildMemberAsync(
            guild,
            user.Entity.ID,
            channelID: channelCreation.Entity.ID,
            ct: ct);

        if (!moveUser.IsSuccess) throw new InvalidOperationException("Unable to move user to channel with exception : " + moveUser.Inner?.Error?.Message);

        return Result.FromSuccess();
    }
}