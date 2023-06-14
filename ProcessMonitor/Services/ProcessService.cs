using ProcessMonitor.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitor.Services;

public class ProcessService : IProcessService
{

    public bool Exists(string processName)
    {
        return Process.GetProcessesByName(processName).Any();
    }

    public void Kill(Process process)
    {
        process.Kill();
  
    }

    public IEnumerable<Process> GetByName(string processName) {

        return Process.GetProcessesByName(processName);
    }

    public bool ShouldBeKilled(Process process, double lifetime)
    {
        TimeSpan runningTime = DateTime.Now - process.StartTime;
        return runningTime.TotalMinutes >= lifetime;
    }
}
