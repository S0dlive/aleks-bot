// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

namespace RemoraDiscordBot.Data.Domain.AutoRoles;

public class AutoRoleReaction
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Emoji { get; set; }
    public ulong RoleId { get; set; }
    public long InstigatorMessageId { get; set; }
    public long InstigatorGuildId { get; set; }
    public AutoRoleChannel AutoRoleChannel { get; set; }
}