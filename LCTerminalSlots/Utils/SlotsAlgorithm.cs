using System;
using System.Linq;
using LCTerminalSlots.Models;
using LCTerminalSlots.Patches;
using System.Text;

namespace LCTerminalSlots.Utils
{
    internal static class SlotsAlgorithm
    {
        private static readonly MemoryStructure<double> PrevResults = new(15);

        internal static string GenerateSlotsCalculated(int betValue)
        {
            const double bigWin = 1;
            const double smallWin = 0.75;
            const double noWin = 0;

            var slots = new SlotsEnum[3];
            var losingAll = TerminalAPI.GetCreditsCount() - betValue <= 5;

            if (losingAll)
            {
                int ranEnum = BetterRandom.GetRandomSlot(Enum.GetValues(typeof(SlotsEnum)).Length);
                int chance = BetterRandom.GetRandomSlot(10) + 1; // 1 to 10

                switch (chance)
                {
                    case > 5:
                        slots = GenerateBigWin();
                        PrevResults.AddItem(bigWin);
                        break;
                    case > 3:
                        slots = GenerateSmallWin();
                        PrevResults.AddItem(smallWin);
                        break;
                    default:
                        slots = GenerateLoss();
                        PrevResults.AddItem(smallWin * 2);
                        break;
                }
            }
            else // look at memory and decide choice
            {
                // memory of 10 = all wins
                // memory of 4.5 = some wins
                // memory of less then 5 = terrible win rate

                double total = 0;
                foreach (double score in PrevResults.GetItems()) total += score;

                ModHelpers.Logger.LogInfo($"Score sum: {total}");

                if (total > 5)
                {
                    if (BetterRandom.GetRandomSlot(2) == 1)
                    {
                        slots = GenerateSmallWin();
                        PrevResults.AddItem(smallWin);
                    }
                    else
                    {
                        slots = GenerateLoss();
                        PrevResults.AddItem(noWin);
                    }
                }
                else if (total > 2.5) // medium scorer
                {
                    if (BetterRandom.GetRandomSlot(5) + 1 > 2)
                    {
                        slots = GenerateBigWin();
                        PrevResults.AddItem(bigWin);
                    }
                    else
                    {
                        slots = GenerateSmallWin();
                        PrevResults.AddItem(smallWin);
                    }
                }
                else
                {
                    slots = GenerateBigWin();
                    PrevResults.AddItem(bigWin * 2);
                }
            }

            return HandleOutput(betValue, slots);
        }
        
        internal static string GenerateSlotsTrueRandom(int betValue)
        {
            TerminalAPI.RemoveGroupCredits(betValue);

            var slots = SlotsGenerator.GenerateSlots(3);

            return HandleOutput(betValue, slots.ToArray());
        }

        private static string HandleOutput(int betValue, SlotsEnum[] slots)
        {
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

        private static SlotsEnum[] GenerateBigWin()
        {
            var ranEnum = BetterRandom.GetRandomSlot(Enum.GetValues(typeof(SlotsEnum)).Length).ToEnum<SlotsEnum>();

            var returnArr = new[] { ranEnum, ranEnum, ranEnum };

            return returnArr;
        }

        private static SlotsEnum[] GenerateSmallWin()
        {
            var randomEnum = BetterRandom.GetRandomSlot(Enum.GetValues(typeof(SlotsEnum)).Length).ToEnum<SlotsEnum>();
            var returnArr = new[] { randomEnum, randomEnum, randomEnum };
            var ranIndex = BetterRandom.GetRandomSlot(returnArr.Length);

            returnArr[ranIndex] = ((int)(randomEnum + 1) % returnArr.Length).ToEnum<SlotsEnum>();

            return returnArr;
        }

        private static SlotsEnum[] GenerateLoss()
        {
            var returnArr = new SlotsEnum[3];
            var slotsArr = Enum.GetValues(typeof(SlotsEnum)).Cast<SlotsEnum>().ToList();

            for (int i = 0; i < 3; i++)
            {
                int ranIndex = BetterRandom.GetRandomSlot(slotsArr.Count);

                returnArr[i] = slotsArr[ranIndex];
                slotsArr.RemoveAt(ranIndex);
            }

            return returnArr;
        }
    }
}
