using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitor.Contracts
{
    public interface IProcessMonitor
    {
        IEnumerable<int> Execute(string? processName, double maxLifetime);
        bool ShouldBeKilled(DateTime startTime, double lifetime);
    }
}
