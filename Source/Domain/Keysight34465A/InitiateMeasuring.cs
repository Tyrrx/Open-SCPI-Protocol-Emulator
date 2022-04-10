using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain.Keysight34465A
{
    public static class InitiateMeasuring
    {
        public static Result<Unit> OfDevice(Keysight34465A keysight34465A)
        {
            keysight34465A.ReadingQueue.Clear();
            keysight34465A.TriggerStateBehaviourSubject.OnNext(TriggerState.WaitForTrigger);
            keysight34465A.TriggerStateBehaviourSubject.OnNext(TriggerState.Idle);
            return Result.Ok(No.Thing);
        }
    }
}