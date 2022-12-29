// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Rest.Core;

namespace RemoraDiscordBot.Business.Extensions;

public static class SnowflakeConverterExtension
{
    public static Snowflake ToSnowflake(this long value)
    {
        var ulongValue = unchecked((ulong)value);
        return new Snowflake(ulongValue);
    }

    public static long ToLong(this Snowflake value)
    {
        return long.TryParse(value.ToString(), out var result) ? result : 0;
    }
}