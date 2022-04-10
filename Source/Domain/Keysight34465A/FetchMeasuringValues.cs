using System.Collections.Concurrent;
using Domain.Interfaces;
using FunicularSwitch;

namespace Domain.Keysight34465A
{
    public static class FetchMeasuringValues
    {
        public static Result<Unit> FromDeviceIntoOutputQueue(Keysight34465A keysight34465A, ConcurrentQueue<IStringConvertible> queue)
        {
            foreach (var measurementValue in keysight34465A.ReadingQueue)
            {
                queue.Enqueue(measurementValue);
            }

            return Result.Ok(No.Thing);
        }
    }
}