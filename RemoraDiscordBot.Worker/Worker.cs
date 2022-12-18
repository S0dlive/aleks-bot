// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Discord.Gateway;

namespace RemoraDiscordBot.Worker;

public class Worker
    : BackgroundService
{
    private readonly DiscordGatewayClient _gatewayClient;
    private readonly ILogger<Worker> _logger;

    public Worker(
        ILogger<Worker> logger,
        DiscordGatewayClient gatewayClient,
        IConfiguration configuration)
    {
        _logger = logger;
        _gatewayClient = gatewayClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var result = await _gatewayClient.RunAsync(stoppingToken);
        if (!result.IsSuccess) _logger.LogError(result.Error.Message);
    }
}