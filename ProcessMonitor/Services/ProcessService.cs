using Microsoft.Extensions.Logging;
using ProcessMonitor.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitor.Services;

/// <summary>
/// A Class to wrap System.Diagnostics.Process methods
/// </summary>
public class ProcessService : IProcessService
{
    private readonly ILogger _logger;
    public ProcessService(ILogger<ProcessService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Checks if a process with a given processName is running
    /// </summary>
    /// <param name="processName"></param>
    /// <returns>True/False</returns>
    public bool Exists(string processName)
    {
        return Process.GetProcessesByName(processName).Any();
    }

    /// <summary>
    /// Kill a process with a given processId
    /// </summary>
    /// <param name="processId"></param>
    /// <returns>int representing the process killed</returns>
    public int Kill(int processId)
    {
        Process.GetProcessById(processId).Kill();
        _logger.LogWarning("{ProcessId} was killed", processId);

        return processId;
    }

    /// <summary>
    /// Get the process ids with a given processName
    /// </summary>
    /// <param name="processName"></param>
    /// <returns>enumerable of int representing the process ids</returns>
    public IEnumerable<int> GetByName(string processName)
    {

        return Process.GetProcessesByName(processName).Select(process => process.Id);
    }


    /// <summary>
    /// Get the  start  time of a process with a given processId
    /// </summary>
    /// <param name="processId"></param>
    /// <returns>DateTime representing the process start time</returns>
    public DateTime GetStartTimeById(int processId)
    {

        return Process.GetProcessById(processId).StartTime;
    }


}
