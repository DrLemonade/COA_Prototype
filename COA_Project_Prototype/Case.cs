using COA_Project_Prototype;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace COA_ProjectPrototype
{
    public class CaseArray : DynamicArray<Case>
    {
        public CaseArray() : base(new Case[10], 0) { }

        public void Sort(CaseSortType type)
        {
            Sort(0, ElementCount - 1, type);
        }

        private void Sort(int startIndex, int pivotIndex, CaseSortType type)
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
    public class Case
	{
		public int Total { get; private set; }
        public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        public CostSortType SortType { get; private set; }
        public Dictionary<string, CostArray> Costs { get; private set; }
        public Dictionary<string, DateTime> TimesOfOperationStart { get; private set; }
        public Dictionary<string, DateTime> TimesOfOperationEnd { get; private set; }

        public Case(string name, DateTime startDate, DateTime endDate)
		{
            Name = name;
			Total = 0;
			StartDate = startDate;
			EndDate = endDate;
            Costs = new Dictionary<string, CostArray>();
            TimesOfOperationStart = new Dictionary<string, DateTime>();
            TimesOfOperationEnd = new Dictionary<string, DateTime>();
        }

		public Cost GetCost(string location, string value) 
        {
            CostArray costArray;
            if(Costs.TryGetValue(location, out costArray))
                return costArray.Find(value, SortType);

            return default;
        }

		public void AddCost(string location, Cost cost)
		{
            CostArray costArray;
            if (Costs.TryGetValue(location, out costArray))
            {
                costArray.Add(cost);
                costArray.Sort(SortType);
                Costs.Remove(location);
                Costs.Add(location, costArray);
            }
            else
            {
                costArray = new CostArray();
                costArray.Add(cost);
                costArray.Sort(SortType);
                Costs.Add(location, costArray);
            }
            UpdateTotal();
		}

        public void RemoveCost(string location, int index)
        {
            CostArray costArray;
            if (Costs.TryGetValue(location, out costArray))
                costArray.Remove(index);
            UpdateTotal(); 
        }

		public void UpdateTotal()
		{
            int total = 0;
            foreach (KeyValuePair<string, CostArray> costArray in Costs)
                for(int i = 0; i < costArray.Value.ElementCount; i++)
                    total += costArray.Value.GetElement(i).CostAmount;

            Total = total;
        }

        public void UpdateSortType(CostSortType type)
        {
            SortType = type;
            foreach (KeyValuePair<string, CostArray> costArray in Costs)
                costArray.Value.Sort(SortType);
        }

        // TODO: Add Operation Start/End Time

        public int CompareTo(Case other, CaseSortType type)
		{
            if(type == CaseSortType.Name)
                return String.Compare(Name, other.Name, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == CaseSortType.StartDate)
                return (StartDate.DayOfYear + StartDate.Year * 365) - (other.StartDate.DayOfYear + other.StartDate.Year * 365);
            if (type == CaseSortType.EndDate)
                return (EndDate.DayOfYear + EndDate.Year * 365) - (other.EndDate.DayOfYear + other.EndDate.Year * 365);
            if (type == CaseSortType.AvgDate)
            {
                int total = 0;
                total += (StartDate.DayOfYear + StartDate.Year * 365) - (other.StartDate.DayOfYear + other.StartDate.Year * 365);
                total += (EndDate.DayOfYear + EndDate.Year * 365) - (other.EndDate.DayOfYear + other.EndDate.Year * 365);
                return total / 2;
            }
            if (type == CaseSortType.Total)
                return Total - other.Total;
            return 0;
        }
	}

    public enum CaseSortType
    {
        Name,
        StartDate,
        EndDate,
        AvgDate,
        Total
    }
}
