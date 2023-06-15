// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProcessMonitor.Contracts;
using ProcessMonitor.Services;
using System.Net;

var processName = args[0];
var isLifetimeValid = Double.TryParse(args[1], out var maxLifetime);
var isMonitorFrequencyValid = Double.TryParse(args[2], out var monitorFrequency);

IHost _host = Host.CreateDefaultBuilder().ConfigureServices(
    services =>
    {
        services.AddSingleton<IProcessService, ProcessService>();
        services.AddSingleton<IProcessMonitor, ProcessMonitor.Services.Monitor>();
    }
    ).ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

var _processMonitor = _host.Services.GetRequiredService<IProcessMonitor>();


var timer = new PeriodicTimer(TimeSpan.FromMinutes(monitorFrequency));
while (await timer.WaitForNextTickAsync(CancellationToken.None))
{
     _processMonitor.Execute(processName, maxLifetime);

}