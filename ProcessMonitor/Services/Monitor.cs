using ProcessMonitor.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitor.Services;

public class Monitor : IProcessMonitor
{
    IProcessService _processService;
    public Monitor(IProcessService processService) { 
    

        _processService = processService;
    
    }
    public async Task Execute(string? processName, double maxLifetime, double monitorFrequency)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(monitorFrequency));
        while (await timer.WaitForNextTickAsync(CancellationToken.None))
        {
            var processes = _processService.GetByName(processName);
            processes.ToList().ForEach(p =>
            {
                if (_processService.ShouldBeKilled(_processService.GetStartTimeById(p.Id), maxLifetime))
                    _processService.Kill(p);
            });
        }
    }
}
