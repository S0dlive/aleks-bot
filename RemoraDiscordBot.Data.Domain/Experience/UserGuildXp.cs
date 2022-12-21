// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace RemoraDiscordBot.Data.Domain.Xp;

public class UserGuildXp
{
    public UserGuildXp(ulong userId, ulong guildId)
    {
        UserId = userId;
        GuildId = guildId;
        XpAmount = 0;
        Level = 0;
        XpNeededToLevelUp = 0;
    }

    [Key] public ulong UserId { get; set; }

    public ulong GuildId { get; set; }

    public ulong XpAmount { get; set; }

    public ulong Level { get; set; }

    public ulong XpNeededToLevelUp { get; set; }
}