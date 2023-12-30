using System.Text;
using LCTerminalSlots.Patches;
using LCTerminalSlots.Utils;
using LethalAPI.LibTerminal.Attributes;

namespace LCTerminalSlots.Commands
{
    public class SlotsCommands
    {
        [TerminalCommand("slotshelp", true)]
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

            if (TerminalAPI.GetCreditsCount() - betValue < 0) return "You can't afford that...";
            if (betValue == 0) return "Bets must be larger than zero...";

            return SlotsAlgorithm.GenerateSlotsTrueRandom(betValue);
        }
    }
}
