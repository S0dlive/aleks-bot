﻿// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Rest.Core;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Queries;

public sealed record GetUniqueGuildVocalChannelRequest(Snowflake GuildId) 
    : IRequest<Data.Domain.PersonalVocal.PersonalVocal?>;