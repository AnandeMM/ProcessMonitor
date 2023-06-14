using Moq;
using NUnit.Framework;
using ProcessMonitor.Contracts;
using ProcessMonitor.Services;
using System.Diagnostics;

namespace ProcessMonitor.Tests;

[TestFixture]
public class ProcessServiceShould
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ProcessMaxLifetimeLessThanExecutionTime()
    {

        //Arrange
        IProcessService processService = new ProcessService();

        //Act
        var shouldBeKilled = processService.ShouldBeKilled(DateTime.Now.AddMinutes(-10), 5);

        //Assert
        Assert.That(shouldBeKilled, Is.True);
    }


    [Test]
    public void ProcessMaxLifetimeGreaterThanExecutionTime()
    {

        //Arrange
        IProcessService processService = new ProcessService();

        //Act
        var shouldBeKilled = processService.ShouldBeKilled(DateTime.Now.AddMinutes(-10), 15);

        //Assert
        Assert.That(shouldBeKilled, Is.False);
    }

    [Test]
    public void ProcessMaxLifetimeEqualExecutionTime()
    {

        //Arrange
        IProcessService processService = new ProcessService();

        //Act
        var shouldBeKilled = processService.ShouldBeKilled(DateTime.Now.AddMinutes(-10), 10.2);

        //Assert
        Assert.That(shouldBeKilled, Is.False);
    }
}