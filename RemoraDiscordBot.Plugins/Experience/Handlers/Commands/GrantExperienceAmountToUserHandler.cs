// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Xp;
using RemoraDiscordBot.Plugins.Experience.Commands;

namespace RemoraDiscordBot.Plugins.Experience.Handlers.Commands;

public sealed class GrantExperienceAmountToUserHandler
    : AsyncRequestHandler<GrantExperienceAmountToUserCommand>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly ILogger<GrantExperienceAmountToUserHandler> _logger;
    private readonly IMediator _mediator;

    public GrantExperienceAmountToUserHandler(
        IMediator mediator,
        RemoraDiscordBotDbContext dbContext,
        ILogger<GrantExperienceAmountToUserHandler> logger)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _logger = logger;
    }

    protected override async Task Handle(
        GrantExperienceAmountToUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Granting {Amount} experience to {User}", request.Amount, request.UserID.ToLong());

        // Find the user in the database
        var user = await _dbContext.UserGuildXps
            .FirstOrDefaultAsync(x => x.UserId == request.UserID.ToLong(), cancellationToken);

        if (user == null) throw new ArgumentNullException(nameof(user));

        // Update the user's experience and level
        user = await UpdateExperienceAndLevel(user, request.Amount, cancellationToken);

        // Save the changes to the database
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task<UserGuildXp> UpdateExperienceAndLevel(
        UserGuildXp user,
        long experience,
        CancellationToken cancellationToken)
    {
        // Calculate the new experience and level
        var (newExperience, newLevel) = CalculateExperienceAndLevel(user.XpAmount, user.Level, experience);

        // Update the user's experience and level
        user.XpAmount = newExperience;
        user.XpNeededToLevelUp = CalculateExperienceNeeded(newExperience, newLevel);
        user.Level = newLevel;

        return user;
    }


    private static (long, long) CalculateExperienceAndLevel(
        long currentExperience,
        long currentLevel,
        long additionalExperience)
    {
        var newExperience = currentExperience + additionalExperience;
        var newLevel = currentLevel;

        while (newExperience >= CalculateExperienceNeeded(newExperience, newLevel))
        {
            newExperience -= CalculateExperienceNeeded(newExperience, newLevel);
            newLevel++;
        }

        return (newExperience, newLevel);
    }

    private static int CalculateExperienceNeeded(long xp, long level)
    {
        return (int) (100 * Math.Pow(level + 1, 2))
               - (int) xp;
    }
}