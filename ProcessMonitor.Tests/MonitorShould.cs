using Moq;
using ProcessMonitor.Contracts;
using ProcessMonitor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessMonitor.Tests;
[TestFixture]
public class MonitorShould
{

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ProcessMaxLifetimeLessThanExecutionTime()
    {

        //Arrange
        Mock<IProcessService> processService = new Mock<IProcessService>();
        IProcessMonitor processMonitor = new Services.Monitor(processService.Object);

        //Act
        var shouldBeKilled = processMonitor.ShouldBeKilled(DateTime.Now.AddMinutes(-10), 5);

        //Assert
        Assert.That(shouldBeKilled, Is.True);
    }


    [Test]
    public void ProcessMaxLifetimeGreaterThanExecutionTime()
    {

        //Arrange
        Mock<IProcessService> processService = new Mock<IProcessService>();
        IProcessMonitor processMonitor = new Services.Monitor(processService.Object);

        //Act
        var shouldBeKilled = processMonitor.ShouldBeKilled(DateTime.Now.AddMinutes(-10), 15);

        //Assert
        Assert.That(shouldBeKilled, Is.False);
    }

    [Test]
    public void ProcessMaxLifetimeEqualExecutionTime()
    {

        //Arrange
        Mock<IProcessService> processService = new Mock<IProcessService>();
        IProcessMonitor processMonitor = new Services.Monitor(processService.Object);

        //Act
        var shouldBeKilled = processMonitor.ShouldBeKilled(DateTime.Now.AddMinutes(-10), 10.2);

        //Assert
        Assert.That(shouldBeKilled, Is.False);
    }
}
