using System.Collections.Generic;

namespace LCTerminalSlots.Models
{
    public class MemoryStructure<T>
    {
        private readonly List<T> _container = new();

        public int Capacity;

        public int Count { get => _container.Count; }

        public MemoryStructure(int maxCapacity)
        {
            Capacity = maxCapacity;
        }

        public void AddItem(T item)
        {
            _container.Insert(0, item);

            if (_container.Count > Capacity)
            {
                _container.RemoveAt(_container.Count - 1);
            }
        }

        public T[] GetItems()
        {
            return _container.ToArray();
        }
    }
}
