using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitor.Contracts;

public interface IProcessService
{
    bool Exists(string processName);
    void Kill(Process process);

    bool ShouldBeKilled(DateTime startTime, double lifetime);

    IEnumerable<Process> GetByName(string processName);
    DateTime GetStartTimeById(int processId);

}
