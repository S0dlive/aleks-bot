using MediatR;

namespace RemoraDiscordBot.Plugins.Ticket.Commands;

public record CreateTicketCommand(long GuildId, long ChannelId, long ModeratorId, long UserId)
    : IRequest;