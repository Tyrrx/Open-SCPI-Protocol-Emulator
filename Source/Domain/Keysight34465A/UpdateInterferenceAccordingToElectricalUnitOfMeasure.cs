using System;
using System.Collections.Generic;
using Domain.UnionTypes;

namespace Domain.Keysight34465A
{
    public static class UpdateInterferenceOnElectricalUnitOfMeasureChanges
    {
        public static void OfMeasuringDevice(Keysight34465A keysight34465A)
        {
            keysight34465A.ElectricalUnitOfMeasureBehaviorSubject
                .Subscribe(measurementTypeValue =>
                {
                    var list = measurementTypeValue.Match(
                        _ =>
                            keysight34465A.Configuration.VoltageInterferenceFactors,
                        _ =>
                            keysight34465A.Configuration.CurrentInterferenceFactors);
                    keysight34465A.GeneratorQueue = new Queue<double>(list);
                });
        }
    }
}