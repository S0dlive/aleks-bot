// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;

namespace RemoraDiscordBot.Plugins.PersonalVocal;

public static class Setup
{
    public static IServiceCollection AddPersonalVocalPlugin(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddCommandTree()
                .Finish()
            ;
    }
}