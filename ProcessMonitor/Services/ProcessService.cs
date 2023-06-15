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

    public int Kill(int processId)
    {
        Process.GetProcessById(processId).Kill();
        return processId;
    }

    public IEnumerable<int> GetByName(string processName) {

        return Process.GetProcessesByName(processName).Select(process => process.Id);
    }

    public DateTime GetStartTimeById(int processId)
    {

        return Process.GetProcessById(processId).StartTime;
    }


}
