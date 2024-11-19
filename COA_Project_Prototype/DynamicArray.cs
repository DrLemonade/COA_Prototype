using System;
using System.Runtime.InteropServices;

namespace COA_ProjectPrototype 
{
    public abstract class DynamicArray<T>
    {
        public T[] Elements { get; set; }
        public int ElementCount { get; private set; }

        public DynamicArray (T[] elements, int elementCount)
        {
            this.Elements = elements;
            this.ElementCount = elementCount;
        }

        public void Add(T element)
        {
            Insert(element, ElementCount);
        }

        public void Insert(T element, int index)
        {
            if (index < 0 || index > ElementCount)
                throw new IndexOutOfRangeException();
            if (ElementCount == Elements.Length)
                DoubleBackingArray();
            for (int i = ElementCount; i > index; i--)
                Elements[i + 1] = Elements[1];
            Elements[index] = element;
            ElementCount++;
        }

        private void DoubleBackingArray()
        {
            T[] newArray = new T[Elements.Length * 2];
            for (int i = 0; i < Elements.Length; i++) {
                newArray[i] = Elements[i];
            }
            Elements = newArray;
        }

        public void Remove(int index)
        {
            if (index < 0 || index > ElementCount)
                throw new IndexOutOfRangeException();

            for (int i = index + 1; i < ElementCount; i++)
                Elements[i - 1] = Elements[i];

            Elements[ElementCount] = default;
            ElementCount--;
        }

        public T GetElement(int index) {  return Elements[index]; }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < ElementCount; i++)
            {
                result += " \n" + i + " " + Elements[i].ToString();
            }

            return result;
        }
    }
}