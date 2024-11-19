using System;
using System.Collections.Generic;
namespace COA_ProjectPrototype
{
    public class EventArray : DynamicArray<Event>
    {
        public EventArray() : base(new Event[10], 0) { }

        public void Sort()
        {
            Sort(0, ElementCount - 1);
        }

        private void Sort(int startIndex, int pivotIndex)
        {
            if (startIndex >= pivotIndex)
                return;

            int i = startIndex - 1;
            int j = startIndex;
            while (j <= pivotIndex)
            {
                if (Elements[j].CompareTo(Elements[pivotIndex]) <= 0)
                {
                    i++;
                    (Elements[j], Elements[i]) = (Elements[i], Elements[j]);
                }
                j++;
            }

            Sort(startIndex, i - 1);
            Sort(i + 1, pivotIndex);
        }
    }
    public class Event
	{
		public int[,] Costs { get; set; }
		public int Total { get; private set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        public Dictionary<string, DateTime> DaysOfOperation { get; set; }

		public Event(DateTime startDate, DateTime endDate)
		{
			Costs = new int[4, 6];
			Total = 0;
			StartDate = startDate;
			EndDate = endDate;
            DaysOfOperation = new Dictionary<string, DateTime>();
		}

		public double GetCost(int location, int costType) { return Costs[location, costType]; }

		public void SetCost(int location, int costType, int value)
		{
			Costs[location, costType] = value;
			UpdateTotal();
		}

		public void UpdateTotal()
		{
			foreach (int cost in Costs)
				Total += cost;
		}

		public int CompareTo(Event other)
		{
			int total = 0;
			total += (StartDate.DayOfYear + StartDate.Year * 365) - (other.StartDate.DayOfYear + other.StartDate.Year * 365);
            total += (EndDate.DayOfYear + EndDate.Year * 365) - (other.EndDate.DayOfYear + other.EndDate.Year * 365);
			return total/2;
        }
	}

    public enum Locations : ushort
    {
        EmergencyRoom = 0,
        OperatingRoom = 1,
        IntenciveCareDept = 2,
        GeneralWard = 3
    }

    public enum CostType : ushort
    {
        Labor = 0,
        Drugs = 1,
        Laboratory = 2,
        Equipment = 3,
        Radiology = 4,
        Other = 5
    }
}
