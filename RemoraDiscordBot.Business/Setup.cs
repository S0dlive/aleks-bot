// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;
using RemoraDiscordBot.Business.Attributes;

namespace RemoraDiscordBot.Business;

public static class Setup
{
    public static IServiceCollection AddDiscordBotBusiness(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddCondition<NoBotCondition>()
                .AddCondition<NonSelfActionableCondition>()
            ;
    }
}