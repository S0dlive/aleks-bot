using MediatR;
using Microsoft.EntityFrameworkCore;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Plugins.Welcomer.Commands;

namespace RemoraDiscordBot.Plugins.Welcomer.Handlers.Commands;

public class UpdateWelcomeMessageHandler
    : AsyncRequestHandler<UpdateWelcomeMessageCommand>
{
    private readonly RemoraDiscordBotDbContext _dbContext;

    public UpdateWelcomeMessageHandler(RemoraDiscordBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task Handle(UpdateWelcomeMessageCommand request, CancellationToken cancellationToken)
    {
        var guild = await _dbContext.WelcomerGuilds.FirstOrDefaultAsync(t => t.GuildId == request.GuildId);

        if (guild != null)
        {
            guild.WelcomeMessage = request.newWelcomeMessage;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}