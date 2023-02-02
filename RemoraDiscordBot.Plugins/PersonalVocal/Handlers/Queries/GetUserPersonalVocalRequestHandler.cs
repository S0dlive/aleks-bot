// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Rest.Core;
using RemoraDiscordBot.Plugins.PersonalVocal.Model;
using RemoraDiscordBot.Plugins.PersonalVocal.Queries;
using RemoraDiscordBot.Plugins.PersonalVocal.Services;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Queries;

public sealed record GetUserPersonalVocalRequestHandler(IPersonalVocalService PersonalVocalService)
    : IRequestHandler<GetUserPersonalVocalRequest, Tuple<UserVocalChannel, Snowflake>?>
{
    public async Task<Tuple<UserVocalChannel, Snowflake>?> Handle(
        GetUserPersonalVocalRequest request,
        CancellationToken cancellationToken)
    {
        return PersonalVocalService.GetVoiceChannel(request.UserId, request.GuildId);
    }
}