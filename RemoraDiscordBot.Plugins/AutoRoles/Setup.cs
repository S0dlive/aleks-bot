// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;
using RemoraDiscordBot.Data.Domain.AutoRoles;
using RemoraDiscordBot.Plugins.AutoRoles.CommandGroups;

namespace RemoraDiscordBot.Plugins.AutoRoles;

public static class Setup
{
    public static IServiceCollection AddAutoRolesPlugin(this IServiceCollection services)
    {
        return services
            .AddCommandTree()
            .WithCommandGroup<AutoRoleCommandGroup>()
            .WithCommandGroup<AutoRoleReactionCommandGroup>()
            .Finish();
    }
}