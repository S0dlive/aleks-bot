// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Rest.Core;
using RemoraDiscordBot.Data.Domain.Welcomer;

namespace RemoraDiscordBot.Plugins.Welcomer.Queries;

public sealed record GetsIfGuildAlreadyRegisteredQuery(Snowflake GuildId) 
    : IRequest<WelcomerGuild?>;