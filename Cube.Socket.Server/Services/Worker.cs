using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCoreServer;
using System.Net;

namespace Cube.Socket.Server.Services;


public class Worker : IHostedService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;


        var server = serviceProvider.GetService<ChatServer>();

        applicationLifetime.ApplicationStarted.Register(() =>
        {
            _logger.LogInformation("Cube.DataServer starting...");
            server?.Start();
        });

        applicationLifetime.ApplicationStopped.Register(() => {

            _logger.LogInformation("Cube.DataServer stopping...");
            server?.Stop();
        });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
