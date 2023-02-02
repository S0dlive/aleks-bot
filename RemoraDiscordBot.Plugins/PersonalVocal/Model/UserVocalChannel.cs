// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Rest.Core;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Model;

public struct UserVocalChannel
{
    public Snowflake UserId { get; set; }
    public Snowflake GuildId { get; set; }
}