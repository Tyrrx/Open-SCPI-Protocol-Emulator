using System.Collections.Concurrent;
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
    public class TestConfigureVoltage
    {
        [TestMethod]
        public void TestOfDeviceAccordingToGivenParameters_Defaults()
        {
            // Arrange
            var electricalUnitOfMeasure = new BehaviorSubject<ElectricalUnitOfMeasure>(ElectricalUnitOfMeasure.Current);
            var electricCurrentType = new BehaviorSubject<ElectricCurrentType>(ElectricCurrentType.AC);
            var range = new BehaviorSubject<Range>(Range.Auto);
            var resolution = new BehaviorSubject<Resolution>(Resolution.Def);

            var measuringInstrument = Substitute.For<IElectricalMeasuringSubsystem>();
            measuringInstrument.ElectricalUnitOfMeasureBehaviorSubject.Returns(electricalUnitOfMeasure);
            measuringInstrument.ElectricCurrentTypeBehaviorSubject.Returns(electricCurrentType);
            measuringInstrument.RangeBehaviorSubject.Returns(range);
            measuringInstrument.ResolutionBehaviorSubject.Returns(resolution);

            // Act
            var result = ConfigureVoltage.OfDeviceAccordingToGivenParameters(
                measuringInstrument,
                ElectricCurrentType.DC,
                Option<Range>.None,
                Option<Resolution>.None);
            
            // Assert
            result.Should().BeOfType<Ok<Unit>>();
            range.Value.Should().Be(Range.Auto);
            resolution.Value.Should().Be(Resolution.Def);
            electricCurrentType.Value.Should().Be(ElectricCurrentType.DC);
            electricalUnitOfMeasure.Value.Should().Be(ElectricalUnitOfMeasure.Voltage);
        }
        
        [TestMethod]
        public void TestOfDeviceAccordingToGivenParameters_NonDefaults()
        {
            // Arrange
            var electricalUnitOfMeasure = new BehaviorSubject<ElectricalUnitOfMeasure>(ElectricalUnitOfMeasure.Current);
            var electricCurrentType = new BehaviorSubject<ElectricCurrentType>(ElectricCurrentType.AC);
            var range = new BehaviorSubject<Range>(Range.Auto);
            var resolution = new BehaviorSubject<Resolution>(Resolution.Def);

            var measuringInstrument = Substitute.For<IElectricalMeasuringSubsystem>();
            measuringInstrument.ElectricalUnitOfMeasureBehaviorSubject.Returns(electricalUnitOfMeasure);
            measuringInstrument.ElectricCurrentTypeBehaviorSubject.Returns(electricCurrentType);
            measuringInstrument.RangeBehaviorSubject.Returns(range);
            measuringInstrument.ResolutionBehaviorSubject.Returns(resolution);

            // Act
            var result = ConfigureVoltage.OfDeviceAccordingToGivenParameters(
                measuringInstrument,
                ElectricCurrentType.AC,
                Range.Max,
                Resolution.Min);
            
            // Assert
            result.Should().BeOfType<Ok<Unit>>();
            range.Value.Should().Be(Range.Max);
            resolution.Value.Should().Be(Resolution.Min);
            electricCurrentType.Value.Should().Be(ElectricCurrentType.AC);
            electricalUnitOfMeasure.Value.Should().Be(ElectricalUnitOfMeasure.Voltage);
        }
    }
}