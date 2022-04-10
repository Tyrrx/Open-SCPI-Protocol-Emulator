using System.Collections.Concurrent;
using Domain.Interfaces;
using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain.Keysight34465A
{
    public static class FetchIdentification
    {
        public static Result<Unit> OfDeviceIntoOutputQueue(
            Keysight34465A keysight34465A,
            ConcurrentQueue<IStringConvertible> outputQueue)
        {
            outputQueue.Enqueue(
                ResponseValue.String(
                    keysight34465A.Configuration.Identification));
            return Result.Ok(No.Thing);
        }
    }
}