using System.Collections.Concurrent;
using Domain.Interfaces;
using FunicularSwitch;

namespace Domain
{
    public static class FetchMeasuringValues
    {
        public static Result<Unit> FromDeviceIntoOutputQueue(
            IValueGenerationSubsystem valueGenerationSubsystem,
            ConcurrentQueue<IStringConvertible> queue)
        {
            foreach (var measurementValue in valueGenerationSubsystem.ReadingQueue)
            {
                queue.Enqueue(measurementValue);
            }

            return Result.Ok(No.Thing);
        }
    }
}