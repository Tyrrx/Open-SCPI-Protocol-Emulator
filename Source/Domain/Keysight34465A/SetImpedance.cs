using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain.Keysight34465A
{
    public static class SetImpedance
    {
        public static Result<Unit> OfMeasuringDevice(Keysight34465A keysight34465A, Impedance impedance)
        {
            keysight34465A.ImpedanceBehaviorSubject.OnNext(impedance);
            return Result.Ok(No.Thing);
        }
    }
}