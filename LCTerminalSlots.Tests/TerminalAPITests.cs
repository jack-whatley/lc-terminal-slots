using System;
using LCTerminalSlots.Patches;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LCTerminalSlots.Tests
{
    [TestClass]
    public class TerminalAPITests
    {
        private readonly Mock<Terminal> _terminal = new();

        [TestCleanup]
        public void MockCleanup()
        {
            _terminal.Reset();
        }

        [TestMethod]
        public void AddGroupCreditsAddsCorrectAmount()
        {
            var terminal = _terminal.Object;
            terminal.groupCredits = 60;

            TerminalAPI.SetTerminal(terminal);
            TerminalAPI.AddGroupCredits(100);

            Assert.AreEqual(160, terminal.groupCredits);
        }

        [TestMethod]
        public void AddGroupCreditsRemovesCorrectAmount()
        {
            var terminal = _terminal.Object;
            terminal.groupCredits = 60;

            TerminalAPI.SetTerminal(terminal);
            TerminalAPI.AddGroupCredits(-40);

            Assert.AreEqual(20, terminal.groupCredits);
        }

        [TestMethod]
        public void RemoveGroupCreditsRemovesCorrectAmount()
        {
            var terminal = _terminal.Object;
            terminal.groupCredits = 60;

            TerminalAPI.SetTerminal(terminal);
            TerminalAPI.RemoveGroupCredits(40);

            Assert.AreEqual(20, terminal.groupCredits);
        }

        [TestMethod]
        public void RemoveGroupCreditsAvoidsNegative()
        {
            var terminal = _terminal.Object;
            terminal.groupCredits = 60;

            TerminalAPI.SetTerminal(terminal);
            TerminalAPI.RemoveGroupCredits(120);

            Assert.AreEqual(0, terminal.groupCredits);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveGroupCreditsNegativeArgumentException()
        {
            var terminal = _terminal.Object;
            terminal.groupCredits = 60;

            TerminalAPI.SetTerminal(terminal);
            TerminalAPI.RemoveGroupCredits(-100);

            Assert.Fail();
        }
    }
}
