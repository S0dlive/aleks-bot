// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Commands.Conditions;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Results;
using RemoraDiscordBot.Business.Infrastructure.Attributes;
using RemoraDiscordBot.Core.Infrastructure.Errors;

namespace RemoraDiscordBot.Business.Attributes;

public class NoBotCondition
    : ICondition<NoBotAttribute, IUser>
{
    public ValueTask<Result> CheckAsync(NoBotAttribute attribute, IUser data, CancellationToken ct = default)
    {
        return ValueTask.FromResult(data?.IsBot is {HasValue: true, Value: true}
            ? Result.FromError(new NoBotError("This command cannot be used on bots."))
            : Result.FromSuccess());
    }
}