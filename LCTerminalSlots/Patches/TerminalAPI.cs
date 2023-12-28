using System;
using HarmonyLib;
using LCTerminalSlots.Utils;

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
            ParamAssert.IsNotNull(_terminal);
            ParamAssert.IsGreaterThanOrEqualToZero(amount);

            SetTerminalCredits(_terminal!.groupCredits + amount);
        }

        /// <summary>
        /// Removes the requested amount of group credits, syncs with server, will always leave credits at zero
        /// </summary>
        /// <param name="amount">Amount of credits to remove as a positive integer</param>
        public static void RemoveGroupCredits(int amount)
        {
            ParamAssert.IsNotNull(_terminal);
            ParamAssert.IsGreaterThanOrEqualToZero(amount);

            var remainingAmount = _terminal!.groupCredits - amount;

            SetTerminalCredits(remainingAmount >= 0 ? remainingAmount : 0);
        }

        private static void SetTerminalCredits(int credits)
        {
            if (_terminal is null) return;

            _terminal.useCreditsCooldown = true;
            _terminal.groupCredits = credits;
            _terminal.SyncGroupCreditsServerRpc(_terminal.groupCredits, _terminal.numberOfItemsInDropship);
        }

        #region TestingHelpers

        public static void SetTerminal(Terminal? mockTerminal)
        {
            _terminal = mockTerminal;
        }

        #endregion
    }
}
