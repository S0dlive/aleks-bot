// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;
using RemoraDiscordBot.Core.Commands;

namespace RemoraDiscordBot.Core.CommandGroups;

public static class Setup
{
    public static IServiceCollection AddDiscordBotCoreCommands(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddCommandTree()
            .WithCommandGroup<CreditCommandGroup>()
            .WithCommandGroup<DiagnosticCommands>()
            .Finish()
            ;

        return serviceCollection;
    }
}