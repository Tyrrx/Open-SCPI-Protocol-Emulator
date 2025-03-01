﻿using System;
using System.Threading.Tasks;
using Domain.UnionTypes;
using FunicularSwitch;
using Range = Domain.UnionTypes.Range;

namespace Emulator.Command
{
	public abstract class Keysight3458ACommand
	{
		public static readonly Keysight3458ACommand Identification = new Identification_();
		public static readonly Keysight3458ACommand Read = new Read_();
		public static readonly Keysight3458ACommand Abort = new Abort_();
		public static readonly Keysight3458ACommand Initiate = new Initiate_();
		public static readonly Keysight3458ACommand Fetch = new Fetch_();

		public static Keysight3458ACommand ConfigureCurrent(ElectricCurrentType electricCurrentType, Option<Range> range,
			Option<Resolution> resolution) => new ConfigureCurrent_(electricCurrentType, range, resolution);

		public static Keysight3458ACommand MeasureCurrent(ElectricCurrentType electricCurrentType, Option<Range> range,
			Option<Resolution> resolution) => new MeasureCurrent_(electricCurrentType, range, resolution);

		public static Keysight3458ACommand ConfigureVoltage(ElectricCurrentType electricCurrentType, Option<Range> range,
			Option<Resolution> resolution) => new ConfigureVoltage_(electricCurrentType, range, resolution);

		public static Keysight3458ACommand MeasureVoltage(ElectricCurrentType electricCurrentType, Option<Range> range,
			Option<Resolution> resolution) => new MeasureVoltage_(electricCurrentType, range, resolution);

		public sealed class Identification_ : Keysight3458ACommand
		{
			public Identification_() : base(UnionCases.Identification)
			{
			}
		}

		public sealed class Read_ : Keysight3458ACommand
		{
			public Read_() : base(UnionCases.Read)
			{
			}
		}

		public sealed class Abort_ : Keysight3458ACommand
		{
			public Abort_() : base(UnionCases.Abort)
			{
			}
		}

		public sealed class Initiate_ : Keysight3458ACommand
		{
			public Initiate_() : base(UnionCases.Initiate)
			{
			}
		}

		public sealed class Fetch_ : Keysight3458ACommand
		{
			public Fetch_() : base(UnionCases.Fetch)
			{
			}
		}

		public sealed class ConfigureCurrent_ : Keysight3458ACommand
		{
			public ElectricCurrentType ElectricCurrentType { get; }
			public Option<Range> Range { get; }
			public Option<Resolution> Resolution { get; }

			public ConfigureCurrent_(ElectricCurrentType electricCurrentType, Option<Range> range,
				Option<Resolution> resolution) : base(UnionCases.ConfigureCurrent)
			{
				ElectricCurrentType = electricCurrentType;
				Range = range;
				Resolution = resolution;
			}

			private bool Equals(ConfigureCurrent_ other)
			{
				return base.Equals(other) && Equals(ElectricCurrentType, other.ElectricCurrentType) && Equals(Range, other.Range) && Equals(Resolution, other.Resolution);
			}

			public override bool Equals(object obj)
			{
				return ReferenceEquals(this, obj) || obj is ConfigureCurrent_ other && Equals(other);
			}

			public override int GetHashCode()
			{
				return HashCode.Combine(base.GetHashCode(), ElectricCurrentType, Range, Resolution);
			}
		}

		public sealed class MeasureCurrent_ : Keysight3458ACommand
		{
			public ElectricCurrentType ElectricCurrentType { get; }
			public Option<Range> Range { get; }
			public Option<Resolution> Resolution { get; }

			public MeasureCurrent_(ElectricCurrentType electricCurrentType, Option<Range> range, Option<Resolution> resolution)
				: base(UnionCases.MeasureCurrent)
			{
				ElectricCurrentType = electricCurrentType;
				Range = range;
				Resolution = resolution;
			}

			private bool Equals(MeasureCurrent_ other)
			{
				return base.Equals(other) && Equals(ElectricCurrentType, other.ElectricCurrentType) && Equals(Range, other.Range) && Equals(Resolution, other.Resolution);
			}

			public override bool Equals(object obj)
			{
				return ReferenceEquals(this, obj) || obj is MeasureCurrent_ other && Equals(other);
			}

			public override int GetHashCode()
			{
				return HashCode.Combine(base.GetHashCode(), ElectricCurrentType, Range, Resolution);
			}
		}

		public sealed class ConfigureVoltage_ : Keysight3458ACommand
		{
			public ElectricCurrentType ElectricCurrentType { get; }
			public Option<Range> Range { get; }
			public Option<Resolution> Resolution { get; }

