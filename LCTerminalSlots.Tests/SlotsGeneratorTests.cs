using LCTerminalSlots.Models;
using LCTerminalSlots.Utils;

namespace LCTerminalSlots.Tests
{
    [TestClass]
    public class SlotsGeneratorTests
    {
        [DataTestMethod]
        [DataRow(new SlotsEnum[] { SlotsEnum.Bell, SlotsEnum.Bell, SlotsEnum.Bell }, DisplayName = "All bells row")]
        [DataRow(new SlotsEnum[] { SlotsEnum.Clover, SlotsEnum.Clover, SlotsEnum.Clover }, DisplayName = "All bells row")]
        public void CheckSlotsEqualIdentifiesWinningSlots(SlotsEnum[] inputSlots)
        {
            Assert.IsTrue(SlotsGenerator.CheckSlotsEqual(inputSlots));
        }

        [DataTestMethod]
        [DataRow(new SlotsEnum[] { SlotsEnum.Cherry, SlotsEnum.Cherry, SlotsEnum.Clover }, DisplayName = "Cherry Cherry Clover Case")]
        [DataRow(new SlotsEnum[] { SlotsEnum.Bell, SlotsEnum.Bell, SlotsEnum.Dollar }, DisplayName = "Bell Bell Dollar Case")]
        [DataRow(new SlotsEnum[] { SlotsEnum.Bell, SlotsEnum.Diamond, SlotsEnum.Bell }, DisplayName = "Bell Diamond Bell Case")]
        public void CheckSlotsEqualIdentifiesLosingSlots(SlotsEnum[] inputSlots)
        {
            Assert.IsFalse(SlotsGenerator.CheckSlotsEqual(inputSlots));
        }
    }
}
