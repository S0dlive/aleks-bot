// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

namespace RemoraDiscordBot.Data.Domain.AutoRoles;

public class AutoRoleChannel
{
    public AutoRoleChannel(
        long messageId,
        long guildId)
    {
        MessageId = messageId;
        GuildId = guildId;
    }

    public long MessageId { get; set; }
    public long GuildId { get; set; }
    public IEnumerable<AutoRoleReaction> Reactions { get; set; } = new List<AutoRoleReaction>();
}