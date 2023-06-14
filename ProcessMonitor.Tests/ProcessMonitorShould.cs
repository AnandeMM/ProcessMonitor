using NUnit.Framework;
using ProcessMonitor.Services;
using System.Diagnostics;

namespace ProcessMonitor.Tests;

[TestFixture]
public class ProcessMonitorShould
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ProcessMxLifetimeExceeded()
    {
        var process = Process.Start("myprocess.exe");
        var sut = new ProcessService();

        await Task.Delay(121000);

        Assert.That(sut.ShouldBeKilled(process, 2), Is.True);
    }
}