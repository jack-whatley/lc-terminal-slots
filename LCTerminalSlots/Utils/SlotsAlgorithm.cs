using LCTerminalSlots.Patches;
using System.Text;

namespace LCTerminalSlots.Utils
{
    internal static class SlotsAlgorithm
    {
        internal static string GenerateSlotsCalculated(int betValue)
        {
            // TerminalAPI.GetCreditsCount()

            return "";
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
