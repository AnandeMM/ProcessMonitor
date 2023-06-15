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
CancellationTokenSource cts = new CancellationTokenSource();

Console.WriteLine("Press q to Stop");

Task task = Task.Run(() =>
{
    if (Console.ReadKey(true).KeyChar == 'q')
        cts.Cancel();
});


await Start();
cts.Dispose();

async Task Start()
{
    var timer = new PeriodicTimer(TimeSpan.FromMinutes(monitorFrequency));

    try
    {

        while (await timer.WaitForNextTickAsync(cts.Token))
        {
            _processMonitor.Execute(processName, maxLifetime);

        }
    }
    catch (OperationCanceledException)
    {
        _processMonitor.Execute(processName, maxLifetime);
        Console.WriteLine("Monitor was stopped");
    }
}