using System.Collections.Concurrent;
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
    public class TestFetchIdentification
    {
        [TestMethod]
        public void TestOfDeviceIntoOutputQueue()
        {
            // Arrange

            var measuringInstrument = Substitute.For<IMeasuringInstrument>();
            var outputQueue = new ConcurrentQueue<IStringConvertible>();
            measuringInstrument.GetIdentification().Returns("test");

            // Act
            var result = FetchIdentification.OfDeviceIntoOutputQueue(measuringInstrument, outputQueue);

            // Assert
            result.Should().BeOfType<Ok<Unit>>();
            outputQueue.Should().ContainSingle(element => element.Equals(ResponseValue.String("test")));
        }
    }
}