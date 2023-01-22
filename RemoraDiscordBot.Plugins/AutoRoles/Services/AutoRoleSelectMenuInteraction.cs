// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Discord.Commands.Contexts;
using Remora.Discord.Interactivity;
using Remora.Results;

namespace RemoraDiscordBot.Plugins.AutoRoles.Services;

public class AutoRoleSelectMenuInteraction
    : InteractionGroup
{
    public const string AutoRoleSelectMenuPrefix = "auto-role-select-menu-";
    private readonly InteractionContext _interactionContext;

    public AutoRoleSelectMenuInteraction(InteractionContext interactionContext)
    {
        _interactionContext = interactionContext;
    }

    [SelectMenu(AutoRoleSelectMenuPrefix)]
    public async Task<IResult> HandleAutoRoleSelectMenuInteractionAsync(IReadOnlyList<string> values)
    {
        // Create select menu with values
        return Result.FromSuccess();
    }
}