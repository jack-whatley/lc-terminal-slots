﻿using System.Text;
using LCTerminalSlots.Patches;
using LCTerminalSlots.Utils;
using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal;
using UnityEngine;
using System;

namespace LCTerminalSlots.Commands
{
    public class SlotsCommands
    {
        private readonly Terminal _instance;

        public SlotsCommands(Terminal instance)
        {
            _instance = instance;
        }

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
            if (betValue <= 0) return "Bets must be larger than zero...";

            return SlotsAlgorithm.GenerateSlotsCalculated(betValue);
        }

        [TerminalCommand("summon", true)]
        public string SummonOpps(string monsterName, string amount)
        {
            if (!int.TryParse(amount, out int monsterCount)) return "Input valid amount...";

            var (result, name) = EnemyHandler.AttemptSpawnEnemy(monsterName, monsterCount);

            if (result)
            {
                string output = $"Summoned {monsterCount}x of {name}";
                ChatAPI.SendServerMessage(output);
                return output;
            }

            return "Failed to summon the opps :(";
        }
    }
}
