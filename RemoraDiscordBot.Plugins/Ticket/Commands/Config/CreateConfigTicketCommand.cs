using MediatR;

namespace RemoraDiscordBot.Plugins.Ticket.Commands.Config;

public record CreateConfigTicketCommand(long GuildId, long ModeratorChannelId, long TicketChannelId)
    : IRequest
{
}