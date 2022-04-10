using System.Collections.Concurrent;
using Domain.Interfaces;
using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain
{
    public static class FetchIdentification
    {
        public static Result<Unit> OfDeviceIntoOutputQueue(
            IMeasuringInstrument measuringInstrument,
            ConcurrentQueue<IStringConvertible> outputQueue)
        {
            outputQueue.Enqueue(
                ResponseValue.String(
                    measuringInstrument.GetIdentification()));
            return Result.Ok(No.Thing);
        }
    }
}