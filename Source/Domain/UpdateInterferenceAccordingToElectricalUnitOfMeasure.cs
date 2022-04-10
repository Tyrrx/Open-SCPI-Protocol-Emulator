using System;
using System.Collections.Generic;
using Domain.Interfaces;

namespace Domain
{
    public static class UpdateInterferenceOnElectricalUnitOfMeasureChanges
    {
        public static void OfMeasuringDevice(IKeysight34Series keysight34465A)
        {
            keysight34465A.ElectricalUnitOfMeasureBehaviorSubject
                .Subscribe(electricalUnitOfMeasure =>
                {
                    keysight34465A.GeneratorQueue =
                        new Queue<double>(keysight34465A.GetInterferenceFactors(electricalUnitOfMeasure));
                });
        }
    }
}