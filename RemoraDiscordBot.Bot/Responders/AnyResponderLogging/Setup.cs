// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Remora.Discord.Gateway.Extensions;

namespace RemoraDiscordBot.Core.Responders.AnyResponderLogging;

public static class Setup
{
    public static IServiceCollection AddAnyEventResponderLogging(this IServiceCollection services)
    {
        return services
            .AddResponder<AnyEventResponder>();
    }
}