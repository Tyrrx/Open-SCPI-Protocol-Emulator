using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Compression;
using System.Reactive.Linq;
using Domain.Interfaces;
using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain
{
    public static class GenerateMeasurementsOnTriggerChanges
    {
        public static void ForMeasuringDevice(IKeysight34Series measuringDevice)
        {
            measuringDevice.TriggerStateBehaviourSubject
                .OfType<TriggerState.WaitForTrigger_>()
                .Subscribe(_ =>
                    RotateGeneratorQueueAndProduceMeasurement(
                        measuringDevice.GenerateMeasurementValue,
                        measuringDevice.GeneratorQueue,
                        measuringDevice.ReadingQueue));
        }
        
        public static void RotateGeneratorQueueAndProduceMeasurement(
            Func<double, double> valueGenerator,
            Queue<double> measuringDeviceGeneratorQueue,
            ConcurrentQueue<MeasurementValue> measuringDeviceReadingQueue)
        {
            if (measuringDeviceGeneratorQueue.TryDequeue(out var interference))
            {
                measuringDeviceReadingQueue.Enqueue(MeasurementValue.Double(valueGenerator(interference)));
                measuringDeviceGeneratorQueue.Enqueue(interference);
            }
            else
            {
                measuringDeviceReadingQueue.Enqueue(MeasurementValue.Double(valueGenerator(1.0)));
            }
        }
    }
}