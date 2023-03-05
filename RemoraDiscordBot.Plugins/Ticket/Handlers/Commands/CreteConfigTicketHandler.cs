using MediatR;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Data;
using RemoraDiscordBot.Data.Domain.Ticket;
using RemoraDiscordBot.Plugins.Ticket.Commands.Config;

namespace RemoraDiscordBot.Plugins.Ticket.Handlers.Commands;

public class CreteConfigTicketHandler : AsyncRequestHandler<CreateConfigTicketCommand>
{
    private readonly RemoraDiscordBotDbContext _dbContext;
    private readonly IDiscordRestChannelAPI _channelApi;
    

    public CreteConfigTicketHandler(
        RemoraDiscordBotDbContext dbContext,
        IDiscordRestChannelAPI channelApi)
    {
        _dbContext = dbContext;
        _channelApi = channelApi;
    }
    
    protected override async Task Handle(CreateConfigTicketCommand request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.GuildConfigutaions.AddAsync(new TicketConfig()
        {
            GuildId = request.GuildId,
            Id = Guid.NewGuid().ToString(),
            ModeratorChannelId = request.ModeratorChannelId,
            TicketChannelId = request.TicketChannelId
        });

        var embed = new Embed()
        {
            Title = ":incoming_envelope: Create a ticket",
            Description = "To create a ticket it's very simple, select the type of your problem, wait for a moderator to answer your problem and expose your problem."
        };
        var embeds = new List<Embed>();
        embeds.Add(embed);
        _channelApi.CreateMessageAsync(request.TicketChannelId.ToSnowflake(), embeds: embeds);
    }
}