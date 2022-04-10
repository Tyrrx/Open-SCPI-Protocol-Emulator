using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain.Keysight34465A
{
    public static class AbortMeasuring
    {
        public static Result<Unit> OfDevice(Keysight34465A keysight34465A)
        {
            keysight34465A.TriggerStateBehaviourSubject.OnNext(TriggerState.Idle);
            return Result.Ok(No.Thing);
        }
    }
}