using LCTerminalSlots.Models;

namespace LCTerminalSlots.Tests
{
    [TestClass]
    public class MemoryStructureTests
    {
        [TestMethod]
        public void MemoryStructureCantStoreMoreThanCapacity()
        {
            var mStruct = new MemoryStructure<string>(3);

            for (int i = 0; i < 10; i++) mStruct.AddItem("test");

            Assert.AreEqual(3, mStruct.Count);
        }

        [TestMethod]
        public void MemoryStructureShiftsDataOnAdd()
        {
            var mStruct = new MemoryStructure<string>(3);
            var strings = new[] { "test1", "test2", "test3", "test4" };

            foreach (string s in strings) mStruct.AddItem(s);

            CollectionAssert.AreEqual(new[] { "test4", "test3", "test2" }, mStruct.GetItems());
        }
    }
}
