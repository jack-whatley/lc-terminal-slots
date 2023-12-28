using System;
using System.Linq;
using System.Text;
using LCTerminalSlots.Models;
using LCTerminalSlots.Patches;
using LCTerminalSlots.Utils;
using LethalAPI.LibTerminal.Attributes;

namespace LCTerminalSlots.Commands
{
    public class SlotsCommands
    {
        [TerminalCommand("slothelp", true)]
        public string SlotsHelp()
        {
            var sb = new StringBuilder();
            sb.AppendLine(">SLOTSHELP");
            sb.AppendLine("This command.");
            sb.AppendLine(">SLOTS [AMOUNT]");
            sb.AppendLine("Bet the amount of money specified.");

            return sb.ToString();
        }

        [TerminalCommand("slots", true)]
        [CommandInfo("Bet the amount of money specified.", "[AMOUNT]")]
        public string SlotsMain(string amount)
        {
            int.TryParse(amount, out int betValue);
            TerminalAPI.RemoveGroupCredits(betValue);

            var slots = SlotsGenerator.GenerateSlots<SlotsEnum>(3);
            var winnings = 0;

            foreach (SlotsEnum val in Enum.GetValues(typeof(SlotsEnum)))
            {
                int appearanceCount = slots.Count(x => x == val);
                if (appearanceCount == 3) winnings = betValue * ((int)val + 2) * 50;
            }

            TerminalAPI.AddGroupCredits(winnings);

            var sb = new StringBuilder();

            sb.AppendLine($"You got {slots[0]} {slots[1]} {slots[2]}");
            sb.AppendLine("");
            sb.AppendLine($"You have won {winnings}");

            return sb.ToString();
        }
    }
}
