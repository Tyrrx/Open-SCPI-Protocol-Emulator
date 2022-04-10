using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Domain.Interfaces;
using Domain.UnionTypes;
using FunicularSwitch;
using Range = Domain.UnionTypes.Range;

namespace Domain.Keysight34465A
{
    public sealed class Keysight34465A : IKeysight34Series, IMeasuringInstrument
    {
        public readonly Keysight34465AConfiguration Configuration;

        public BehaviorSubject<TriggerState> TriggerStateBehaviourSubject { get; } = new(TriggerState.Idle);
        public ConcurrentQueue<MeasurementValue> ReadingQueue { get; } = new();
        public Queue<double> GeneratorQueue { get; set; } = new();

        public BehaviorSubject<ElectricalUnitOfMeasure> ElectricalUnitOfMeasureBehaviorSubject { get; } =
            new(ElectricalUnitOfMeasure.Voltage);

        public BehaviorSubject<ElectricCurrentType> ElectricCurrentTypeBehaviorSubject { get; } =
            new(ElectricCurrentType.DC);
        public BehaviorSubject<Impedance> ImpedanceBehaviorSubject { get; } = new(Impedance.Low);
        public BehaviorSubject<Range> RangeBehaviorSubject { get; } = new(Range.Auto);
        public BehaviorSubject<Resolution> ResolutionBehaviorSubject { get; } = new(Resolution.Def);
        public BehaviorSubject<DisplayState> DisplayStateBehaviorSubject { get; } = new(DisplayState.Empty);

        public Keysight34465A(Option<Keysight34465AConfiguration> optionalConfiguration)
        {
            Configuration = optionalConfiguration
                .Match(
                    some => some,
                    () => throw new Exception("Tried to start a device without a configuration present"));

            UpdateInterferenceOnElectricalUnitOfMeasureChanges.OfMeasuringDevice(this);
            GenerateMeasurementsOnTriggerChanges.ForMeasuringDevice(this);
        }

        string IMeasuringInstrument.GetIdentification()
        {
            return Configuration.Identification;
        }
    }
}