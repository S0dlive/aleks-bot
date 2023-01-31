// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Discord.API.Abstractions.Gateway.Events;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Commands;

public sealed record LeavePossibleUserPersonalVocalRequest(IVoiceStateUpdate GatewayEvent)
    : IRequest;