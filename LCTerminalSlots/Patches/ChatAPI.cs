using HarmonyLib;

namespace LCTerminalSlots.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    public static class ChatAPI
    {
        private static HUDManager? _instance;

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Awake(ref HUDManager __instance)
        {
            _instance = __instance;
        }

        /// <summary>
        /// Send a text message in the chat to all members with no username
        /// </summary>
        /// <param name="messageText">The message text to display</param>
        public static void SendServerMessage(string messageText)
        {
            _instance?.AddTextToChatOnServer(messageText);
        }
    }
}
