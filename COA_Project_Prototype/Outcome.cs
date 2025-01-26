using COA_ProjectPrototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COA_Project_Prototype
{
    class OutcomeArray : DynamicArray<Outcome>
    {
        public OutcomeArray() : base(new Outcome[10], 0) { }

        public void Sort(OutcomeSortType type)
        {
            Sort(0, ElementCount - 1, type);
            for (int i = 0; i < ElementCount; i++)
                Elements[i].Index = i;
        }
        private void Sort(int startIndex, int pivotIndex, OutcomeSortType type)
        {
            if (startIndex >= pivotIndex)
                return;

            int i = startIndex - 1;
            int j = startIndex;
            while (j <= pivotIndex)
            {
                if (Elements[j].CompareTo(Elements[pivotIndex], type) <= 0)
                {
                    i++;
                    (Elements[j], Elements[i]) = (Elements[i], Elements[j]);
                }
                j++;
            }

            Sort(startIndex, i - 1, type);
            Sort(i + 1, pivotIndex, type);
        }
    }

    class Outcome
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }

        public Outcome(string name, string description, int index)
        {
            Name = name;
            Description = description;
            Index = index;
        }
        public int CompareTo(Outcome other, OutcomeSortType type)
        {
            if (type == OutcomeSortType.Name)
                return String.Compare(Name, other.Name, comparisonType: StringComparison.OrdinalIgnoreCase);
            return 0;
        }
    }
    public enum OutcomeSortType
    {
        Name
    }
}
