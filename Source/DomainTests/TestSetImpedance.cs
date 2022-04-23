using System.Reactive.Subjects;
using Domain;
using Domain.Interfaces;
using Domain.UnionTypes;
using FluentAssertions;
using FunicularSwitch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace DomainTests
{
    [TestClass]
    public class TestSetImpedance
    {
        [TestMethod]
        public void Test_OfMeasuringDevice()
        {
            // Arrange
            var impedance = new BehaviorSubject<Impedance>(Impedance.Low);
            var updatedImpedance = Impedance.High;

            var electricalMeasuringSubsystem = Substitute.For<IElectricalMeasuringSubsystem>();
            electricalMeasuringSubsystem.ImpedanceBehaviorSubject.Returns(impedance);

            // Act
            var result = SetImpedance.OfMeasuringDevice(electricalMeasuringSubsystem, updatedImpedance);

            // Assert
            result.Should().BeOfType<Ok<Unit>>();
            impedance.Value.Should().Be(updatedImpedance);
        }
    }
}