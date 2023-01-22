// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest.Core;

namespace RemoraDiscordBot.Plugins.AutoRoles.Commands;

public sealed record AddReactionRoleRequest(Snowflake ChannelId, Snowflake MessageId, Snowflake GuildId, IEmoji Emoji, Snowflake RoleId, string Label)
    : IRequest;