using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitor.Contracts
{
    public interface IProcessMonitor
    {
        Task Execute(string? processName, double maxLifetime, double monitorFrequency);
    }
}
