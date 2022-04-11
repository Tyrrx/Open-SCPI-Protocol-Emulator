using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Domain;
using Domain.Interfaces;
using Domain.UnionTypes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DomainTests
{
    [TestClass]
    public class TestGenerateMeasurementsOnTriggerChanges
    {
        [TestMethod]
        public void TestForMeasuringDevice()
        {
            // Arrange
            var trigger = new BehaviorSubject<TriggerState>(TriggerState.Idle);
            var generatorQueue = new Queue<double>(new[] { 1.0, 2.0 });
            var readingQueue = new ConcurrentQueue<MeasurementValue>();

            var measuringDevice = Substitute.For<IKeysight34Series>();
            measuringDevice.TriggerStateBehaviourSubject.Returns(trigger);
            measuringDevice.GeneratorQueue.Returns(generatorQueue);
            measuringDevice.ReadingQueue.Returns(readingQueue);
            measuringDevice.GenerateMeasurementValue(default).ReturnsForAnyArgs(x => 1.0);

            // Act
            GenerateMeasurementsOnTriggerChanges.ForMeasuringDevice(measuringDevice);
            trigger.OnNext(TriggerState.WaitForTrigger);

            // Assert
            generatorQueue.Should().Equal(2.0, 1.0);
            readingQueue.Should().ContainSingle(value => value.Equals(MeasurementValue.Double(1.0)));
        }

        [TestMethod]
        public void TestRotateGeneratorQueueAndProduceMeasurement()
        {
            // Arange
            var measuringDeviceGeneratorQueue = new Queue<double>(new[] { 1.0 });
            var measuringDeviceReadingQueue = new ConcurrentQueue<MeasurementValue>();

            // Act
            GenerateMeasurementsOnTriggerChanges.RotateGeneratorQueueAndProduceMeasurement(_ => 1.0,
                measuringDeviceGeneratorQueue, measuringDeviceReadingQueue);

            // Assert
            measuringDeviceGeneratorQueue.Should().Equal(1.0);
            measuringDeviceReadingQueue.Should().ContainSingle(value => value.Equals(MeasurementValue.Double(1.0)));
        }

        [TestMethod]
        public void TestRotateGeneratorQueueAndProduceMeasurementWithEmptyQueue()
        {
            // Arange
            var measuringDeviceGeneratorQueue = new Queue<double>();
            var measuringDeviceReadingQueue = new ConcurrentQueue<MeasurementValue>();

            // Act
            GenerateMeasurementsOnTriggerChanges.RotateGeneratorQueueAndProduceMeasurement(_ => 1.0,
                measuringDeviceGeneratorQueue, measuringDeviceReadingQueue);

            // Assert
            measuringDeviceGeneratorQueue.Should().BeEmpty();
            measuringDeviceReadingQueue.Should().ContainSingle(value => value.Equals(MeasurementValue.Double(1.0)));
        }
    }
}