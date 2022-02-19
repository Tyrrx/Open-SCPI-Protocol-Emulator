using System;
using System.Threading.Tasks;

namespace Domain.UnionTypes
{
    public abstract class DisplayState
    {
        public static DisplayState DisplayText(string textValue) => new DisplayText_(textValue);

        public static readonly DisplayState Empty = new Empty_();

        public sealed class DisplayText_ : DisplayState
        {
            public string TextValue { get; }
            public DisplayText_(string textValue) : base(UnionCases.DisplayText)
            {
                TextValue = textValue;
            }
        }

        public sealed class Empty_ : DisplayState
        {
            public Empty_() : base(UnionCases.Empty)
            {
            }
        }

        internal enum UnionCases
        {
            DisplayText,
            Empty,
        }

        internal UnionCases UnionCase { get; }
        DisplayState(UnionCases unionCase) => UnionCase = unionCase;

        public override string ToString() => Enum.GetName(typeof(UnionCases), UnionCase) ?? UnionCase.ToString();
        bool Equals(DisplayState other) => UnionCase == other.UnionCase;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DisplayState)obj);
        }

        public override int GetHashCode() => (int)UnionCase;
    }

    public static class DisplayStateExtension
    {
        public static T Match<T>(this DisplayState displayState, Func<DisplayState.DisplayText_, T> displayText, Func<DisplayState.Empty_, T> empty)
        {
            switch (displayState.UnionCase)
            {
                case DisplayState.UnionCases.DisplayText:
                    return displayText((DisplayState.DisplayText_)displayState);
                case DisplayState.UnionCases.Empty:
                    return empty((DisplayState.Empty_)displayState);
                default:
                    throw new ArgumentException($"Unknown type derived from DisplayState: {displayState.GetType().Name}");
            }
        }

        public static async Task<T> Match<T>(this DisplayState displayState, Func<DisplayState.DisplayText_, Task<T>> displayText, Func<DisplayState.Empty_, Task<T>> empty)
        {
            switch (displayState.UnionCase)
            {
                case DisplayState.UnionCases.DisplayText:
                    return await displayText((DisplayState.DisplayText_)displayState).ConfigureAwait(false);
                case DisplayState.UnionCases.Empty:
                    return await empty((DisplayState.Empty_)displayState).ConfigureAwait(false);
                default:
                    throw new ArgumentException($"Unknown type derived from DisplayState: {displayState.GetType().Name}");
            }
        }

        public static async Task<T> Match<T>(this Task<DisplayState> displayState, Func<DisplayState.DisplayText_, T> displayText, Func<DisplayState.Empty_, T> empty) => (await displayState.ConfigureAwait(false)).Match(displayText, empty);
        public static async Task<T> Match<T>(this Task<DisplayState> displayState, Func<DisplayState.DisplayText_, Task<T>> displayText, Func<DisplayState.Empty_, Task<T>> empty) => await(await displayState.ConfigureAwait(false)).Match(displayText, empty).ConfigureAwait(false);
    }
}