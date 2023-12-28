using System;

namespace LCTerminalSlots.Utils
{
    public class BetterRandom
    {
        private static readonly Random _generator = new(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Generates a random number using a unique hash code as its seed
        /// </summary>
        /// <param name="maxNum">The upper limit of the random, is NOT included</param>
        public static int GetRandomSlot(int maxNum)
        {
            return _generator.Next(0, maxNum);
        }
    }
}
