﻿// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace RemoraDiscordBot.Data.Domain.Xp;

public class UserGuildXp
{
    private readonly IEnumerable<string> _allCreatureTypes = new[]
    {
        "Snake",
        "Cat"
    };

    public UserGuildXp(long userId, long guildId)
    {
        UserId = userId;
        GuildId = guildId;
        XpAmount = 0;
        Level = 0;
        XpNeededToLevelUp = CalculateExperienceNeeded(XpAmount, Level);
        AssociatedCreature = RandomCreature();
    }

    [Key] public long UserId { get; set; }

    public long GuildId { get; set; }

    public long XpAmount { get; set; }

    public long Level { get; set; }

    public long XpNeededToLevelUp { get; set; }

    public string AssociatedCreature { get; set; }

    private string RandomCreature()
    {
        return _allCreatureTypes.OrderBy(x => Guid.NewGuid()).First();
    }

    private static int CalculateExperienceNeeded(long xp, long level)
    {
        return (int) (100 * Math.Pow(level + 1, 2))
               - (int) xp;
    }
}