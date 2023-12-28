using System;

namespace LCTerminalSlots.Utils
{
    internal class ParamAssert
    {
        internal static void IsGreaterThanOrEqualToZero(int value)
        {
            if (value >= 0) return;

            throw new ArgumentOutOfRangeException(
                nameof(value),
                value, 
                "Amount of was outside of valid range, can't be below zero"); ;
        }

        internal static void IsNotNull(object? obj)
        {
            if (obj is not null) return;

            throw new ArgumentNullException(
                nameof(obj), 
                $"{nameof(IsNotNull)} threw exception as {nameof(obj)} was asserted to be not null when it is null");
        }
    }
}
