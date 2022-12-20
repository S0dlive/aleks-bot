// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Gateway.Extensions;
using RemoraDiscordBot.Core.Commands;
using RemoraDiscordBot.Core.Exceptions;
using RemoraDiscordBot.Core.Responders.AnyResponderLogging;
using RemoraDiscordBot.Data;

namespace RemoraDiscordBot.Core;

public static class Setup
{
    public static IServiceCollection AddDiscordBot(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var botToken = configuration["Discord:BotToken"]
                       ?? throw new BotTokenCannotBeNullException();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new ArgumentNullException();

        return serviceCollection
                .AddDiscordGateway(_ => botToken)
                .AddDiscordCommands(true)
                .AddDiscordBotCommands()
                .AddAnyEventResponderLogging()
            ;
    }
}