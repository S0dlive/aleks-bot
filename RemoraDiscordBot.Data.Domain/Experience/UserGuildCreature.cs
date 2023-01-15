// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace RemoraDiscordBot.Data.Domain.Experience;

public class UserGuildCreature
{
    private readonly IEnumerable<string> _allCreatureTypes = new[]
    {
        "Snake",
        "Cat",
        "Cow"
    };

    public UserGuildCreature()
    {
        CreatureType = GetCreatureType();
    }

    [Key] public int Id { get; set; }

    public string CreatureType { get; set; }

    public bool IsEgg { get; set; } = true;

    public int Level { get; set; } = 0;
    public long PossessorId { get; set; }
    public long PossessorGuildId { get; set; }
    public UserGuildXp Possessor { get; set; }

    private string GetCreatureType()
    {
        return _allCreatureTypes.ElementAt(new Random().Next(0, _allCreatureTypes.Count()));
    }
}