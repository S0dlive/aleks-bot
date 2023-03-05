using System.ComponentModel;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Results;
using RemoraDiscordBot.Business.Extensions;
using RemoraDiscordBot.Plugins.Ticket.Commands.Config;

namespace RemoraDiscordBot.Plugins.Ticket.CommandGroup;

[Group("ticket"), Description("Ticket System.")]
public class TicketConfigurationCommandGroup :
    Remora.Commands.Groups.CommandGroup
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly IDiscordRestGuildAPI _guildApi;
    private readonly ICommandContext _commandContext;
    private readonly FeedbackService _feedbackService;
    private readonly IMediator _mediator;

    public TicketConfigurationCommandGroup(IDiscordRestChannelAPI channelApi,
        ICommandContext commandContext,
        FeedbackService feedbackService,
        IMediator mediator,
        IDiscordRestGuildAPI guildApi)
    {
        _channelApi = channelApi;
        _commandContext = commandContext;
        _feedbackService = feedbackService;
        _mediator = mediator;
        _guildApi = guildApi;
    }

    [Command("config")]
    [Description("config the ticket system.")]
    [Ephemeral()]
    public async Task<IResult> ConfigTicketSystemAsync(
        [Description("set the moderator channel")] [Option("moderator")]IChannel moderatorChannel,
        [Description("set the ticket channel")] [Option("ticket")]IChannel ticketChannel)
    {
        if (!_commandContext.TryGetGuildID(out var guildId))
            throw new InvalidOperationException();
        if (moderatorChannel is null)
        {
            moderatorChannel = _guildApi.CreateGuildChannelAsync(guildId.Value, "moderator-ticket-channel").GetAwaiter().GetResult().Entity;
        }
        if (ticketChannel is null)
        {
            ticketChannel = _guildApi.CreateGuildChannelAsync(guildId.Value, "ticket-channel").GetAwaiter().GetResult().Entity;
        }

        _mediator.Send(new CreateConfigTicketCommand(guildId.Value.ToLong(),
            moderatorChannel.ID.ToLong(),
            ticketChannel.ID.ToLong()));

        return (Result) await _feedbackService.SendContextualSuccessAsync(
            "The ticket system was successful update.");
    }
}