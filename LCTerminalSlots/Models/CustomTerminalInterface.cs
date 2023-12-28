using System.Linq;
using LethalAPI.LibTerminal.Interfaces;
using LethalAPI.LibTerminal.Models;

namespace LCTerminalSlots.Models
{
    public class CustomTerminalInterface : ITerminalInterface
    {
        public TerminalNode HandleInput(Terminal instance, ArgumentStream arguments)
        {
            var interactionResult = CommandHandler.ExecuteInteractions(arguments, instance);

            if (interactionResult is not null) return interactionResult;

            return CommandHandler.ExecuteCommand(arguments.Arguments[0], 
                new ArgumentStream(arguments.Arguments.Skip(1)), instance);
        }

        public string PreProcessText(Terminal instance, string text)
        {
            return text;
        }

        public string PostProcessText(Terminal terminal, string text)
        {
            return text;
        }

        public TerminalNode GetSplashScreen(Terminal terminal)
        {
            return null; // can do custom screen here
        }

        public bool APITextPostProcessing { get; } = false;
        public bool VanillaTextPostProcessing { get; } = true;
    }
}
