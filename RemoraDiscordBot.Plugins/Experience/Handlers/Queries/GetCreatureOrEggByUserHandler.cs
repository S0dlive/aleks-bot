// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Rest;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.Experience.Queries;

namespace RemoraDiscordBot.Plugins.Experience.Handlers;

public sealed record GetCreatureOrEggByUserHandler(
        RemoraDiscordBotDbContext DbContext,
        HttpClient HttpClient,
        IConfiguration Configuration,
        ILogger<GetCreatureOrEggByUserHandler> Logger)
    : IRequestHandler<GetCreatureOrEggByUserQuery, FileData>
{
    public async Task<FileData> Handle(
        GetCreatureOrEggByUserQuery request,
        CancellationToken cancellationToken)
    {
        var userGuildXp = await DbContext.UserGuildXps
            .Where(x => x.UserId == request.UserId.ToLong())
            .Where(x => x.GuildId == request.GuildId.ToLong())
            .Include(x => x.Creature)
            .FirstOrDefaultAsync(cancellationToken);

        if (userGuildXp is null) return new FileData("image/png", Stream.Null);

        var baseAddress = Configuration["Api:BaseUrl"]
                          ?? throw new InvalidOperationException("Api:BaseUrl is not set in configuration");


        var url = userGuildXp.Creature.IsEgg switch
        {
            true => $"http://{baseAddress}:5106/api/v1/Egg?Type={userGuildXp.Creature.CreatureType}&Cracks={userGuildXp.Creature.Level}",
            false => $"http://{baseAddress}:5106/api/v1/Creature?Type={userGuildXp.Creature.CreatureType}&Age={userGuildXp.Creature.Level}"
        };

        Logger.LogInformation("Calling {Url}", url);

        var response = await HttpClient.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode) return new FileData("image/png", Stream.Null);

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return new FileData("file.png", stream);
    }
}