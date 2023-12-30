using System;
using System.Collections.Generic;
using System.Linq;
using LCTerminalSlots.Models;

namespace LCTerminalSlots.Utils
{
    public static class SlotsGenerator
    {
        public static List<SlotsEnum> GenerateSlots(int length)
        {
            var slotList = new List<SlotsEnum>();

            for (int i = 0; i < length; i++)
            {
                var number = BetterRandom.GetRandomSlot(Enum.GetNames(typeof(SlotsEnum)).Length);
                slotList.Add(number.ToEnum<SlotsEnum>());
            }

            return slotList;
        }

        public static bool CheckSlotsEqual(List<SlotsEnum> inputSlots)
        {
            var appearanceCount = inputSlots.Count(x => x == inputSlots[0]);

            return appearanceCount == 3;
        }

        public static bool CheckHalfWin(List<SlotsEnum> inputSlots)
        {
            return inputSlots.GroupBy(slot => slot).Count(grp => grp.Count() == 2) == 1;
        }
    }
}
