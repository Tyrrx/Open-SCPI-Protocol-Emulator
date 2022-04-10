using System.Collections.Generic;
using Domain.UnionTypes;

namespace Domain.Interfaces
{
    public interface IKeysight34Series :
        ITriggerSubsystem,
        IValueGenerationSubsystem,
        IElectricalMeasuringSubsystem,
        IMeasuringInstrument
    {
        public List<double> GetInterferenceFactors(ElectricalUnitOfMeasure electricalUnitOfMeasure);
    }
}