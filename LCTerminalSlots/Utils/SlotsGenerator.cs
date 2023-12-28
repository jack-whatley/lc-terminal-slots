using System;
using System.Collections.Generic;

namespace LCTerminalSlots.Utils
{
    public class SlotsGenerator
    {
        public static List<T> GenerateSlots<T>(int length) where T : Enum
        {
            var slotList = new List<T>();

            for (int i = 0; i < length + 1; i++)
            {
                var number = BetterRandom.GetRandomSlot(Enum.GetNames(typeof(T)).Length);
                slotList.Add(number.ToEnum<T>());
            }

            return slotList;
        }
    }
}
