﻿// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace RemoraDiscordBot.Data.Domain.Xp;

public class UserGuildXp
{
    [Key] public long UserId { get; set; }

    public long GuildId { get; set; }

    public long XpAmount { get; set; }

    public long Level { get; set; }

    public long XpNeededToLevelUp { get; set; }
}