using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.KeysightBase;
using Domain.UnionTypes;
using FunicularSwitch;
using Range = Domain.UnionTypes.Range;

namespace Domain.Keysight3458A
{
	public class Keysight3458A : KeysightDeviceBase, IMeasuringInstrument
	{
		private readonly Keysight3458AConfiguration configurationProvider;

		private BehaviorSubject<ElectricalUnitOfMeasure> MeasurementTypeBehaviorSubject { get; }
		private BehaviorSubject<ElectricCurrentType> ElectricityTypeBehaviorSubject { get; }
		private BehaviorSubject<Range> RangeBehaviorSubject { get; }
		private BehaviorSubject<Resolution> ResolutionBehaviorSubject { get; }


		public Keysight3458A(Option<Keysight3458AConfiguration> optionalConfiguration)
		{
			configurationProvider = optionalConfiguration
				.Match(
					some => some,
					() => throw new Exception("Tried to start a device without a configuration present"));
			MeasurementTypeBehaviorSubject = new BehaviorSubject<ElectricalUnitOfMeasure>(ElectricalUnitOfMeasure.Voltage);
			ElectricityTypeBehaviorSubject = new BehaviorSubject<ElectricCurrentType>(ElectricCurrentType.DC);
			RangeBehaviorSubject = new BehaviorSubject<Range>(Range.Auto);
			ResolutionBehaviorSubject = new BehaviorSubject<Resolution>(Resolution.Def);

			MeasurementTypeBehaviorSubject.Subscribe(measurementTypeValue =>
			{
				var list = measurementTypeValue.Match(
					_ =>
						configurationProvider.VoltageInterferenceFactors,
					_ =>
						configurationProvider.CurrentInterferenceFactors);
				GeneratorQueue = new Queue<double>(list);
			});
		}

		public Task<Result<ResponseValue>> GetIdentification()
		{
			return Task.FromResult(Result.Ok(ResponseValue.String(configurationProvider.Identification)));
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

		public Task<Result<Unit>> ConfigureCurrent(ElectricCurrentType electricCurrentType, Option<Range> range, Option<Resolution> resolution)
		{
			MeasurementTypeBehaviorSubject.OnNext(ElectricalUnitOfMeasure.Current);
			ElectricityTypeBehaviorSubject.OnNext(electricCurrentType);
			RangeBehaviorSubject.OnNext(range.Match(
				some => some,
				() => Range.Auto));
			ResolutionBehaviorSubject.OnNext(resolution.Match(
				some => some,
				() => Resolution.Def));
			return Task.FromResult(Result<Unit>.Ok(No.Thing));
		}
		public Task<Result<Unit>> ConfigureVoltage(ElectricCurrentType electricCurrentType, Option<Range> range, Option<Resolution> resolution)
		{
			MeasurementTypeBehaviorSubject.OnNext(ElectricalUnitOfMeasure.Voltage);
			ElectricityTypeBehaviorSubject.OnNext(electricCurrentType);
			RangeBehaviorSubject.OnNext(range.Match(
				some => some,
				() => Range.Auto));
			ResolutionBehaviorSubject.OnNext(resolution.Match(some => some,
				() => Resolution.Def));
			return Task.FromResult(Result.Ok(No.Thing));
		}
		
		public override double CalculateNextMeasurementValue(double interference)
		{
			double GetRangeValue()
			{
				return MeasurementTypeBehaviorSubject.Value.Match(_ => RangeBehaviorSubject.Value.Match(
						number => number.Value,
						_ => configurationProvider.VoltageRangeAuto,
						_ => configurationProvider.VoltageRangeMin,
						_ => configurationProvider.VoltageRangeMax,
						_ => configurationProvider.VoltageRangeDef),
					_ => RangeBehaviorSubject.Value.Match(
						number => number.Value,
						_ => configurationProvider.CurrentRangeAuto,
						_ => configurationProvider.CurrentRangeMin,
						_ => configurationProvider.CurrentRangeMax,
						_ => configurationProvider.CurrentRangeDef));
			}

			var rangeValue = GetRangeValue();
			return rangeValue + interference * rangeValue;
		}

		string IMeasuringInstrument.GetIdentification()
		{
			return configurationProvider.Identification;
		}
	}
}