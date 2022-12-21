﻿// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Rest.Core;

namespace RemoraDiscordBot.Business.Extensions;

public static class SnowflakeConverterExtension
{
    public static Snowflake ToSnowflake(this ulong value)
    {
        return new Snowflake(value);
    }
}