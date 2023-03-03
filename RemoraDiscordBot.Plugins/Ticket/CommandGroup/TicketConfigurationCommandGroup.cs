using System.ComponentModel;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Feedback.Services;

namespace RemoraDiscordBot.Plugins.Ticket.CommandGroup;

[Group("ticket"), Description("Ticket System.")]
public class TicketConfigurationCommandGroup :
    Remora.Commands.Groups.CommandGroup
{
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly ICommandContext _commandContext;
    private readonly FeedbackService _feedbackService;
    private readonly IMediator _mediator;

    public TicketConfigurationCommandGroup(IDiscordRestChannelAPI channelApi,
        ICommandContext commandContext,
        FeedbackService feedbackService,
        IMediator mediator)
    {
        _channelApi = channelApi;
        _commandContext = commandContext;
        _feedbackService = feedbackService;
        _mediator = mediator;
    }
}