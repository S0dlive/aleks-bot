using MediatR;

namespace RemoraDiscordBot.Plugins.Welcomer.Commands;

public record UpdateWelcomeMessageCommand(long GuildId, string newWelcomeMessage) : IRequest;