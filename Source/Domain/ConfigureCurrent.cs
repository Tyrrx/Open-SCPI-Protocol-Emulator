using Domain.Interfaces;
using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain
{
    public static class ConfigureCurrent
    {
        public static Result<Unit> OfDeviceAccordingToGivenParameters(
            IElectricalMeasuringSubsystem electricalMeasuringSubsystem,
            ElectricCurrentType electricCurrentType,
            Option<Range> range,
            Option<Resolution> resolution)
        {
            electricalMeasuringSubsystem.ElectricalUnitOfMeasureBehaviorSubject.OnNext(ElectricalUnitOfMeasure.Current);
            electricalMeasuringSubsystem.ElectricCurrentTypeBehaviorSubject.OnNext(electricCurrentType);
            electricalMeasuringSubsystem.RangeBehaviorSubject.OnNext(range.Match(
                some => some,
                () => Range.Auto));
            electricalMeasuringSubsystem.ResolutionBehaviorSubject.OnNext(resolution.Match(
                some => some,
                () => Resolution.Def));
            return Result.Ok(No.Thing);
        }
    }
}