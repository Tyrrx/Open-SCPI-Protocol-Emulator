using Domain.UnionTypes;
using FunicularSwitch;

namespace Domain.Keysight34465A
{
    public static class DisplayText
    {
        public static Result<Unit> OfMeasuringDevice(string text, Keysight34465A keysight34465A)
        {
            keysight34465A.DisplayStateBehaviorSubject.OnNext(DisplayState.DisplayText(text));
            return Result.Ok(No.Thing);
        }
    }
}