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
    int Kill(int processId);
    IEnumerable<int> GetByName(string processName);
    DateTime GetStartTimeById(int processId);

}
