using System.ComponentModel.DataAnnotations;

namespace RemoraDiscordBot.Data.Domain.Ticket;

public class TicketConfig
{
    [Key]
    public string Id { get; set; }
    public long GuildId { get; set; }
    public long ModeratorChannelId { get; set; }
    public long TicketChannelId { get; set; }
}