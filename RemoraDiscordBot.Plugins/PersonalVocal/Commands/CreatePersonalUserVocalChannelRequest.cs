// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Rest.Core;
using RemoraDiscordBot.Data.Domain.PersonalVocal;
using RemoraDiscordBot.Plugins.PersonalVocal.Model;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Commands;

public sealed record CreatePersonalUserVocalChannelRequest(
        Snowflake UserId,
        Snowflake GuildId,
        Snowflake CategoryId)
    : IRequest<Tuple<UserVocalChannel, Snowflake>>;