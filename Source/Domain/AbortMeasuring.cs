using Domain.Interfaces;
using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain
{
    public static class AbortMeasuring
    {
        public static Result<Unit> ByNotifying(ITriggerSubsystem triggerSubsystem)
        {
            triggerSubsystem.TriggerStateBehaviourSubject.OnNext(TriggerState.Idle);
            return Result.Ok(No.Thing);
        }
    }
}