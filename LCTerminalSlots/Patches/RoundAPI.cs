using HarmonyLib;

namespace LCTerminalSlots.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    public class RoundAPI
    {
        public static EnemyVent[]? AllVents = null;

        public static SelectableLevel? CurrentLevel = null;

        [HarmonyPatch("AdvanceHourAndSpawnNewBatchOfEnemies")]
        [HarmonyPrefix]
        private static void GetCurrentLevelInfo(ref EnemyVent[] ___allEnemyVents, ref SelectableLevel ___currentLevel)
        {
            AllVents = ___allEnemyVents;
            CurrentLevel = ___currentLevel;
        }
    }
}
