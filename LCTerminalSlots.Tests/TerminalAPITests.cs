using System;
using LCTerminalSlots.Patches;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LCTerminalSlots.Tests
{
    [TestClass]
    public class TerminalAPITests // TODO: Add int max and min value tests (for both add and remove).
    {
        private readonly Mock<Terminal> _terminal = new();

        [TestCleanup]
        public void MockCleanup()
        {
            _terminal.Reset();
        }

        [DataTestMethod]
        [DataRow(60, 60, 120, DisplayName = "60 + 60 = 120 Credits")]
        [DataRow(100, 199, 299, DisplayName = "100 + 199 = 299 Credits")]
        public void AddGroupCreditsAddsCorrectAmount(int startingAmount, int amountToAdd, int expectedAmount)
        {
            var terminal = _terminal.Object;
            terminal.groupCredits = startingAmount;

            TerminalAPI.SetTerminal(terminal);
            TerminalAPI.AddGroupCredits(amountToAdd);

            Assert.AreEqual(expectedAmount, terminal.groupCredits);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddGroupCreditsPreventsNegativeInputAmount()
        {
            var terminal = _terminal.Object;
            terminal.groupCredits = 60;

            TerminalAPI.SetTerminal(terminal);
            TerminalAPI.AddGroupCredits(-40);

            Assert.Fail();
        }

        [DataTestMethod]
        [DataRow(60, 40, 20, DisplayName = "60 - 40 = 20 Credits")]
        [DataRow(100, 99, 1, DisplayName = "100 - 99 = 1 Credit")]
        public void RemoveGroupCreditsRemovesCorrectAmount(int startingAmount, int amountToRemove, int expectedAmount)
        {
            var terminal = _terminal.Object;
            terminal.groupCredits = startingAmount;

            TerminalAPI.SetTerminal(terminal);
            TerminalAPI.RemoveGroupCredits(amountToRemove);

            Assert.AreEqual(expectedAmount, terminal.groupCredits);
        }

        [TestMethod]
        public void RemoveGroupCreditsPreventsNegativeGroupCredits()
        {
            var terminal = _terminal.Object;
            terminal.groupCredits = 60;

            TerminalAPI.SetTerminal(terminal);
            TerminalAPI.RemoveGroupCredits(120);

            Assert.AreEqual(0, terminal.groupCredits);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveGroupCreditsPreventsNegativeInputAmount()
        {
            var terminal = _terminal.Object;
            terminal.groupCredits = 60;

            TerminalAPI.SetTerminal(terminal);
            TerminalAPI.RemoveGroupCredits(-100);

            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNullExceptionIfTerminalNull()
        {
            TerminalAPI.SetTerminal(null);
            TerminalAPI.AddGroupCredits(100);

            Assert.Fail();
        }
    }
}
