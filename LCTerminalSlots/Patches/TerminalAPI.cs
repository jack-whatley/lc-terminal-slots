using System;
using HarmonyLib;

namespace LCTerminalSlots.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    public static class TerminalAPI
    {
        private static Terminal? _terminal;

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Awake(ref Terminal __instance)
        {
            _terminal = __instance;
        }

        public static int GetCreditsCount()
        {
            return _terminal?.groupCredits ?? 0;
        }

        /// <summary>
        /// Adds the requested amount of group credits, syncs with server
        /// </summary>
        /// <param name="amount">Amount of group credits to add</param>
        public static void AddGroupCredits(int amount)
        {
            if (_terminal is null) return;

            _terminal.useCreditsCooldown = true;
            _terminal.groupCredits += amount;
            _terminal.SyncGroupCreditsServerRpc(_terminal.groupCredits, _terminal.numberOfItemsInDropship);
        }

        /// <summary>
        /// Removes the requested amount of group credits, syncs with server, will always leave credits at zero
        /// </summary>
        /// <param name="amount">Amount of credits to remove as a positive integer</param>
        public static void RemoveGroupCredits(int amount)
        {
            if (_terminal is null) return;
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount of was outside of valid range, can't be below zero");

            var remainingAmount = _terminal.groupCredits - amount;

            if (remainingAmount >= 0)
            {
                AddGroupCredits(-amount);
            } 
            else
            {
                AddGroupCredits(-_terminal.groupCredits);
            }
        }

        #region TestingHelpers

        public static void SetTerminal(Terminal mockTerminal)
        {
            _terminal = mockTerminal;
        }

        #endregion
    }
}
