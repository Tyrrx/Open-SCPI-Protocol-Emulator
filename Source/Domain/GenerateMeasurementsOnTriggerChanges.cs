using System;
using System.Reactive.Linq;
using Domain.Interfaces;
using Domain.UnionTypes;

namespace Domain
{
    public static class GenerateMeasurementsOnTriggerChanges
    {
        public static void ForMeasuringDevice(IKeysight34Series measuringDevice)
        {
            measuringDevice.TriggerStateBehaviourSubject
                .OfType<TriggerState.WaitForTrigger_>()
                .Subscribe(_ =>
                {
                    var interference = measuringDevice.GeneratorQueue.Dequeue();
                    measuringDevice.ReadingQueue.Enqueue(
                        MeasurementValue.Double(
                            measuringDevice.GenerateMeasurementValue(interference)
                        )
                    );
                    measuringDevice.GeneratorQueue.Enqueue(interference);
                });
        }
    }
}