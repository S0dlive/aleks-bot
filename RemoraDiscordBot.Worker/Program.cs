// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Core;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.AdvertisementGuard;
using RemoraDiscordBot.Plugins.AutoRoles;
using RemoraDiscordBot.Plugins.Experience;
using RemoraDiscordBot.Plugins.PersonalVocal;
using RemoraDiscordBot.Plugins.Welcomer;
using RemoraDiscordBot.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services
            .AddHostedService<Worker>()
            .AddDbContext<RemoraDiscordBotDbContext>(options =>
            {
                options
                    .UseMySql(
                        hostContext.Configuration["ConnectionStrings:DefaultConnection"],
                        ServerVersion.AutoDetect(hostContext.Configuration["ConnectionStrings:DefaultConnection"]));
            })
            .AddDiscordBot(hostContext.Configuration)
            .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
            .AddExperiencePlugin()
            .AddWelcomerPlugin()
            .AddAdvertisementGuardPlugin()
            .AddAutoRolesPlugin()
            .AddPersonalVocalPlugin()
            ;
    })
    .ConfigureLogging(
        c => c
            .AddConsole()
            .AddFilter("System.Net.Http.HttpClient.*", LogLevel.Warning)
    )
    .Build();

await host.RunAsync();