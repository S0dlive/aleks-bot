// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest.Core;

namespace RemoraDiscordBot.Plugins.Experience.Queries;

public sealed record GetCreatureOrEggByUserQuery(Snowflake UserId, Snowflake GuildId)
    : IRequest<FileData>;