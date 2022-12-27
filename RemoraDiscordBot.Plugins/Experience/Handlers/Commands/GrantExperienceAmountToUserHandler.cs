// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.Experience.Commands;

namespace RemoraDiscordBot.Plugins.Experience.Handlers.Commands;

public sealed class GrantExperienceAmountToUserHandler
    : AsyncRequestHandler<GrantExperienceAmountToUserCommand>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly ILogger<GrantExperienceAmountToUserHandler> _logger;

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
        try
        {
            
            _logger.LogInformation("Granting {Amount} experience to {User}", request.Amount, request.UserID.ToLong());
            var user = await _dbContext.UserGuildXps.FirstOrDefaultAsync(
                x => x.UserId == request.UserID.ToLong(), cancellationToken: cancellationToken);

            ArgumentNullException.ThrowIfNull(user);
            
            user.XpAmount += request.Amount;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while granting experience to user {UserID}", request.UserID);
        }
    }
}