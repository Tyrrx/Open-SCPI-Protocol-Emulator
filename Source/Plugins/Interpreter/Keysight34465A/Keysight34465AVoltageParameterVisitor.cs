﻿using System.Globalization;
using Domain.UnionTypes;
using FunicularSwitch;
using ProtocolParser.Keysight34465A;

namespace EmulatorHost.Interpreter.Keysight34465A
{
    public class Keysight34465AVoltageParameterVisitor : Keysight34465ASCPIBaseVisitor<(Option<Range>, Option<Resolution>)>
    {
        public override (Option<Range>, Option<Resolution>) VisitVoltageParameters(Keysight34465ASCPIParser.VoltageParametersContext context)
        {
            var range = Option<Range>.None;
            if (context.range != null)
            {
                var text = context.range.Text;
                range = context.range.Type switch
                {
                    Keysight34465ASCPILexer.Number => Option.Some(Range.Number(ToDouble(context.range.Text))),
                    Keysight34465ASCPILexer.AUTO => Option.Some(Range.Auto),
                    Keysight34465ASCPILexer.MIN => Option.Some(Range.Min),
                    Keysight34465ASCPILexer.MAX => Option.Some(Range.Max),
                    Keysight34465ASCPILexer.DEF => Option.Some(Range.Def),
                    _ => Option<Range>.None
                };
            }

            var resolution = Option.None<Resolution>();
            if (context.resolution != null)
            {
                resolution = context.resolution.Type switch
                {
                    Keysight34465ASCPILexer.Number => Option.Some(Resolution.Number(ToDouble(context.resolution.Text))),
                    Keysight34465ASCPILexer.MIN => Option.Some(Resolution.Min),
                    Keysight34465ASCPILexer.MAX => Option.Some(Resolution.Max),
                    Keysight34465ASCPILexer.DEF => Option.Some(Resolution.Def),
                    _ => Option<Resolution>.None
                };
            }

            return (range, resolution);
        }

        private static double ToDouble(string input)
        {
            return decimal.ToDouble(decimal.Parse(input, NumberStyles.Float, new CultureInfo("en-US")));
        }

        protected override (Option<Range>, Option<Resolution>) DefaultResult { get; } =
            (Option<Range>.None, Option<Resolution>.None);
    }
}