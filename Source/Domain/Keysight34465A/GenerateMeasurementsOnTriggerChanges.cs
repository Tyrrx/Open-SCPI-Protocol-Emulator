using System;
using System.Reactive.Linq;
using Domain.UnionTypes;

namespace Domain.Keysight34465A
{
    public static class GenerateMeasurementsOnTriggerChanges
    {
        public static void ForMeasuringDevice(Keysight34465A keysight34465A)
        {
            keysight34465A.TriggerStateBehaviourSubject
                .OfType<TriggerState.WaitForTrigger_>()
                .Subscribe(_ =>
                {
                    var interference = keysight34465A.GeneratorQueue.Dequeue();
                    keysight34465A.ReadingQueue.Enqueue(
                        MeasurementValue.Double(
                            CalculateMeasurementValue.ForRangeValueUsingMultipliers(
                                keysight34465A.Configuration.GetRangeValueByElectricalUnitOfMeasureAndRange(
                                    keysight34465A.ElectricalUnitOfMeasureBehaviorSubject.Value,
                                    keysight34465A.RangeBehaviorSubject.Value),
                                keysight34465A.Configuration.GetImpedanceMultiplierByImpedance(
                                    keysight34465A.ImpedanceBehaviorSubject.Value),
                                interference)
                        )
                    );
                    keysight34465A.GeneratorQueue.Enqueue(interference);
                });
        }
    }
}