// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.Drawing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Rest.Core;
using Remora.Results;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Experience;
using RemoraDiscordBot.Plugins.Experience.Commands;

namespace RemoraDiscordBot.Plugins.Experience.Handlers.Commands;

public sealed class GrantExperienceAmountToUserHandler
    : AsyncRequestHandler<GrantExperienceAmountToUserCommand>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly IDiscordRestUserAPI _discordRestUserApi;
    private readonly FeedbackService _feedbackService;
    private readonly ILogger<GrantExperienceAmountToUserHandler> _logger;

    public GrantExperienceAmountToUserHandler(
        RemoraDiscordBotDbContext dbContext,
        ILogger<GrantExperienceAmountToUserHandler> logger,
        FeedbackService feedbackService,
        IDiscordRestUserAPI discordRestUserApi)
    {
        _dbContext = dbContext;
        _logger = logger;
        _feedbackService = feedbackService;
        _discordRestUserApi = discordRestUserApi;
    }

    protected override async Task Handle(
        GrantExperienceAmountToUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Granting {Amount} experience to {User}", request.Amount, request.UserId.ToLong());

        // Find the user in the database
        var user = await _dbContext.UserGuildXps
            .Include(x => x.Creature)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId.ToLong() && x.GuildId == request.GuildId.ToLong(),
                cancellationToken);
        
        _logger.LogInformation("Found user {User} with {Amount} experience", user?.UserId, user?.XpAmount);

        if (user == null)
        {
            _logger.LogInformation("User {User} not found in database, creating a new one", request.UserId.ToLong());
            
            _dbContext.UserGuildXps.Add(new UserGuildXp(
                request.UserId.ToLong(),
                request.GuildId.ToLong()));

            await _dbContext.SaveChangesAsync(cancellationToken);

            return;
        }


        // Update the user's experience and level
        user = await UpdateExperienceAndLevel(user, request.ChannelId, request.Amount);

        // Save the changes to the database
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<UserGuildXp> UpdateExperienceAndLevel(
        UserGuildXp user,
        Snowflake channelId,
        long experience)
    {
        // Calculate the new experience and level
        var (newExperience, newLevel) = CalculateExperienceAndLevel(user.XpAmount, user.Level, experience);

        _logger.LogInformation("User {User} has {Experience} experience and is level {Level}", user.UserId, newExperience, newLevel);
        
        await NotifyUserIfLevelUpAsync(user, channelId, newLevel);

        // Update the user's experience and level
        user.XpAmount = newExperience;
        user.XpNeededToLevelUp = CalculateExperienceNeeded(newExperience, newLevel);
        user.Level = newLevel;
        
        _logger.LogInformation("User {User} has been updated to {Experience} experience and is level {Level}", user.UserId, newExperience, newLevel);

        return user;
    }

    private async Task<Result> NotifyUserIfLevelUpAsync(UserGuildXp user, Snowflake channelId, long newLevel)
    {
        if (user.Level == newLevel) return Result.FromSuccess();

        var member = await _discordRestUserApi.GetUserAsync(user.UserId.ToSnowflake());

        if (newLevel % 5 == 0)
        {
            _logger.LogInformation("User {User} has leveled up to level {Level}", user.UserId, newLevel);
            
            if (user.Creature is {IsEgg: true, Level: 5})
            {
                user.Creature.IsEgg = false;
                user.Creature.Level = 1;
                
                _logger.LogInformation("User {User} has hatched their creature to level {Creature}!", user.UserId, user.Creature.Level);
            }
            else
            {
                user.Creature.Level++;
                
                _logger.LogInformation("User {User} has leveled up their creature to level {Level}!", user.UserId, user.Creature.Level);
            }

            return (Result) await _feedbackService.SendContentAsync(
                channelId,
                $"Félicitations **{member.Entity.Username}**! Tu as atteint le niveau {newLevel}!\n*Et quelque chose d'autre semble avoir bougé...*",
                Color.Aqua);
        }


        return (Result) await _feedbackService.SendContentAsync(
            channelId,
            $"Félicitations **{member.Entity.Username}**! Tu as atteint le niveau {newLevel}!",
            Color.Aqua);
    }


    private static (long NewExperience, long NewLevel) CalculateExperienceAndLevel(
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