using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
    public class Keysight34465AStateTests
    {
        private Keysight34465A Keysight34465A { get; set; }
        private Keysight34465ACommandHandler Keysight34465ACommandHandler { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            var configuration = new Keysight34465AConfiguration
            {
                Identification = "bla",
                Port = 5025,
                Ip = "127.0.0.1",
                VoltageInterferenceFactors = new List<double> {0.3, 0, 0.3},
                CurrentInterferenceFactors = new List<double> {0.1, 0, 0.1},
                LowImpedanceInterferenceMultiplier = 10,
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

            Keysight34465A = new Keysight34465A(configuration);
            Keysight34465ACommandHandler = new Keysight34465ACommandHandler();
        }

        [TestMethod]
        public async Task ConfigureVoltage_WithDefaultParameters()
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
        }

        [TestMethod]
        public async Task ConfigureVoltage_WithoutOptionalParameters()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = await executor
                .Execute(Keysight34465ACommand.ConfigureVoltage(ElectricCurrentType.DC, Option<Range>.None,
                    Option<Resolution>.None))
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
        }


        [TestMethod]
        public async Task MeasureVoltage_WithDefaultParameters()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);

            var result = await executor
                .Execute(Keysight34465ACommand.MeasureVoltage(ElectricCurrentType.DC, Range.Auto, Resolution.Def))
                .ConfigureAwait(true);

            var (electricityType, impedance, range, resolution, triggerState, displayState) =
                await Keysight34465A.State.FirstAsync();

            electricityType.Should().BeOfType<ElectricCurrentType.DC_>();
            impedance.Should().BeOfType<Impedance.Low_>();
            range.Should().BeOfType<Range.Auto_>();
            resolution.Should().BeOfType<Resolution.Def_>();
            triggerState.Should().BeOfType<TriggerState.Idle_>();
            displayState.Should().BeOfType<DisplayState.Empty_>();
            executor.GetOutputQueue().First().Should().BeOfType<MeasurementValue.Double_>();
        }

        [TestMethod]
        public async Task MeasureVoltage_WithoutOptionalParameters()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = await executor
                .Execute(Keysight34465ACommand.MeasureVoltage(ElectricCurrentType.DC, Option<Range>.None,
                    Option<Resolution>.None))
                .ConfigureAwait(false);
            var (electricityType, impedance, range, resolution, triggerState, displayState) =
                await Keysight34465A.State.FirstAsync();


            electricityType.Should().BeOfType<ElectricCurrentType.DC_>();
            impedance.Should().BeOfType<Impedance.Low_>();
            range.Should().BeOfType<Range.Auto_>();
            resolution.Should().BeOfType<Resolution.Def_>();
            triggerState.Should().BeOfType<TriggerState.Idle_>();
            displayState.Should().BeOfType<DisplayState.Empty_>();
            executor.GetOutputQueue().First().Should().BeOfType<MeasurementValue.Double_>();
        }

        [TestMethod]
        public async Task ConfigureCurrent_WithDefaultParameters()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = await executor
                .Execute(Keysight34465ACommand.ConfigureCurrent(ElectricCurrentType.DC, Range.Auto, Resolution.Def))
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
        }

        [TestMethod]
        public async Task ConfigureCurrent_WithoutOptionalParameters()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = await executor
                .Execute(Keysight34465ACommand.ConfigureCurrent(ElectricCurrentType.DC, Option<Range>.None,
                    Option<Resolution>.None))
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
        }

        [TestMethod]
        public async Task MeasureCurrent_WithDefaultParameters()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = await executor
                .Execute(Keysight34465ACommand.MeasureCurrent(ElectricCurrentType.DC, Range.Auto, Resolution.Def))
                .ConfigureAwait(false);
            var (electricityType, impedance, range, resolution, triggerState, displayState) =
                await Keysight34465A.State.FirstAsync();


            electricityType.Should().BeOfType<ElectricCurrentType.DC_>();
            impedance.Should().BeOfType<Impedance.Low_>();
            range.Should().BeOfType<Range.Auto_>();
            resolution.Should().BeOfType<Resolution.Def_>();
            triggerState.Should().BeOfType<TriggerState.Idle_>();
            displayState.Should().BeOfType<DisplayState.Empty_>();
            executor.GetOutputQueue().First().Should().BeOfType<MeasurementValue.Double_>();
        }

        [TestMethod]
        public async Task MeasureCurrent_WithoutOptionalParameters()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = await executor
                .Execute(Keysight34465ACommand.MeasureCurrent(ElectricCurrentType.DC, Option<Range>.None,
                    Option<Resolution>.None))
                .ConfigureAwait(false);
            var (electricityType, impedance, range, resolution, triggerState, displayState) =
                await Keysight34465A.State.FirstAsync();


            electricityType.Should().BeOfType<ElectricCurrentType.DC_>();
            impedance.Should().BeOfType<Impedance.Low_>();
            range.Should().BeOfType<Range.Auto_>();
            resolution.Should().BeOfType<Resolution.Def_>();
            triggerState.Should().BeOfType<TriggerState.Idle_>();
            displayState.Should().BeOfType<DisplayState.Empty_>();
            executor.GetOutputQueue().First().Should().BeOfType<MeasurementValue.Double_>();
        }

        [TestMethod]
        public async Task Read_WithDefaults()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = await executor.Execute(Keysight34465ACommand.Read).ConfigureAwait(false);
            var (electricityType, impedance, range, resolution, triggerState, displayState) =
                await Keysight34465A.State.FirstAsync();

            electricityType.Should().BeOfType<ElectricCurrentType.DC_>();
            impedance.Should().BeOfType<Impedance.Low_>();
            range.Should().BeOfType<Range.Auto_>();
            resolution.Should().BeOfType<Resolution.Def_>();
            triggerState.Should().BeOfType<TriggerState.Idle_>();
            displayState.Should().BeOfType<DisplayState.Empty_>();
            executor.GetOutputQueue().First().Should().BeOfType<MeasurementValue.Double_>();
        }

        [TestMethod]
        public void TestInitiate_ReadingQueueClear_AndOutputEmpty_WhenReadingQueueIsNotEmpty()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            executor.Execute(Keysight34465ACommand.Initiate);
            executor.Execute(Keysight34465ACommand.Initiate);
            Keysight34465A.ReadingQueue.Single().Should().BeOfType<MeasurementValue.Double_>();
            executor.GetOutputQueue().Should().BeEmpty();
        }

        [TestMethod]
        public void TestFetch_ReadingQueueNotChanged_AndOutputQueueEnqueue()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            executor.Execute(Keysight34465ACommand.Initiate);
            Keysight34465A.ReadingQueue.Should().ContainSingle();
            executor.GetOutputQueue().Should().BeEmpty();
            executor.Execute(Keysight34465ACommand.Fetch);
            Keysight34465A.ReadingQueue.Should().ContainSingle();
            executor.GetOutputQueue().Should().ContainSingle();
        }

        [TestMethod]
        public async Task TestSetImpedance_High()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = await executor.Execute(Keysight34465ACommand.SetImpedance(Impedance.High)).ConfigureAwait(false);
            var (electricityType, impedance, range, resolution, triggerState, displayState) =
                await Keysight34465A.State.FirstAsync();
            impedance.Should().BeOfType<Impedance.High_>();
        }

        [TestMethod]
        public async Task TestAbort()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = await executor.Execute(Keysight34465ACommand.Abort)
                .ConfigureAwait(false);
            var (electricityType, impedance, range, resolution, triggerState, displayState) =
                await Keysight34465A.State.FirstAsync();
            triggerState.Should().BeOfType<TriggerState.Idle_>();
        }

        [TestMethod]
        public async Task TestDisplayText_AutoClearAfterDelay()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var helloWorld = "Hello world!";
            var result = executor.Execute(Keysight34465ACommand.DisplayText(helloWorld));
            var (_, _, _, _, _, displayState) = await Keysight34465A.State.FirstAsync();
            displayState.Should().BeOfType<DisplayState.DisplayText_>().Which.TextValue.Should().Be(helloWorld);
            await result.ConfigureAwait(true);

            (_, _, _, _, _, displayState) = await Keysight34465A.State.FirstAsync();
            displayState.Should().BeOfType<DisplayState.Empty_>();
        }

        [TestMethod]
        public void TestIdentification()
        {
            var executor = new CommandExecutionAdapter<Keysight34465A, Keysight34465ACommand>(Keysight34465ACommandHandler, Keysight34465A);
            var result = executor.Execute(Keysight34465ACommand.Identification);
            executor.GetOutputQueue().Should().ContainSingle().Which.Should().BeOfType<ResponseValue.String_>();
        }
    }
}