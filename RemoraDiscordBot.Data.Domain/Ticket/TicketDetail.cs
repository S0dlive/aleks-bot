using System.ComponentModel.DataAnnotations;

namespace RemoraDiscordBot.Data.Domain.Ticket;

public class TicketDetails
{ 
    [Key]
    public string TicketId { get; set; }
    public long GuildId { get; set; }
    public long ChannelId { get; set; }
    public long ModeratorId { get; set; }
    public long UserId { get; set; }
}