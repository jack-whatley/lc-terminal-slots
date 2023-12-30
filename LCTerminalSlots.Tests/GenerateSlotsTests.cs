using LCTerminalSlots.Utils;

namespace LCTerminalSlots.Tests
{
    [TestClass]
    public class GenerateSlotsTests
    {
        [TestMethod]
        public void GenerateSlotsReturnsCorrectNumber()
        {
            var slotsGen = new SlotsGenerator();
            const int slotsLen = 3;

            var slots = slotsGen.GenerateSlots(slotsLen);

            Assert.AreEqual(slotsLen, slots.Count);
        }
    }
}
