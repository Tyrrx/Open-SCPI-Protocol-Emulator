using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain.Keysight34465A
{
    public class ClearDisplay
    {
        public static Result<Unit> OfMeasuringDevice(Keysight34465A keysight34465A)
        {
            keysight34465A.DisplayStateBehaviorSubject.OnNext(DisplayState.Empty);
            return Result.Ok(No.Thing);
        }
    }
}