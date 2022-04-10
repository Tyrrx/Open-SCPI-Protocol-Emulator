using Domain.Interfaces;
using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain
{
    public static class SetImpedance
    {
        public static Result<Unit> OfMeasuringDevice(IElectricalMeasuringSubsystem electricalMeasuringSubsystem, Impedance impedance)
        {
            electricalMeasuringSubsystem.ImpedanceBehaviorSubject.OnNext(impedance);
            return Result.Ok(No.Thing);
        }
    }
}