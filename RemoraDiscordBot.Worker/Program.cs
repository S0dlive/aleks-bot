// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Core;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services
            .AddHostedService<Worker>()
            .AddDiscordBot(hostContext.Configuration)
            .AddDbContext<RemoraDiscordBotDbContext>(options =>
                options.UseMySql(
                    hostContext.Configuration["ConnectionStrings:DefaultConnection"],
                    ServerVersion.AutoDetect(hostContext.Configuration["ConnectionStrings:DefaultConnection"])));
    })
    .ConfigureLogging
    (
        c => c
            .AddConsole()
            .AddFilter("System.Net.Http.HttpClient.*.LogicalHandler", LogLevel.Warning)
            .AddFilter("System.Net.Http.HttpClient.*.ClientHandler", LogLevel.Warning)
    )
    .Build();

await host.RunAsync();