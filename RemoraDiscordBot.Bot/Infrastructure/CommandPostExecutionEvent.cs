// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Services;
using Remora.Results;

namespace RemoraDiscordBot.Core.Infrastructure;

public class CommandPostExecutionEvent
    : IPostExecutionEvent
{
    private readonly ILogger<CommandPostExecutionEvent> _logger;

    public CommandPostExecutionEvent(ILogger<CommandPostExecutionEvent> logger)
    {
        _logger = logger;
    }

    public Task<Result> AfterExecutionAsync(
        ICommandContext context,
        IResult commandResult,
        CancellationToken ct = default)
    {
        if (commandResult.IsSuccess) return Task.FromResult(Result.FromSuccess());

        _logger.LogError(
            "Command {CommandName} failed with error {Error}",
            context.Command.Command.Node.CommandMethod.Name,
            commandResult.Error);

        return Task.FromResult(Result.FromError(commandResult.Error));
    }
}