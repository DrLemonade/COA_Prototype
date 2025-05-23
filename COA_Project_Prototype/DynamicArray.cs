using COA_Project_Prototype;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace COA_ProjectPrototype
{
    public abstract class DynamicArray<T> : IEnumerable<T>
    {
        public T[] Elements { get; set; }
        public int ElementCount { get; private set; }

        public DynamicArray(T[] elements, int elementCount)
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

        public T GetElement(int index) { return Elements[index]; }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < ElementCount; i++)
            {
                result += " \n" + i + " " + Elements[i].ToString();
            }

            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for(int i = 0; i < ElementCount; i++)
                yield return Elements[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}