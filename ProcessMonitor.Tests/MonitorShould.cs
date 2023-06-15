using Microsoft.Extensions.Logging;
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
        var processServiceMock = new Mock<IProcessService>();
        var loggerMock = new Mock<ILogger<Services.Monitor>>();
        IProcessMonitor processMonitor = new Services.Monitor(processServiceMock.Object, loggerMock.Object);

        //Act
        var shouldBeKilled = processMonitor.Exceeds(DateTime.Now.AddMinutes(-10), 5);

        //Assert
        Assert.That(shouldBeKilled, Is.True);
    }


    [Test]
    public void ProcessMaxLifetimeGreaterThanExecutionTime()
    {

        //Arrange
        var processServiceMock = new Mock<IProcessService>();
        var loggerMock = new Mock<ILogger<Services.Monitor>>();
        IProcessMonitor processMonitor = new Services.Monitor(processServiceMock.Object, loggerMock.Object);

        //Act
        var shouldBeKilled = processMonitor.Exceeds(DateTime.Now.AddMinutes(-10), 15);

        //Assert
        Assert.That(shouldBeKilled, Is.False);
    }

    [Test]
    public void ProcessMaxLifetimeEqualExecutionTime()
    {

        //Arrange
        var processServiceMock = new Mock<IProcessService>();
        var loggerMock = new Mock<ILogger<Services.Monitor>>();
        IProcessMonitor processMonitor = new Services.Monitor(processServiceMock.Object, loggerMock.Object);

        //Act
        var shouldBeKilled = processMonitor.Exceeds(DateTime.Now.AddMinutes(-10), 10.2);

        //Assert
        Assert.That(shouldBeKilled, Is.False);
    }

    [Test]
    public void AllProcessesExeceededLimit()
    {

        //Arrange
        var processServiceMock = new Mock<IProcessService>();
        var loggerMock = new Mock<ILogger<Services.Monitor>>();

        processServiceMock.Setup(ps => ps.GetByName("test")).Returns(new List<int> { 1, 2 });
        processServiceMock.Setup(ps => ps.GetStartTimeById(1)).Returns(DateTime.Now.AddMinutes(-10));
        processServiceMock.Setup(ps => ps.GetStartTimeById(2)).Returns(DateTime.Now.AddMinutes(-6));
        processServiceMock.Setup(ps => ps.Kill(1)).Returns(1);
        processServiceMock.Setup(ps => ps.Kill(2)).Returns(2);

        IProcessMonitor processMonitor = new Services.Monitor(processServiceMock.Object, loggerMock.Object);


        //Act
        var killedProcesses = processMonitor.Execute("test", 5);

        //Assert
        Assert.That(killedProcesses.Count, Is.EqualTo(2));
        Assert.That(killedProcesses.Contains(1), Is.True);
        Assert.That(killedProcesses.Contains(2), Is.True);
    }



    [Test]
    public void SomeProcessesExeceededLimit()
    {

        //Arrange
        var processServiceMock = new Mock<IProcessService>();
        var loggerMock = new Mock<ILogger<Services.Monitor>>();

        processServiceMock.Setup(ps => ps.GetByName("test")).Returns(new List<int> { 1, 2 });
        processServiceMock.Setup(ps => ps.GetStartTimeById(1)).Returns(DateTime.Now.AddMinutes(-10));
        processServiceMock.Setup(ps => ps.GetStartTimeById(2)).Returns(DateTime.Now.AddMinutes(-6));
        processServiceMock.Setup(ps => ps.Kill(1)).Returns(1);
        processServiceMock.Setup(ps => ps.Kill(2)).Returns(2);

        IProcessMonitor processMonitor = new Services.Monitor(processServiceMock.Object, loggerMock.Object);

        //Act
        var killedProcesses = processMonitor.Execute("test", 8);

        //Assert
        Assert.That(killedProcesses.Count, Is.EqualTo(1));
        Assert.That(killedProcesses.Contains(1), Is.True);
        Assert.That(killedProcesses.Contains(2), Is.False);
    }



    [Test]
    public void ZeroProcessesExeceededLimit()
    {

        //Arrange
        var processServiceMock = new Mock<IProcessService>();
        var loggerMock = new Mock<ILogger<Services.Monitor>>();

        processServiceMock.Setup(ps => ps.GetByName("test")).Returns(new List<int> { 1, 2 });
        processServiceMock.Setup(ps => ps.GetStartTimeById(1)).Returns(DateTime.Now.AddMinutes(-10));
        processServiceMock.Setup(ps => ps.GetStartTimeById(2)).Returns(DateTime.Now.AddMinutes(-6));
        processServiceMock.Setup(ps => ps.Kill(1)).Returns(1);
        processServiceMock.Setup(ps => ps.Kill(2)).Returns(2);

        IProcessMonitor processMonitor = new Services.Monitor(processServiceMock.Object, loggerMock.Object);

        //Act
        var killedProcesses = processMonitor.Execute("test", 15);

        //Assert
        Assert.That(killedProcesses.Any, Is.False);
        Assert.That(killedProcesses.Contains(1), Is.False);
        Assert.That(killedProcesses.Contains(2), Is.False);
    }
}
