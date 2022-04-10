using Domain.Interfaces;
using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain
{
    public static class InitiateMeasuring
    {
        public static Result<Unit> OfDevice(IKeysight34Series measuringDevice)
        {
            measuringDevice.ReadingQueue.Clear();
            measuringDevice.TriggerStateBehaviourSubject.OnNext(TriggerState.WaitForTrigger);
            measuringDevice.TriggerStateBehaviourSubject.OnNext(TriggerState.Idle);
            return Result.Ok(No.Thing);
        }
    }
}