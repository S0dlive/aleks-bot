// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Responders;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Extensions;
using Remora.Discord.Interactivity;
using Remora.Discord.Interactivity.Extensions;
using RemoraDiscordBot.Business;
using RemoraDiscordBot.Core.CommandGroups;
using RemoraDiscordBot.Core.Exceptions;
using RemoraDiscordBot.Core.Infrastructure;
using RemoraDiscordBot.Core.Responders.AnyResponderLogging;
using RemoraDiscordBot.Core.Responders.SelfResponder;
using RemoraDiscordBot.Plugins.Experience;
using RemoraDiscordBot.Plugins.Welcomer;

namespace RemoraDiscordBot.Core;

public static class Setup
{
    public static IServiceCollection AddDiscordBot(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var botToken = configuration["Discord:BotToken"]
                       ?? throw new BotTokenCannotBeNullException();

        return serviceCollection
                .AddDiscordGateway(_ => botToken)
                .Configure<DiscordGatewayClientOptions>(options =>
                {
                    options.Intents = GatewayIntents.MessageContents
                                      | GatewayIntents.GuildMessages
                                      | GatewayIntents.Guilds
                                      | GatewayIntents.GuildMembers
                                      | GatewayIntents.GuildMessageReactions
                                      | GatewayIntents.GuildMessageTyping
                                      | GatewayIntents.Guilds
                                      | GatewayIntents.GuildVoiceStates;
                })
                .AddInteractivity()
                
                .AddDiscordCommands(true)
                .AddDiscordBotCoreCommands()
                .AddExperiencePlugin()
                .AddWelcomerPlugin()
                .AddAnyEventResponderLogging()
                .AddDiscordBotInfrastructure()
                .AddDiscordBotBusiness()
            
                .AddSelfResponder()
            ;
    }
}