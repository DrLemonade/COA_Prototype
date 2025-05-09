using COA_Project_Prototype;
using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
namespace COA_ProjectPrototype
{
    public class CaseArray : DynamicArray<Case>
    {
        public string PatientID { get; set; }
        public CaseArray(string patientID) : base(new Case[10], 0) 
        {
            PatientID = patientID;
        }

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

        /**
         * Binary search based on a string value.
         * CaseSortType must be name
         */
        public Case Find(string value, CaseSortType type)
        {
            Sort(type);
            int min = 0;
            int max = Elements.Length - 1;
            while (max >= min)
            {
                int mid = (min + max) / 2;
                if (Elements[mid] != null)
                {
                    string compareValue = "";
                    if (type == CaseSortType.Name)
                        compareValue = Elements[mid].Name;

                    if (String.Compare(value, compareValue, comparisonType: StringComparison.OrdinalIgnoreCase) < 0)
                        max = mid - 1;
                    else if (String.Compare(value, compareValue, comparisonType: StringComparison.OrdinalIgnoreCase) > 0)
                        min = mid + 1;
                    else if (compareValue.Equals(value))
                        return Elements[mid];
                }
            }
            return null;
        }

        /**
         * Binary search based on a int value.
         * CaseSortType must be total
         */
        public Case Find(int value, CaseSortType type)
        {
            Sort(type);
            int min = 0;
            int max = Elements.Length - 1;
            while (max >= min)
            {
                int mid = (min + max) / 2;
                if (Elements[mid] != null)
                {
                    int compareValue = 0;
                    if (type == CaseSortType.Total)
                        compareValue = Elements[mid].Total;

                    if (value < compareValue)
                        max = mid - 1;
                    else if (value > compareValue)
                        min = mid + 1;
                    else if (value == compareValue)
                        return Elements[mid];
                }
            }
            return null;
        }

        /**
         * Binary search based on a DateTime value.
         * CaseSortType must be start date, end date, or average date
         */
        public Case Find(DateTime value, CaseSortType type)
        {
            Sort(type);
            int min = 0;
            int max = Elements.Length - 1;
            while (max >= min)
            {
                int mid = (min + max) / 2;
                if (Elements[mid] != null)
                {
                    DateTime compareValue = default;
                    if (type == CaseSortType.StartDate)
                        compareValue = Elements[mid].StartDate;
                    if (type == CaseSortType.EndDate)
                        compareValue = Elements[mid].EndDate;
                    if (type == CaseSortType.AvgDate)
                        compareValue = Elements[mid].GetAverageDate();

                    if (value.CompareTo(compareValue) < 0)
                        max = mid - 1;
                    else if (value.CompareTo(compareValue) > 0)
                        min = mid + 1;
                    else if (value.CompareTo(compareValue) == 0)
                        return Elements[mid];
                }
            }
            return null;
        }

        public void ReadCSV()
        {
            using (var reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/cases_coa.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                for (int i = 0; csv.Read(); i++)
                {
                    if(csv.GetField("patient_id") == PatientID)
                    {
                        string caseID = csv.GetField("case_id");
                        string name = csv.GetField("case_name");
                        DateTime startDate = new DateTime(Int32.Parse(csv.GetField("start_date").Substring(0, 4)), Int32.Parse(csv.GetField("start_date").Substring(5, 2)), Int32.Parse(csv.GetField("start_date").Substring(8, 2)), 0, 0, 0);
                        DateTime endDate = new DateTime(Int32.Parse(csv.GetField("end_date").Substring(0, 4)), Int32.Parse(csv.GetField("end_date").Substring(5, 2)), Int32.Parse(csv.GetField("end_date").Substring(8, 2)), 0, 0, 0);
                        int outcome = 0;
                        if (Double.TryParse(csv.GetField("outcome"), out double result2))
                            outcome = (int)result2;
                        int satisfaction = 0;
                        if (Double.TryParse(csv.GetField("satisfaction_rate"), out double result3))
                            satisfaction = (int)result3;
                        bool readmissions = false;
                        if (Boolean.TryParse(csv.GetField("readmissions"), out bool result4))
                            readmissions = result4;

                        Case record = new Case(caseID, name, startDate, endDate, outcome, satisfaction, readmissions, i);
                        Add(record);
                    }
                }
            }
        }

        public void AppendCSV(Case @case)
        {
            List<object> records = new List<object> { new { case_id = @case.CaseID, hospital_id = "H1", patient_id = PatientID, case_name = @case.Name, start_date = @case.StartDate.ToString("yyyy-MM-dd"), end_date = @case.EndDate.ToString("yyyy-MM-dd"), mortality_flag = false, readmissions = @case.Readmissions, satisfaction_rate = @case.Satisfaction, outcome = @case.Outcome, total_cost = @case.Total } };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, };

