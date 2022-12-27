// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.Commands.Extensions;

namespace RemoraDiscordBot.Core.Infrastructure;

public static class Setup
{
    public static IServiceCollection AddDiscordBotInfrastructure(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddPostExecutionEvent<CommandPostExecutionEvent>()
            ;
    }
}