			public ConfigureVoltage_(ElectricCurrentType electricCurrentType, Option<Range> range,
				Option<Resolution> resolution) : base(UnionCases.ConfigureVoltage)
			{
				ElectricCurrentType = electricCurrentType;
				Range = range;
				Resolution = resolution;
			}

			private bool Equals(ConfigureVoltage_ other)
			{
				return base.Equals(other) && Equals(ElectricCurrentType, other.ElectricCurrentType) && Equals(Range, other.Range) && Equals(Resolution, other.Resolution);
			}

			public override bool Equals(object obj)
			{
				return ReferenceEquals(this, obj) || obj is ConfigureVoltage_ other && Equals(other);
			}

			public override int GetHashCode()
			{
				return HashCode.Combine(base.GetHashCode(), ElectricCurrentType, Range, Resolution);
			}
		}

		public sealed class MeasureVoltage_ : Keysight3458ACommand
		{
			public ElectricCurrentType ElectricCurrentType { get; }
			public Option<Range> Range { get; }
			public Option<Resolution> Resolution { get; }

			public MeasureVoltage_(ElectricCurrentType electricCurrentType, Option<Range> range, Option<Resolution> resolution)
				: base(UnionCases.MeasureVoltage)
			{
				ElectricCurrentType = electricCurrentType;
				Range = range;
				Resolution = resolution;
			}

			private bool Equals(MeasureVoltage_ other)
			{
				return base.Equals(other) && Equals(ElectricCurrentType, other.ElectricCurrentType) && Equals(Range, other.Range) && Equals(Resolution, other.Resolution);
			}

			public override bool Equals(object obj)
			{
				return ReferenceEquals(this, obj) || obj is MeasureVoltage_ other && Equals(other);
			}

			public override int GetHashCode()
			{
				return HashCode.Combine(base.GetHashCode(), ElectricCurrentType, Range, Resolution);
			}
		}

		internal enum UnionCases
		{
			Identification,
			Read,
			Abort,
			Initiate,
			Fetch,
			ConfigureCurrent,
			MeasureCurrent,
			ConfigureVoltage,
			MeasureVoltage,
		}

		internal UnionCases UnionCase { get; }
		Keysight3458ACommand(UnionCases unionCase) => UnionCase = unionCase;

