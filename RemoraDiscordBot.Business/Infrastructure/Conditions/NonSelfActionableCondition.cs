// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Commands.Conditions;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Contexts;
using Remora.Results;
using RemoraDiscordBot.Business.Infrastructure.Attributes;
using RemoraDiscordBot.Core.Infrastructure.Errors;

namespace RemoraDiscordBot.Business.Attributes;

public class NonSelfActionableCondition
    : ICondition<NonSelfActionableAttribute, IUser>
{
    private readonly ICommandContext _commandContext;

    public NonSelfActionableCondition(
        ICommandContext commandContext)
    {
        _commandContext = commandContext;
    }

    public async ValueTask<Result> CheckAsync(NonSelfActionableAttribute attribute, IUser data, CancellationToken ct = default)
    {
        var user = _commandContext switch
        {
            IInteractionContext interactionContext => interactionContext.Interaction.User.Value,
            ITextCommandContext textCommandContext => textCommandContext.Message.Author.Value,
            _ => throw new InvalidOperationException(),
        };

        return user.ID == data.ID
            ? Result.FromError(new NonSelfActionableError("You can't do this to yourself."))
            : Result.FromSuccess();
    }
}