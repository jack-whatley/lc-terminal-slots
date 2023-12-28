using System;

namespace LCTerminalSlots.Utils
{
    public static class IntegerExtensions
    {
        public static T ToEnum<T>(this int value) where T : Enum
        {
            if (!Enum.IsDefined(typeof(T), value))
                throw new ArgumentException($"Value {value} is not defined in type {typeof(T)}", nameof(value));

            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}
