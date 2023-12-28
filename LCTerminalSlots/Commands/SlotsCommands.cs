using System.Text;
using LCTerminalSlots.Models;
using LCTerminalSlots.Patches;
using LCTerminalSlots.Utils;
using LethalAPI.LibTerminal.Attributes;

namespace LCTerminalSlots.Commands
{
    public class SlotsCommands
    {
        [TerminalCommand("slots", true)]
        [CommandInfo("Bet the amount of money specified.", "[AMOUNT]")]
        public string SlotsMain(string amount)
        {
            int.TryParse(amount, out int betValue);
            TerminalAPI.RemoveGroupCredits(betValue);

            var slots = SlotsGenerator.GenerateSlots<SlotsEnum>(3);
            int winnings = 0;

            if (SlotsGenerator.CheckSlotsEqual(slots)) winnings = betValue * ((int)slots[0] + 2);

            TerminalAPI.AddGroupCredits(winnings);

            var sb = new StringBuilder();

            sb.AppendLine($"You got {slots[0]} {slots[1]} {slots[2]}");
            sb.AppendLine("");
            sb.AppendLine($"You have won {winnings}");

            return sb.ToString();
        }
    }
}
