// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Discord.Commands.Services;
using Remora.Discord.Gateway;
using Remora.Rest.Core;

namespace RemoraDiscordBot.Worker;

public class Worker
    : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly DiscordGatewayClient _gatewayClient;
    private readonly ILogger<Worker> _logger;
    private readonly SlashService _slashService;

    public Worker(
        ILogger<Worker> logger,
        DiscordGatewayClient gatewayClient,
        SlashService slashService,
        IConfiguration configuration)
    {
        _logger = logger;
        _gatewayClient = gatewayClient;
        _slashService = slashService;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InitializeSlashCommands(stoppingToken);

        var result = await _gatewayClient.RunAsync(stoppingToken);
        if (!result.IsSuccess) _logger.LogError(result.Error.Message);
    }

    private async Task InitializeSlashCommands(CancellationToken stoppingToken)
    {
        var guildId = _configuration["Discord:GUILD_ID_"]
                      ?? throw new ArgumentNullException("Discord:GUILD_ID_");

        var guildIdParsed = ulong.Parse(guildId);

        var development = _configuration["DOTNET_ENVIRONMENT"] == "Development";

        _logger.LogInformation("Your bot is running in {Environment} mode.", development
            ? "Development"
            : "Production");

        var updateSlash = development switch
        {
            true => await _slashService.UpdateSlashCommandsAsync(new Snowflake(guildIdParsed), ct: stoppingToken),
            false => await _slashService.UpdateSlashCommandsAsync(ct: stoppingToken)
        };

        if (!updateSlash.IsSuccess)
            _logger.LogWarning("Failed to update slash commands: {Reason}", updateSlash.Error.Message);
    }
}