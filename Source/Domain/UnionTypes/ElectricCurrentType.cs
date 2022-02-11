using System;
using System.Threading.Tasks;

namespace Domain.UnionTypes
{
    public abstract class ElectricCurrentType
    {
        public static readonly ElectricCurrentType AC = new AC_();
        public static readonly ElectricCurrentType DC = new DC_();

        public class AC_ : ElectricCurrentType
        {
            public AC_() : base(UnionCases.AC)
            {
            }
        }

        public class DC_ : ElectricCurrentType
        {
            public DC_() : base(UnionCases.DC)
            {
            }
        }

        internal enum UnionCases
        {
            AC,
            DC,
        }

        internal UnionCases UnionCase { get; }
        ElectricCurrentType(UnionCases unionCase) => UnionCase = unionCase;

        public override string ToString() => Enum.GetName(typeof(UnionCases), UnionCase) ?? UnionCase.ToString();
        bool Equals(ElectricCurrentType other) => UnionCase == other.UnionCase;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ElectricCurrentType)obj);
        }

        public override int GetHashCode() => (int)UnionCase;
    }

    public static class ElectricityTypeExtension
    {
        public static T Match<T>(this ElectricCurrentType electricCurrentType, Func<ElectricCurrentType.AC_, T> aC, Func<ElectricCurrentType.DC_, T> dC)
        {
            switch (electricCurrentType.UnionCase)
            {
                case ElectricCurrentType.UnionCases.AC:
                    return aC((ElectricCurrentType.AC_)electricCurrentType);
                case ElectricCurrentType.UnionCases.DC:
                    return dC((ElectricCurrentType.DC_)electricCurrentType);
                default:
                    throw new ArgumentException($"Unknown type derived from ElectricityType: {electricCurrentType.GetType().Name}");
            }
        }

        public static async Task<T> Match<T>(this ElectricCurrentType electricCurrentType, Func<ElectricCurrentType.AC_, Task<T>> aC, Func<ElectricCurrentType.DC_, Task<T>> dC)
        {
            switch (electricCurrentType.UnionCase)
            {
                case ElectricCurrentType.UnionCases.AC:
                    return await aC((ElectricCurrentType.AC_)electricCurrentType).ConfigureAwait(false);
                case ElectricCurrentType.UnionCases.DC:
                    return await dC((ElectricCurrentType.DC_)electricCurrentType).ConfigureAwait(false);
                default:
                    throw new ArgumentException($"Unknown type derived from ElectricityType: {electricCurrentType.GetType().Name}");
            }
        }

        public static async Task<T> Match<T>(this Task<ElectricCurrentType> electricityType, Func<ElectricCurrentType.AC_, T> aC, Func<ElectricCurrentType.DC_, T> dC) => (await electricityType.ConfigureAwait(false)).Match(aC, dC);
        public static async Task<T> Match<T>(this Task<ElectricCurrentType> electricityType, Func<ElectricCurrentType.AC_, Task<T>> aC, Func<ElectricCurrentType.DC_, Task<T>> dC) => await(await electricityType.ConfigureAwait(false)).Match(aC, dC).ConfigureAwait(false);
    }
}