            using (var stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/cases_coa.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.NextRecord();
                csv.WriteRecords(records);
            }
        }
    }
    public class Case
	{
        public string CaseID { get; set; }
		public int Total { get; private set; }
        public int Outcome { get; private set; }
        public int Satisfaction { get; private set; }
        public bool Readmissions { get; set; }
        public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        public TreatmentSortType SortType { get; private set; }
        public Dictionary<string, TreatmentArray> Treatments { get; private set; }
        public Dictionary<string, DateTime> TimesOfOperationStart { get; private set; }
        public Dictionary<string, DateTime> TimesOfOperationEnd { get; private set; }
        public int Index {  get; set; }

        public Case(string caseID, string name, DateTime startDate, DateTime endDate, int outcome, int satisfaction, bool readmissions, int index)
        {
            CaseID = caseID;
            Name = name;
            Total = 0;
            Readmissions = readmissions;
            Outcome = outcome; 
            Satisfaction = satisfaction;
            StartDate = startDate;
            EndDate = endDate;
            Treatments = new Dictionary<string, TreatmentArray>();
            TimesOfOperationStart = new Dictionary<string, DateTime>();
            TimesOfOperationEnd = new Dictionary<string, DateTime>();
            Index = index;

            TreatmentArray tempArr = new TreatmentArray(caseID);
            tempArr.ReadCSV();

            for (int i = 0; i < tempArr.ElementCount; i++)
            {
                if (Treatments.TryGetValue(tempArr.GetElement(i).TreatmentType, out TreatmentArray arr))
                    arr.Add(tempArr.GetElement(i));
                else
                {
                    arr = new TreatmentArray(CaseID);
                    Treatments.Add(tempArr.GetElement(i).TreatmentType, arr);
                    arr.Add(tempArr.GetElement(i));
                }
            }
            UpdateTotal();
        }

        public Treatment GetCost(string location, string value) 
        {
            TreatmentArray costArray;
            if(Treatments.TryGetValue(location, out costArray))
                return costArray.Find(value, SortType);

            return default;
        }

		public void AddCost(string location, Treatment cost)
		{
            TreatmentArray costArray;
            if (Treatments.TryGetValue(location, out costArray))
            {
                costArray.Add(cost);
                costArray.Sort(SortType);
                Treatments.Remove(location);
                Treatments.Add(location, costArray);
            }
            else
            {
                costArray = new TreatmentArray(CaseID);
                costArray.Add(cost);
                costArray.Sort(SortType);
                Treatments.Add(location, costArray);
            }
            UpdateTotal();
		}

        public void RemoveCost(string location, int index)
        {
            TreatmentArray treatmentArray;
            if (Treatments.TryGetValue(location, out treatmentArray))
                treatmentArray.Remove(index);
            UpdateTotal(); 
        }

		public void UpdateTotal()
		{
            int total = 0;
            foreach (KeyValuePair<string, TreatmentArray> costArray in Treatments)
                for(int i = 0; i < costArray.Value.ElementCount; i++)
                    total += costArray.Value.GetElement(i).Amount;

            Total = total;
        }

        public void UpdateSortType(TreatmentSortType type)
        {
            SortType = type;
            foreach (KeyValuePair<string, TreatmentArray> costArray in Treatments)
                costArray.Value.Sort(SortType);
        }

        public DateTime GetAverageDate()
        {
            int day = (StartDate.DayOfYear + StartDate.Year * 365 + EndDate.DayOfYear + EndDate.Year * 365) / 2;
            int year = day / 365;
            return new DateTime(year, 1, 1).AddDays((day % 365) - 1);
        }

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
                return (GetAverageDate().DayOfYear + GetAverageDate().Year * 365) - (other.GetAverageDate().DayOfYear + other.GetAverageDate().Year * 365);
            }
            if (type == CaseSortType.Total)
                return Total - other.Total;
            return 0;
        }

        public override string ToString()
        {
            string str = Name + ": " + StartDate.ToString() + " - " + EndDate.ToString() + ", ¥" + Total + ", Readmission: " + Readmissions + ", Outcome: " + Outcome + ", Satisfaction: " + Satisfaction + "\n";
            foreach (KeyValuePair<string, TreatmentArray> treatmentArray in Treatments)
                str += treatmentArray.Value.ToString() + "\n";
            return str;
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
