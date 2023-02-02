// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Remora.Rest.Core;
using RemoraDiscordBot.Plugins.PersonalVocal.Commands;
using RemoraDiscordBot.Plugins.PersonalVocal.Model;
using RemoraDiscordBot.Plugins.PersonalVocal.Services;

namespace RemoraDiscordBot.Plugins.PersonalVocal.Handlers.Commands;

public sealed class PersistUserVocalChannelRequestHandler
    : IRequestHandler<PersistUserVocalChannelRequest, Tuple<UserVocalChannel, Snowflake>>
{
    private readonly IPersonalVocalService _personalVocalService;

    public PersistUserVocalChannelRequestHandler(
        IPersonalVocalService personalVocalService)
    {
        _personalVocalService = personalVocalService;
    }

    public async Task<Tuple<UserVocalChannel, Snowflake>> Handle(
        PersistUserVocalChannelRequest request,
        CancellationToken cancellationToken)
    {
        if (_personalVocalService.HasVoiceChannel(request.UserId, request.GuildId))
        {
            _personalVocalService.DeleteVoiceChannel(request.UserId, request.GuildId);
        }

        _personalVocalService.CreateVoiceChannel(request.UserId, request.GuildId, request.ChannelId);

        return new Tuple<UserVocalChannel, Snowflake>(
            new UserVocalChannel {UserId = request.UserId, GuildId = request.GuildId},
            request.ChannelId);
    }
}