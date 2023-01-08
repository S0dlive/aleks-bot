// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;

namespace RemoraDiscordBot.Data.Domain.Welcomer;

public class WelcomerGuild
{
    [Key] public long GuildId { get; set; }

    public bool IsWelcomerEnabled { get; set; } = false;

    public string? WelcomeMessage { get; set; } = "Welcome to the server, %user%!";

    public long? WelcomeChannelId { get; set; }
}