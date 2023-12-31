using System;
using System.Linq;
using LCTerminalSlots.Models;
using LCTerminalSlots.Patches;
using System.Text;

namespace LCTerminalSlots.Utils
{
    internal static class SlotsAlgorithm
    {
        private static readonly MemoryStructure<double> _prevResults = new(10);

        internal static string GenerateSlotsCalculated(int betValue)
        {
            var slots = new SlotsEnum[3];
            var losingAll = TerminalAPI.GetCreditsCount() - betValue <= 5;

            if (losingAll) // keep them addicted
            {
                int ranEnum = BetterRandom.GetRandomSlot(Enum.GetValues(typeof(SlotsEnum)).Length);
                int chance = BetterRandom.GetRandomSlot(10) + 1; // 1 to 10

                if (chance > 5)
                {
                    for (int i = 0; i < slots.Length; i++) slots[i] = ranEnum.ToEnum<SlotsEnum>();
                    _prevResults.AddItem(1.0);
                }
                else if (chance > 3)
                {
                    slots[0] = SlotsEnum.Bell;
                    for (int i = 1; i < slots.Length; i++) slots[i] = ranEnum.ToEnum<SlotsEnum>();
                    _prevResults.AddItem(0.5);
                }
                else
                {
                    _prevResults.AddItem(0.5);
                    return GenerateSlotsTrueRandom(betValue);
                }
            }
            else // look at memory and decide choice
            {
                // memory of 10 = all wins
                // memory of 4.5 = some wins
                // memory of less then 5 = terrible win rate

                double total = 0;
                foreach (double score in _prevResults.GetItems()) total += score;

                if (total > 5) // high scorer
                {
                    // coin flip between two or nothing
                    if (BetterRandom.GetRandomSlot(2) == 1)
                    {
                        int ranEnum = BetterRandom.GetRandomSlot(Enum.GetValues(typeof(SlotsEnum)).Length);

                        ranEnum = ranEnum > 1 ? ranEnum - 1 : ranEnum + 1;

                        slots[0] = ranEnum.ToEnum<SlotsEnum>();
                        for (int i = 1; i < slots.Length; i++) slots[i] = (ranEnum - 1).ToEnum<SlotsEnum>();
                        _prevResults.AddItem(0.5);
                    }
                    else
                    {
                        // TODO: randomise this
                        slots[0] = SlotsEnum.Bell;
                        slots[1] = SlotsEnum.Diamond;
                        slots[2] = SlotsEnum.Dollar;
                        _prevResults.AddItem(0);
                    }
                }
                else if (total > 2.5) // medium scorer
                {
                    if (BetterRandom.GetRandomSlot(5) + 1 > 2)
                    {
                        int ranEnum = BetterRandom.GetRandomSlot(Enum.GetValues(typeof(SlotsEnum)).Length);

                        for (int i = 0; i < slots.Length; i++) slots[i] = ranEnum.ToEnum<SlotsEnum>();
                        _prevResults.AddItem(1.0);
                    }
                    else
                    {
                        int ranEnum = BetterRandom.GetRandomSlot(Enum.GetValues(typeof(SlotsEnum)).Length);

                        slots[0] = SlotsEnum.Cherry;
                        for (int i = 1; i < slots.Length; i++) slots[i] = ranEnum.ToEnum<SlotsEnum>();
                        _prevResults.AddItem(0.5);
                    }
                }
                else // low scorer
                {
                    int ranEnum = BetterRandom.GetRandomSlot(Enum.GetValues(typeof(SlotsEnum)).Length);

                    for (int i = 0; i < slots.Length; i++) slots[i] = ranEnum.ToEnum<SlotsEnum>();
                    _prevResults.AddItem(1.0);
                }
            }

            TerminalAPI.RemoveGroupCredits(betValue);

            int winnings = 0; int multiplier = BetterRandom.GetRandomSlot(3) + 1;

            bool slotFullSet = SlotsGenerator.CheckSlotsEqual(slots.ToList());
            bool slotHalfWin = SlotsGenerator.CheckHalfWin(slots.ToList());

            if (slotFullSet) winnings = betValue * ((int)slots[0] + 1) * multiplier;
            if (slotHalfWin) winnings = (int)(betValue * double.Parse($"1.{(int)slots[0]}"));

            TerminalAPI.AddGroupCredits(winnings);

            var sb = new StringBuilder();

            sb.AppendLine($"You got {slots[0]} {slots[1]} {slots[2]}");
            sb.AppendLine("");
            sb.AppendLine($"You have won {winnings}");
            if (slotFullSet && multiplier > 1) sb.AppendLine($"Including a {multiplier}x multiplier");

            ChatAPI.SendServerMessage($"{GameNetworkManager.Instance.localPlayerController.playerUsername} bet {betValue} on slots and won {winnings}");

            return sb.ToString();
        }
        
        internal static string GenerateSlotsTrueRandom(int betValue)
        {
            TerminalAPI.RemoveGroupCredits(betValue);

            var slots = SlotsGenerator.GenerateSlots(3);
            int winnings = 0; int multiplier = BetterRandom.GetRandomSlot(3) + 1;

            bool slotFullSet = SlotsGenerator.CheckSlotsEqual(slots);
            bool slotHalfWin = SlotsGenerator.CheckHalfWin(slots);

            if (slotFullSet) winnings = betValue * ((int)slots[0] + 1) * multiplier;
            if (slotHalfWin) winnings = (int)(betValue * double.Parse($"1.{(int)slots[0]}"));

            TerminalAPI.AddGroupCredits(winnings);

            var sb = new StringBuilder();

            sb.AppendLine($"You got {slots[0]} {slots[1]} {slots[2]}");
            sb.AppendLine("");
            sb.AppendLine($"You have won {winnings}");
            if (slotFullSet && multiplier > 1) sb.AppendLine($"Including a {multiplier}x multiplier");

            ChatAPI.SendServerMessage($"{GameNetworkManager.Instance.localPlayerController.playerUsername} bet {betValue} on slots and won {winnings}");

            return sb.ToString();
        }
    }
}
