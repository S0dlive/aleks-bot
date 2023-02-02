// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using Remora.Rest.Core;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Services;

public class PersonalVocalService
    : IPersonalVocalService
{
    private readonly ConcurrentDictionary<Snowflake, Snowflake> _currentUserVocalChannel;

    public PersonalVocalService()
    {
        _currentUserVocalChannel = new ConcurrentDictionary<Snowflake, Snowflake>();
    }

    public void JoinVoiceChannel(Snowflake userId, Snowflake channelId)
    {
        _currentUserVocalChannel.AddOrUpdate(userId, channelId, (_, _) => channelId);
    }

    public void LeaveVoiceChannel(Snowflake userId)
    {
        _currentUserVocalChannel.TryRemove(userId, out _);
    }

    public Snowflake? GetVoiceChannel(Snowflake userId)
    {
        return _currentUserVocalChannel.TryGetValue(userId, out var channelId) 
            ? channelId 
            : null;
    }
}