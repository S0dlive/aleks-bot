// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Rest.Core;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Services;

public interface IPersonalVocalService
{
    void JoinVoiceChannel(Snowflake userId, Snowflake channelId);
    void LeaveVoiceChannel(Snowflake userId);
    
    Snowflake? GetVoiceChannel(Snowflake userId);
}