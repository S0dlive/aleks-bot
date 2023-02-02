// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Remora.Rest.Core;
using RemoraDiscordBot.Plugins.PersonalVocal.Model;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Services;

public class PersonalVocalService
    : IPersonalVocalService
{
    private readonly ConcurrentDictionary<Snowflake, Snowflake> _currentUserVocalChannel;
    private readonly ILogger<PersonalVocalService> _logger;
    private readonly ConcurrentDictionary<UserVocalChannel, Snowflake> _userVocalChannel;

    public PersonalVocalService(ILogger<PersonalVocalService> logger)
    {
        _logger = logger;
        _currentUserVocalChannel = new ConcurrentDictionary<Snowflake, Snowflake>();
        _userVocalChannel = new ConcurrentDictionary<UserVocalChannel, Snowflake>();
    }

    public void JoinVoiceChannel(Snowflake userId, Snowflake channelId)
    {
        _currentUserVocalChannel.AddOrUpdate(userId, channelId, (_, _) => channelId);

        _logger.LogInformation("User {UserId} joined voice channel {ChannelId}", userId, channelId);
    }

    public void LeaveVoiceChannel(Snowflake userId)
    {
        _logger.LogInformation(
            "User {UserId} left voice channel {ChannelId}",
            userId,
            _currentUserVocalChannel[userId]);

        _currentUserVocalChannel.TryRemove(userId, out _);
    }

    public Snowflake? GetVoiceChannel(Snowflake userId)
    {
        return _currentUserVocalChannel.TryGetValue(userId, out var channelId)
            ? channelId
            : null;
    }

    public void CreateVoiceChannel(Snowflake userId, Snowflake guildId, Snowflake channelId)
    {
        _userVocalChannel.AddOrUpdate(new UserVocalChannel {UserId = userId, GuildId = guildId}, channelId,
            (_, _) => channelId);

        _logger.LogInformation("User {UserId} created voice channel {ChannelId} in guild {GuildId}", userId, channelId,
            guildId);
    }

    public void DeleteVoiceChannel(Snowflake userId, Snowflake guildId)
    {
        _userVocalChannel.TryRemove(new UserVocalChannel {UserId = userId, GuildId = guildId}, out _);

        _logger.LogInformation("User {UserId} deleted voice channel in guild {GuildId}", userId, guildId);
    }

    public bool HasVoiceChannel(Snowflake userId, Snowflake guildId)
    {
        return _userVocalChannel.ContainsKey(new UserVocalChannel {UserId = userId, GuildId = guildId});
    }

    public Tuple<UserVocalChannel, Snowflake>? GetVoiceChannel(Snowflake userId, Snowflake guildId)
    {
        return _userVocalChannel.TryGetValue(
            new UserVocalChannel {UserId = userId, GuildId = guildId},
            out var channelId)
            ? new Tuple<UserVocalChannel, Snowflake>(
                new UserVocalChannel {UserId = userId, GuildId = guildId},
                channelId)
            : null;
    }
}