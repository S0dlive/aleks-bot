// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using MediatR;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Experience;
using RemoraDiscordBot.Plugins.Experience.Queries;

namespace RemoraDiscordBot.Plugins.Experience.Handlers;

public sealed record GetGlobalLeaderBoardHandler(RemoraDiscordBotDbContext DbContext)
    : IRequestHandler<GetGlobalLeaderBoardQuery, IEnumerable<UserGuildXp>>
{
    public async Task<IEnumerable<UserGuildXp>> Handle(GetGlobalLeaderBoardQuery request, CancellationToken cancellationToken)
    {
        var leaderBoard = DbContext.UserGuildXps
            .OrderByDescending(x => x.Level)
            .ToList();

        return new ReadOnlyCollection<UserGuildXp>(leaderBoard);
    }
}