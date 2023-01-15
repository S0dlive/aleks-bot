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

        var level = userGuildXp.Creature.Level >= 5 ? 5 : userGuildXp.Creature.Level;

        if (userGuildXp.Creature is {IsEgg: true, Level: >= 4})
        {
            userGuildXp.Creature.Level = 1;
            userGuildXp.Creature.IsEgg = false;
            
            await DbContext.SaveChangesAsync(cancellationToken);
        }
        
        var url = userGuildXp.Creature.IsEgg switch
        {
            true => $"http://{baseAddress}:5106/api/v1/Egg?Type={userGuildXp.Creature.CreatureType}&Cracks={level}",
            false => $"http://{baseAddress}:5106/api/v1/Creature?Type={userGuildXp.Creature.CreatureType}&Age={level}"
        };

        Logger.LogInformation("Calling {Url}", url);

        var response = await HttpClient.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode) return new FileData("image/png", Stream.Null);

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return new FileData("file.png", stream);
    }
}