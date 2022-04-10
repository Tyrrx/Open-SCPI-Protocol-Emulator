using Domain.Keysight34465A;
using Domain.Keysight3458A;
using Domain.UnionTypes;

namespace Domain
{
    public static class FromConfiguration
    {
        public static double GetRangeValueByElectricalUnitOfMeasureAndRange(
            this Keysight34465AConfiguration configuration,
            ElectricalUnitOfMeasure electricalUnitOfMeasure,
            Range range)
        {
            return electricalUnitOfMeasure.Match(
                _ => range.Match(
                    number => number.Value,
                    _ => configuration.VoltageRangeAuto,
                    _ => configuration.VoltageRangeMin,
                    _ => configuration.VoltageRangeMax,
                    _ => configuration.VoltageRangeDef),
                _ => range.Match(
                    number => number.Value,
                    _ => configuration.CurrentRangeAuto,
                    _ => configuration.CurrentRangeMin,
                    _ => configuration.CurrentRangeMax,
                    _ => configuration.CurrentRangeDef));
        }
        
        public static double GetRangeValueByElectricalUnitOfMeasureAndRange(
            this Keysight3458AConfiguration configuration,
            ElectricalUnitOfMeasure electricalUnitOfMeasure,
            Range range)
        {
            return electricalUnitOfMeasure.Match(
                _ => range.Match(
                    number => number.Value,
                    _ => configuration.VoltageRangeAuto,
                    _ => configuration.VoltageRangeMin,
                    _ => configuration.VoltageRangeMax,
                    _ => configuration.VoltageRangeDef),
                _ => range.Match(
                    number => number.Value,
                    _ => configuration.CurrentRangeAuto,
                    _ => configuration.CurrentRangeMin,
                    _ => configuration.CurrentRangeMax,
                    _ => configuration.CurrentRangeDef));
        }

        public static double GetImpedanceMultiplierByImpedance(
            this Keysight34465AConfiguration configuration,
            Impedance impedance)
        {
            return impedance.Match(
                _ => configuration.HighImpedanceInterferenceMultiplier,
                _ => configuration.LowImpedanceInterferenceMultiplier);
        }
    }
}