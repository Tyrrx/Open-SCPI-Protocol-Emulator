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
    public class TestUpdateInterferenceOnElectricalUnitOfMeasureChanges
    {
        [TestMethod]
        public void TestForMeasuringDeviceWithInitialValue()
        {
            // Arrange
            var electricalUnitOfMeasure = new BehaviorSubject<ElectricalUnitOfMeasure>(ElectricalUnitOfMeasure.Current);
            var interferenceFactors = new List<double>(new[] {1.0});

            var measuringDevice = Substitute.For<IKeysight34Series>();
            measuringDevice.ElectricalUnitOfMeasureBehaviorSubject.Returns(electricalUnitOfMeasure);
            measuringDevice.GetInterferenceFactors(default).ReturnsForAnyArgs(interferenceFactors);

            // Act
            UpdateInterferenceOnElectricalUnitOfMeasureChanges.OfMeasuringDevice(measuringDevice);

            // Assert
            measuringDevice.GeneratorQueue.Should().Equal(interferenceFactors);
            measuringDevice.Received().GetInterferenceFactors(ElectricalUnitOfMeasure.Current);
        }

        [TestMethod]
        public void TestForMeasuringDeviceWithUpdatedUnit()
        {
            // Arrange
            var electricalUnitOfMeasure = new BehaviorSubject<ElectricalUnitOfMeasure>(ElectricalUnitOfMeasure.Current);
            var interferenceFactors = new List<double>(new[] {1.0});

            var measuringDevice = Substitute.For<IKeysight34Series>();
            measuringDevice.ElectricalUnitOfMeasureBehaviorSubject.Returns(electricalUnitOfMeasure);
            measuringDevice.GetInterferenceFactors(default).ReturnsForAnyArgs(interferenceFactors);

            // Act
            UpdateInterferenceOnElectricalUnitOfMeasureChanges.OfMeasuringDevice(measuringDevice);
            electricalUnitOfMeasure.OnNext(ElectricalUnitOfMeasure.Voltage);

            // Assert
            measuringDevice.GeneratorQueue.Should().Equal(interferenceFactors);
            measuringDevice.Received().GetInterferenceFactors(ElectricalUnitOfMeasure.Current);
            measuringDevice.Received().GetInterferenceFactors(ElectricalUnitOfMeasure.Voltage);
        }
    }
}