using System;
using System.Threading.Tasks;

namespace Domain.UnionTypes
{
    public abstract class ElectricalUnitOfMeasure
    {
        public static readonly ElectricalUnitOfMeasure Voltage = new Voltage_();
        public static readonly ElectricalUnitOfMeasure Current = new Current_();

        public sealed class Voltage_ : ElectricalUnitOfMeasure
        {
            public Voltage_() : base(UnionCases.Voltage)
            {
            }
        }

        public sealed class Current_ : ElectricalUnitOfMeasure
        {
            public Current_() : base(UnionCases.Current)
            {
            }
        }

        internal enum UnionCases
        {
            Voltage,
            Current,
        }

        internal UnionCases UnionCase { get; }
        ElectricalUnitOfMeasure(UnionCases unionCase) => UnionCase = unionCase;

        public override string ToString() => Enum.GetName(typeof(UnionCases), UnionCase) ?? UnionCase.ToString();
        bool Equals(ElectricalUnitOfMeasure other) => UnionCase == other.UnionCase;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ElectricalUnitOfMeasure)obj);
        }

        public override int GetHashCode() => (int)UnionCase;
    }

    public static class MeasurementTypeExtension
    {
        public static T Match<T>(this ElectricalUnitOfMeasure electricalUnitOfMeasure, Func<ElectricalUnitOfMeasure.Voltage_, T> voltage, Func<ElectricalUnitOfMeasure.Current_, T> current)
        {
            switch (electricalUnitOfMeasure.UnionCase)
            {
                case ElectricalUnitOfMeasure.UnionCases.Voltage:
                    return voltage((ElectricalUnitOfMeasure.Voltage_)electricalUnitOfMeasure);
                case ElectricalUnitOfMeasure.UnionCases.Current:
                    return current((ElectricalUnitOfMeasure.Current_)electricalUnitOfMeasure);
                default:
                    throw new ArgumentException($"Unknown type derived from MeasurementType: {electricalUnitOfMeasure.GetType().Name}");
            }
        }

        public static async Task<T> Match<T>(this ElectricalUnitOfMeasure electricalUnitOfMeasure, Func<ElectricalUnitOfMeasure.Voltage_, Task<T>> voltage, Func<ElectricalUnitOfMeasure.Current_, Task<T>> current)
        {
            switch (electricalUnitOfMeasure.UnionCase)
            {
                case ElectricalUnitOfMeasure.UnionCases.Voltage:
                    return await voltage((ElectricalUnitOfMeasure.Voltage_)electricalUnitOfMeasure).ConfigureAwait(false);
                case ElectricalUnitOfMeasure.UnionCases.Current:
                    return await current((ElectricalUnitOfMeasure.Current_)electricalUnitOfMeasure).ConfigureAwait(false);
                default:
                    throw new ArgumentException($"Unknown type derived from MeasurementType: {electricalUnitOfMeasure.GetType().Name}");
            }
        }

        public static async Task<T> Match<T>(this Task<ElectricalUnitOfMeasure> measurementType, Func<ElectricalUnitOfMeasure.Voltage_, T> voltage, Func<ElectricalUnitOfMeasure.Current_, T> current) => (await measurementType.ConfigureAwait(false)).Match(voltage, current);
        public static async Task<T> Match<T>(this Task<ElectricalUnitOfMeasure> measurementType, Func<ElectricalUnitOfMeasure.Voltage_, Task<T>> voltage, Func<ElectricalUnitOfMeasure.Current_, Task<T>> current) => await(await measurementType.ConfigureAwait(false)).Match(voltage, current).ConfigureAwait(false);
    }
}