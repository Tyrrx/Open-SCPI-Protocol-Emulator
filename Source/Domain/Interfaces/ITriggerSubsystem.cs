using System.Reactive.Subjects;
using Domain.UnionTypes;

namespace Domain.Interfaces
{
    public interface ITriggerSubsystem
    {
        public BehaviorSubject<TriggerState> TriggerStateBehaviourSubject { get; }
    }
}