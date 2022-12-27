// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;
using Remora.Discord.Gateway.Extensions;
using RemoraDiscordBot.Plugins.Experience.CommandGroups;
using RemoraDiscordBot.Plugins.Experience.Responders;

namespace RemoraDiscordBot.Plugins.Experience;

public static class Setup
{
    public static IServiceCollection AddExperiencePlugin(
        this IServiceCollection services)
    {
        return services
                .AddCommandTree()
                .WithCommandGroup<ExperienceCommandGroup>()
                .Finish()
            
                .AddResponder<MessageCreateGrantExperienceResponder>()
            ;
    }
}