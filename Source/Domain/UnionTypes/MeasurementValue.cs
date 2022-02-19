﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Domain.UnionTypes
{
    public abstract class MeasurementValue : IStringConvertible
    {
        public static MeasurementValue Double(double value) => new Double_(value);

        public abstract string ToOutputString(CultureInfo cultureInfo);

        public sealed class Double_ : MeasurementValue
        {
            public double Value { get; }
            public Double_(double value) : base(UnionCases.Double)
            {
                Value = value;
            }

            public override string ToOutputString(CultureInfo cultureInfo) // todo factory for double creation
            {
                return Value.ToString(cultureInfo);
            }

            private bool Equals(Double_ other)
            {
                return base.Equals(other) && Value.Equals(other.Value);
            }

            public override bool Equals(object obj)
            {
                return ReferenceEquals(this, obj) || obj is Double_ other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(base.GetHashCode(), Value);
            }
        }

        internal enum UnionCases
        {
            Double,
        }

        internal UnionCases UnionCase { get; }
        MeasurementValue(UnionCases unionCase) => UnionCase = unionCase;

        public override string ToString() => Enum.GetName(typeof(UnionCases), UnionCase) ?? UnionCase.ToString();
        bool Equals(MeasurementValue other) => UnionCase == other.UnionCase;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MeasurementValue)obj);
        }

        public override int GetHashCode() => (int)UnionCase;
    }

    public static class MeasurementValueExtension
    {
        public static T Match<T>(this MeasurementValue measurementValue, Func<MeasurementValue.Double_, T> @double)
        {
            switch (measurementValue.UnionCase)
            {
                case MeasurementValue.UnionCases.Double:
                    return @double((MeasurementValue.Double_)measurementValue);
                default:
                    throw new ArgumentException($"Unknown type derived from MeasurementValue: {measurementValue.GetType().Name}");
            }
        }

        public static async Task<T> Match<T>(this MeasurementValue measurementValue, Func<MeasurementValue.Double_, Task<T>> @double)
        {
            switch (measurementValue.UnionCase)
            {
                case MeasurementValue.UnionCases.Double:
                    return await @double((MeasurementValue.Double_)measurementValue).ConfigureAwait(false);
                default:
                    throw new ArgumentException($"Unknown type derived from MeasurementValue: {measurementValue.GetType().Name}");
            }
        }

        public static async Task<T> Match<T>(this Task<MeasurementValue> measurementValue, Func<MeasurementValue.Double_, T> @double) => (await measurementValue.ConfigureAwait(false)).Match(@double);
        public static async Task<T> Match<T>(this Task<MeasurementValue> measurementValue, Func<MeasurementValue.Double_, Task<T>> @double) => await(await measurementValue.ConfigureAwait(false)).Match(@double).ConfigureAwait(false);
    }
}