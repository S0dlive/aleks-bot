// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Rest.Core;

namespace RemoraDiscordBot.Business.Extensions;

public static class UserSnowflakeMentionConverterExtension
{
    public static string ToMention(this Snowflake userSnowflake)
    {
        return $"<@{userSnowflake.Value}>";
    }
}