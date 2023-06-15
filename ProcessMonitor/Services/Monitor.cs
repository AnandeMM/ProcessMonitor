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
    public Monitor(IProcessService processService)
    {


        _processService = processService;

    }
    public IEnumerable<int> Execute(string? processName, double maxLifetime)
    {
        IList<int> killedProcessIds = new List<int>();
        var processeIds = _processService.GetByName(processName);
        processeIds.ToList().ForEach(pId =>
        {
            if (ShouldBeKilled(_processService.GetStartTimeById(pId), maxLifetime))
                killedProcessIds.Add( _processService.Kill(pId));
        });

        return killedProcessIds;
    }

    public bool ShouldBeKilled(DateTime startTime, double lifetime)
    {
        TimeSpan runningTime = DateTime.Now - startTime;
        return runningTime.TotalMinutes >= lifetime;
    }
}
