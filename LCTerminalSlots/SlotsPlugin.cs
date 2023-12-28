using BepInEx;
using HarmonyLib;
using LCTerminalSlots.Utils;
using System.IO;
using System.Reflection;
using LCTerminalSlots.Commands;
using LCTerminalSlots.Models;
using LethalAPI.LibTerminal.Models;

namespace LCTerminalSlots
{
    [BepInPlugin(SlotsPluginInfo.MOD_GUID, SlotsPluginInfo.MOD_NAME, SlotsPluginInfo.MOD_VER)]
    [BepInDependency("LethalAPI.Terminal", BepInDependency.DependencyFlags.HardDependency)]
    public class SlotsPlugin : BaseUnityPlugin
    {
        private readonly Harmony _harmony = new(SlotsPluginInfo.MOD_GUID);

        private readonly TerminalModRegistry _registry = TerminalRegistry.CreateTerminalRegistry();

        public string ModFilesPath = "";

        void Awake()
        {
            /* handling files */
            ModFilesPath = $"{Directory.GetCurrentDirectory()}\\BepInEx\\config\\Slots";
            if (!Directory.Exists(ModFilesPath)) Directory.CreateDirectory(ModFilesPath);

            /* registering commands */
            _registry.RegisterFrom<SlotsCommands>();

            CommandHandler.SetInterface(new CustomTerminalInterface());

            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            DontDestroyOnLoad(this);
        }
    }
}
