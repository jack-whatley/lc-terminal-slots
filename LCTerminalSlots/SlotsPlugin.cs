using BepInEx;
using HarmonyLib;
using LCTerminalSlots.Utils;
using System.IO;
using System.Reflection;
using LCTerminalSlots.Commands;
using LCTerminalSlots.Models;
using LCTerminalSlots.Patches;
using LethalAPI.LibTerminal.Models;

namespace LCTerminalSlots
{
    [BepInPlugin(SlotsPluginInfo.MOD_GUID, SlotsPluginInfo.MOD_NAME, SlotsPluginInfo.MOD_VER)]
    [BepInDependency("LethalAPI.Terminal", BepInDependency.DependencyFlags.HardDependency)]
    public class SlotsPlugin : BaseUnityPlugin
    {
        private readonly Harmony _harmony = new(SlotsPluginInfo.MOD_GUID);

        private readonly TerminalModRegistry _registry = TerminalRegistry.CreateTerminalRegistry();

        public static string ModFilesPath = "";

        void Awake()
        {
            /* handling files */
            ModFilesPath = $"{Directory.GetCurrentDirectory()}\\BepInEx\\config\\Slots";
            if (!Directory.Exists(ModFilesPath)) Directory.CreateDirectory(ModFilesPath);

            /* registering commands */
            TerminalAPI.OnTerminalInitialised += Handle_TerminalAwake;

            CommandHandler.SetInterface(new CustomTerminalInterface());

            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            DontDestroyOnLoad(this);
        }

        private void Handle_TerminalAwake(Terminal instance)
        {
            var slotsCommands = new SlotsCommands(instance);
            _registry.RegisterFrom(slotsCommands);
        }
    }
}
