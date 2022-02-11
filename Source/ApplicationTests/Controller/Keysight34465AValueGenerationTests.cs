using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Keysight34465A;
using Domain.UnionTypes;
using Emulator.Command;
using Emulator.CommandHandler;
using FluentAssertions;
using FunicularSwitch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Range = Domain.UnionTypes.Range;

namespace EmulatorTests.Controller
{
    [TestClass]
    public class Keysight34465AValueGenerationTests
    {
        private Keysight34465A Keysight34465A { get; set; }
        private Keysight34465ACommandHandler Keysight34465ACommandHandler { get; set; }
        private Keysight34465AConfiguration Configuration { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Configuration = new Keysight34465AConfiguration
            {
                VoltageInterferenceFactors = new List<double> {0, 0.001, 0.002, 0.001, 0, -0.001, -0.002, -0.001},
                CurrentInterferenceFactors = new List<double> {0, 0.001, 0.002, 0.001, 0, -0.001, -0.002, -0.001},
                LowImpedanceInterferenceMultiplier = 2,
                HighImpedanceInterferenceMultiplier = 1,

                VoltageRangeAuto = 300,
                VoltageRangeMin = 100,
                VoltageRangeMax = 1000,
                VoltageRangeDef = 500,

                CurrentRangeAuto = 0.01,
                CurrentRangeMin = 0.01,
                CurrentRangeMax = 0.01,
                CurrentRangeDef = 0.01,
            };

            Keysight34465A = new Keysight34465A(Configuration);
            Keysight34465ACommandHandler = new Keysight34465ACommandHandler();
        }

        [TestMethod]
        public async Task TestIdentification()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = await executor.Execute(Keysight34465ACommand.Identification)
                .ConfigureAwait(false);
            executor.GetOutputQueue().First().Should().BeOfType<ResponseValue.String_>().Which.Value.Should()
                .Be(Configuration.Identification);
        }

        [TestMethod]
        public async Task TestReadOrder()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = await executor
                .Execute(Keysight34465ACommand.ConfigureVoltage(ElectricCurrentType.DC, Range.Auto, Resolution.Def))
                .ConfigureAwait(false);

            var (electricityType, impedance, range, resolution, triggerState, displayState) =
                await Keysight34465A.State.FirstAsync();

            electricityType.Should().BeOfType<ElectricCurrentType.DC_>();
            impedance.Should().BeOfType<Impedance.Low_>();
            range.Should().BeOfType<Range.Auto_>();
            resolution.Should().BeOfType<Resolution.Def_>();
            triggerState.Should().BeOfType<TriggerState.Idle_>();
            displayState.Should().BeOfType<DisplayState.Empty_>();
            executor.GetOutputQueue().Should().BeEmpty();

            List<Task<Result<CommandExecutionResult<Keysight34465ACommand>>>> tasks =
                new List<Task<Result<CommandExecutionResult<Keysight34465ACommand>>>>();

            for (int i = 0; i < 200; i++)
            {
                tasks.Add(executor.Execute(Keysight34465ACommand.Read));
            }

            (await tasks
                    .Aggregate()
                    .ConfigureAwait(false))
                .Should()
                .BeOfType<Ok<List<CommandExecutionResult<Keysight34465ACommand>>>>();

            var outputQueue =
                executor
                    .GetOutputQueue()
                    .Select(e =>
                        ((MeasurementValue.Double_) e).Value)
                    .ToList();

            for (var i = 0; i < outputQueue.Count; i++)
            {
                outputQueue[i].Should().Be(CalculateExpectedValue(i));
            }
        }

        private double CalculateExpectedValue(int i)
        {
            var interferenceCount = Configuration.VoltageInterferenceFactors.Count;
            var rangeAuto = Configuration.VoltageRangeAuto;
            var interference =
                Configuration.VoltageInterferenceFactors[i % interferenceCount]
                * rangeAuto
                * Configuration.LowImpedanceInterferenceMultiplier;
            var value =
                rangeAuto + interference;
            Console.WriteLine(value);
            return value;
        }
    }
}