// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Feedback.Messages;
using Remora.Discord.Commands.Feedback.Services;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace RemoraDiscordBot.Plugins.AdvertisementGuard.Responders;

public class UserMessageAdvertisementGuardResponder
    : IResponder<IMessageCreate>
{
    private const string DISCORD_INVITE_REGEX =
        @"(https?:\/\/)?(www\.)?(discord\.(gg|io|me|li)|discordapp\.com\/invite)\/.+[a-z]";

    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly FeedbackService _feedbackService;

    public UserMessageAdvertisementGuardResponder(
        FeedbackService feedbackService,
        IDiscordRestChannelAPI channelApi)
    {
        _feedbackService = feedbackService;
        _channelApi = channelApi;
    }

    public async Task<Result> RespondAsync(IMessageCreate gatewayEvent, CancellationToken ct = default)
    {
        if (gatewayEvent.Author.IsBot is {HasValue: true, Value: true}) return Result.FromSuccess();

        if (gatewayEvent.Member.Value.Permissions.Value.HasPermission(DiscordPermission.ManageMessages))
            return Result.FromSuccess();

        var message = gatewayEvent.Content;
        if (string.IsNullOrWhiteSpace(message)) return Result.FromSuccess();

        var match = Regex.Match(message, DISCORD_INVITE_REGEX, RegexOptions.IgnoreCase);
        if (!match.Success) return Result.FromSuccess();

        var user = gatewayEvent.Author;

        await _channelApi.DeleteMessageAsync(gatewayEvent.ChannelID, gatewayEvent.ID, ct: ct);

        return (Result) await _feedbackService.SendErrorAsync(
            gatewayEvent.ChannelID,
            "Oops... Il semblerait que votre message contenait une potentielle invitation. Erreur ? Contactez un administrateur.",
            user.ID,
            new FeedbackMessageOptions
            {
                MessageFlags = MessageFlags.Urgent | MessageFlags.Ephemeral
            },
            ct);
    }
}