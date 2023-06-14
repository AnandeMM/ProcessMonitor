// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProcessMonitor.Contracts;
using ProcessMonitor.Services;
using System.Net;

var processName = args[0];
var isLifetimeValid = Double.TryParse(args[1], out var lifetime);
var isMonitorFrequencyValid = Double.TryParse(args[2], out var monitorFrequency);

IHost _host = Host.CreateDefaultBuilder().ConfigureServices(
    services =>
    {
        services.AddSingleton<IProcessService, ProcessService>();
    }
    ).Build();

var _processService = _host.Services.GetRequiredService<IProcessService>();

var timer = new PeriodicTimer(TimeSpan.FromMinutes(monitorFrequency));
while (await timer.WaitForNextTickAsync(CancellationToken.None))
{
    var processes = _processService.GetByName(processName);
    processes.ToList().ForEach(p =>
    {
        if(_processService.ShouldBeKilled(p, lifetime))
            _processService.Kill(p);
    });
}
