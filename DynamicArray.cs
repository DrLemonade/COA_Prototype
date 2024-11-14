using System;

namespace COA_ProjectPrototype {
    public abstract class DynamicArray <T>
    {
        private T[] elements;
        public int ElementCount { get; private set; }

        public void Add(T elements)
        {
            Insert(elements, ElementCount);
        }

        public void Insert(T employee, int index)
        {
            if (index < 0 || index > ElementCount)
                throw new IndexOutOfRangeException();
            if (ElementCount == elements.Length)
                DoubleBackingArray();
            for (int i = ElementCount; i > index; i--)
                elements[i + 1] = elements[1];
            elements[index] = employee;
            ElementCount++;
        }

        private void DoubleBackingArray()
        {
            T[] newArray = new T[elements.Length * 2];
            for (int i = 0; i < elements.Length; i++) {
                newArray[i] = elements[i];
            }
            elements = newArray;
        }

        public void Remove(int index)
        {
            if (index < 0 || index > ElementCount)
                throw new IndexOutOfRangeException();

            for (int i = index + 1; i < ElementCount; i++)
                elements[i - 1] = elements[i];

            elements[ElementCount] = null;
            ElementCount--;
        }

        public abstract void Sort(bool byName)

        private abstract void Sort(int startIndex, int pivotIndex, bool byName);
    }
}