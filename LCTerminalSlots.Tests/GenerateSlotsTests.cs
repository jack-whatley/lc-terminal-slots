using LCTerminalSlots.Utils;

namespace LCTerminalSlots.Tests
{
    [TestClass]
    public class GenerateSlotsTests
    {
        [TestMethod]
        public void GenerateSlotsReturnsCorrectNumber()
        {
            const int slotsLen = 3;

            var slots = SlotsGenerator.GenerateSlots(slotsLen);

            Assert.AreEqual(slotsLen, slots.Count);
        }
    }
}