		public override string ToString() => Enum.GetName(typeof(UnionCases), UnionCase) ?? UnionCase.ToString();
		bool Equals(Keysight3458ACommand other) => UnionCase == other.UnionCase;

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Keysight3458ACommand) obj);
		}

		public override int GetHashCode() => (int) UnionCase;
	}

	public static class Keysight3458ACommandExtension
	{
		public static T Match<T>(this Keysight3458ACommand command34465A,
			Func<Keysight3458ACommand.Identification_, T> identification, Func<Keysight3458ACommand.Read_, T> read,
			Func<Keysight3458ACommand.Abort_, T> abort, Func<Keysight3458ACommand.Initiate_, T> initiate,
			Func<Keysight3458ACommand.Fetch_, T> fetch,
			Func<Keysight3458ACommand.ConfigureCurrent_, T> configureCurrent,
			Func<Keysight3458ACommand.MeasureCurrent_, T> measureCurrent,
			Func<Keysight3458ACommand.ConfigureVoltage_, T> configureVoltage,
			Func<Keysight3458ACommand.MeasureVoltage_, T> measureVoltage)
		{
			switch (command34465A.UnionCase)
			{
				case Keysight3458ACommand.UnionCases.Identification:
					return identification((Keysight3458ACommand.Identification_) command34465A);
				case Keysight3458ACommand.UnionCases.Read:
					return read((Keysight3458ACommand.Read_) command34465A);
				case Keysight3458ACommand.UnionCases.Abort:
					return abort((Keysight3458ACommand.Abort_) command34465A);
				case Keysight3458ACommand.UnionCases.Initiate:
					return initiate((Keysight3458ACommand.Initiate_) command34465A);
				case Keysight3458ACommand.UnionCases.Fetch:
					return fetch((Keysight3458ACommand.Fetch_) command34465A);
				case Keysight3458ACommand.UnionCases.ConfigureCurrent:
					return configureCurrent((Keysight3458ACommand.ConfigureCurrent_) command34465A);
				case Keysight3458ACommand.UnionCases.MeasureCurrent:
					return measureCurrent((Keysight3458ACommand.MeasureCurrent_) command34465A);
				case Keysight3458ACommand.UnionCases.ConfigureVoltage:
					return configureVoltage((Keysight3458ACommand.ConfigureVoltage_) command34465A);
				case Keysight3458ACommand.UnionCases.MeasureVoltage:
					return measureVoltage((Keysight3458ACommand.MeasureVoltage_) command34465A);
				default:
					throw new ArgumentException(
						$"Unknown type derived from Keysight34465ACommand: {command34465A.GetType().Name}");
			}
		}

		public static async Task<T> Match<T>(this Keysight3458ACommand command34465A,
			Func<Keysight3458ACommand.Identification_, Task<T>> identification,
			Func<Keysight3458ACommand.Read_, Task<T>> read, Func<Keysight3458ACommand.Abort_, Task<T>> abort,
			Func<Keysight3458ACommand.Initiate_, Task<T>> initiate, Func<Keysight3458ACommand.Fetch_, Task<T>> fetch,
			Func<Keysight3458ACommand.ConfigureCurrent_, Task<T>> configureCurrent,
			Func<Keysight3458ACommand.MeasureCurrent_, Task<T>> measureCurrent,
			Func<Keysight3458ACommand.ConfigureVoltage_, Task<T>> configureVoltage,
			Func<Keysight3458ACommand.MeasureVoltage_, Task<T>> measureVoltage)
		{
			switch (command34465A.UnionCase)
			{
				case Keysight3458ACommand.UnionCases.Identification:
					return await identification((Keysight3458ACommand.Identification_) command34465A)
						.ConfigureAwait(false);
				case Keysight3458ACommand.UnionCases.Read:
					return await read((Keysight3458ACommand.Read_) command34465A).ConfigureAwait(false);
				case Keysight3458ACommand.UnionCases.Abort:
					return await abort((Keysight3458ACommand.Abort_) command34465A).ConfigureAwait(false);
				case Keysight3458ACommand.UnionCases.Initiate:
					return await initiate((Keysight3458ACommand.Initiate_) command34465A).ConfigureAwait(false);
				case Keysight3458ACommand.UnionCases.Fetch:
					return await fetch((Keysight3458ACommand.Fetch_) command34465A).ConfigureAwait(false);
				case Keysight3458ACommand.UnionCases.ConfigureCurrent:
					return await configureCurrent((Keysight3458ACommand.ConfigureCurrent_) command34465A)
						.ConfigureAwait(false);
				case Keysight3458ACommand.UnionCases.MeasureCurrent:
					return await measureCurrent((Keysight3458ACommand.MeasureCurrent_) command34465A)
						.ConfigureAwait(false);
				case Keysight3458ACommand.UnionCases.ConfigureVoltage:
					return await configureVoltage((Keysight3458ACommand.ConfigureVoltage_) command34465A)
						.ConfigureAwait(false);
				case Keysight3458ACommand.UnionCases.MeasureVoltage:
					return await measureVoltage((Keysight3458ACommand.MeasureVoltage_) command34465A)
						.ConfigureAwait(false);
				default:
					throw new ArgumentException(
						$"Unknown type derived from Keysight34465ACommand: {command34465A.GetType().Name}");
			}
		}

		public static async Task<T> Match<T>(this Task<Keysight3458ACommand> command,
			Func<Keysight3458ACommand.Identification_, T> identification, Func<Keysight3458ACommand.Read_, T> read,
			Func<Keysight3458ACommand.Abort_, T> abort, Func<Keysight3458ACommand.Initiate_, T> initiate,
			Func<Keysight3458ACommand.Fetch_, T> fetch,
			Func<Keysight3458ACommand.ConfigureCurrent_, T> configureCurrent,
			Func<Keysight3458ACommand.MeasureCurrent_, T> measureCurrent,
			Func<Keysight3458ACommand.ConfigureVoltage_, T> configureVoltage,
			Func<Keysight3458ACommand.MeasureVoltage_, T> measureVoltage) => (await command.ConfigureAwait(false)).Match(
			identification, read, abort, initiate, fetch, configureCurrent, measureCurrent, configureVoltage,
			measureVoltage);

		public static async Task<T> Match<T>(this Task<Keysight3458ACommand> command,
			Func<Keysight3458ACommand.Identification_, Task<T>> identification,
			Func<Keysight3458ACommand.Read_, Task<T>> read, Func<Keysight3458ACommand.Abort_, Task<T>> abort,
			Func<Keysight3458ACommand.Initiate_, Task<T>> initiate, Func<Keysight3458ACommand.Fetch_, Task<T>> fetch,
			Func<Keysight3458ACommand.ConfigureCurrent_, Task<T>> configureCurrent,
			Func<Keysight3458ACommand.MeasureCurrent_, Task<T>> measureCurrent,
			Func<Keysight3458ACommand.ConfigureVoltage_, Task<T>> configureVoltage,
			Func<Keysight3458ACommand.MeasureVoltage_, Task<T>> measureVoltage) =>
			await (await command.ConfigureAwait(false)).Match(identification, read, abort, initiate, fetch,
				configureCurrent, measureCurrent, configureVoltage, measureVoltage).ConfigureAwait(false);
	}
}