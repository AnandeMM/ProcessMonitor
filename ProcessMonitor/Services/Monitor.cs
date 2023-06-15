using Microsoft.Extensions.Logging;
using ProcessMonitor.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitor.Services;

/// <summary>
/// A Class to Monitor the processes and Kill the ones that have exceeded a given lifetime
/// </summary>
public class Monitor : IProcessMonitor
{
    private readonly IProcessService _processService;
    private readonly ILogger _logger;
    public Monitor(
        IProcessService processService, 
        ILogger<Monitor> logger)
    {


        _processService = processService;
        _logger = logger;

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="processName"></param>
    /// <param name="maxLifetime"></param>
    /// <returns>The list of killed Processes</returns>
    public IEnumerable<int> Execute(string? processName, double maxLifetime)
    {
        _logger.LogInformation( "Executing Monitor");
        IList<int> killedProcessIds = new List<int>();
        var processeIds = _processService.GetByName(processName);
        processeIds.ToList().ForEach(pId =>
        {
            if (Exceeds(_processService.GetStartTimeById(pId), maxLifetime))
                killedProcessIds.Add( _processService.Kill(pId));
        });

        return killedProcessIds;
    }

    /// <summary>
    /// Checks if a given lifetime was exceeded 
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="lifetime"></param>
    /// <returns>True if the lifetime was exceeded; False otherwise</returns>
    public bool Exceeds(DateTime startTime, double lifetime)
    {
        TimeSpan runningTime = DateTime.Now - startTime;
        return runningTime.TotalMinutes > lifetime;
    }
}
