using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.KeysightBase;
using Domain.UnionTypes;
using FunicularSwitch;
using Range = Domain.UnionTypes.Range;

namespace Domain.Keysight34465A
{
    public sealed class Keysight34465A : KeysightDeviceBase, IMeasuringInstrument
    {
        private readonly Keysight34465AConfiguration configuration;
        private static readonly TimeSpan DisplayVisibilityTime = TimeSpan.FromSeconds(5);

        private BehaviorSubject<ElectricalUnitOfMeasure> ElectricalUnitOfMeasureBehaviorSubject { get; }
        private BehaviorSubject<ElectricCurrentType> ElectricCurrentTypeBehaviorSubject { get; }
        private BehaviorSubject<Impedance> ImpedanceBehaviorSubject { get; }
        private BehaviorSubject<Range> RangeBehaviorSubject { get; }
        private BehaviorSubject<Resolution> ResolutionBehaviorSubject { get; }
        private BehaviorSubject<DisplayState> DisplayStateBehaviorSubject { get; }

        public Keysight34465A(Option<Keysight34465AConfiguration> optionalConfiguration)
        {
            configuration = optionalConfiguration
                .Match(
                    some => some,
                    () => throw new Exception("Tried to start a device without a configuration present"));
            ElectricalUnitOfMeasureBehaviorSubject = new BehaviorSubject<ElectricalUnitOfMeasure>(ElectricalUnitOfMeasure.Voltage);
            ElectricCurrentTypeBehaviorSubject = new BehaviorSubject<ElectricCurrentType>(ElectricCurrentType.DC);
            ImpedanceBehaviorSubject = new BehaviorSubject<Impedance>(Impedance.Low);
            RangeBehaviorSubject = new BehaviorSubject<Range>(Range.Auto);
            ResolutionBehaviorSubject = new BehaviorSubject<Resolution>(Resolution.Def);
            DisplayStateBehaviorSubject = new BehaviorSubject<DisplayState>(DisplayState.Empty);

            ElectricalUnitOfMeasureBehaviorSubject.Subscribe(measurementTypeValue =>
            {
                var list = measurementTypeValue.Match(
                    _ =>
                        configuration.VoltageInterferenceFactors,
                    _ =>
                        configuration.CurrentInterferenceFactors);
                GeneratorQueue = new Queue<double>(list);
            });
        }

        public Task<Result<ResponseValue>> GetIdentification()
        {
            return Task.FromResult(Result.Ok(ResponseValue.String(configuration.Identification)));
        }

        public Task<Result<Unit>> Abort()
        {
            TriggerStateBehaviourSubject.OnNext(TriggerState.Idle);
            return Task.FromResult(Result.Ok(No.Thing));
        }

        public Task<Result<Unit>> Initiate()
        {
            ReadingQueue.Clear();
            TriggerStateBehaviourSubject.OnNext(TriggerState.WaitForTrigger);
            TriggerStateBehaviourSubject.OnNext(TriggerState.Idle);
            return Task.FromResult(Result.Ok(No.Thing));
        }

        public Task<Result<Unit>> Fetch(ConcurrentQueue<IStringConvertible> queue)
        {
            foreach (var measurementValue in ReadingQueue)
            {
                queue.Enqueue(measurementValue);
            }

            return Task.FromResult(Result.Ok(No.Thing));
        }

        public Task<Result<Unit>> ConfigureCurrent(ElectricCurrentType electricCurrentType, Option<Range> range,
            Option<Resolution> resolution)
        {
            ElectricalUnitOfMeasureBehaviorSubject.OnNext(ElectricalUnitOfMeasure.Current);
            ElectricCurrentTypeBehaviorSubject.OnNext(electricCurrentType);
            RangeBehaviorSubject.OnNext(range.Match(
                some => some,
                () => Range.Auto));
            ResolutionBehaviorSubject.OnNext(resolution.Match(
                some => some,
                () => Resolution.Def));
            return Task.FromResult(Result.Ok(No.Thing));
        }

        public async Task<Result<Unit>> DisplayText(string text)
        {
            DisplayStateBehaviorSubject.OnNext(DisplayState.DisplayText(text));
            await Task.Delay(DisplayVisibilityTime).ConfigureAwait(false);
            return Result.Ok(No.Thing);
        }

        public Task<Result<Unit>> ClearDisplay()
        {
            DisplayStateBehaviorSubject.OnNext(DisplayState.Empty);
            return Task.FromResult(Result.Ok(No.Thing));
        }

        public Task<Result<Unit>> SetImpedance(Impedance impedance)
        {
            ImpedanceBehaviorSubject.OnNext(impedance);
            return Task.FromResult(Result.Ok(No.Thing));
        }

        public Task<Result<Unit>> ConfigureVoltage(ElectricCurrentType electricCurrentType, Option<Range> range,
            Option<Resolution> resolution)
        {
            ElectricalUnitOfMeasureBehaviorSubject.OnNext(ElectricalUnitOfMeasure.Voltage);
            ElectricCurrentTypeBehaviorSubject.OnNext(electricCurrentType);
            RangeBehaviorSubject.OnNext(range.Match(
                some => some,
                () => Range.Auto));
            ResolutionBehaviorSubject.OnNext(resolution.Match(some => some,
                () => Resolution.Def));
            return Task.FromResult(Result.Ok(No.Thing));
        }

        public IObservable<(ElectricCurrentType, Impedance, Range, Resolution, TriggerState, DisplayState)> State =>
            ElectricCurrentTypeBehaviorSubject.CombineLatest(
                ImpedanceBehaviorSubject,
                RangeBehaviorSubject,
                ResolutionBehaviorSubject,
                TriggerStateBehaviourSubject,
                DisplayStateBehaviorSubject);

        public override double CalculateNextMeasurementValue(double interference)
        {
            return CalculateMeasurementValue.ForRangeValueUsingMultipliers(
                configuration.GetRangeValueByElectricalUnitOfMeasureAndRange(
                    ElectricalUnitOfMeasureBehaviorSubject.Value,
                    RangeBehaviorSubject.Value),
                configuration.GetImpedanceMultiplierByImpedance(
                    ImpedanceBehaviorSubject.Value),
                interference);
        }

        string IMeasuringInstrument.GetIdentification()
        {
            return configuration.Identification;
        }
    }
}