using System;
using System.Collections.Generic;
using System.Linq;
using LCTerminalSlots.Models;

namespace LCTerminalSlots.Utils
{
    public class SlotsGenerator
    {
        public static T[] GenerateSlots<T>(int length) where T : Enum
        {
            T[] slotList = new T[3];

            for (int i = 0; i < length + 1; i++)
            {
                var number = BetterRandom.GetRandomSlot(Enum.GetNames(typeof(T)).Length);
                slotList[i] = number.ToEnum<T>();
            }

            return slotList;
        }

        public static bool CheckSlotsEqual(SlotsEnum[] inputSlots)
        {
            int appearanceCount = inputSlots.Count(x => x == inputSlots[0]);

            return appearanceCount == 3;
        }
    }
}
