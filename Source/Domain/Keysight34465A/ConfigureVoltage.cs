using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain.Keysight34465A
{
    public static class ConfigureVoltage
    {
        public static Result<Unit> OfDeviceAccordingToGivenParameters(
            Keysight34465A keysight34465A,
            ElectricCurrentType electricCurrentType,
            Option<Range> range,
            Option<Resolution> resolution)
        {
            keysight34465A.ElectricalUnitOfMeasureBehaviorSubject.OnNext(ElectricalUnitOfMeasure.Voltage);
            keysight34465A.ElectricCurrentTypeBehaviorSubject.OnNext(electricCurrentType);
            keysight34465A.RangeBehaviorSubject.OnNext(range.Match(
                some => some,
                () => Range.Auto));
            keysight34465A.ResolutionBehaviorSubject.OnNext(resolution.Match(some => some,
                () => Resolution.Def));
            return Result.Ok(No.Thing);
        }
    }
}