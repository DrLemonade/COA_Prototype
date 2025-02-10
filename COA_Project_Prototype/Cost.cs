using COA_ProjectPrototype;
using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace COA_Project_Prototype
{
    public class CostArray : DynamicArray<Cost>
    {
        private string CaseID { get; set; }
        public CostArray(string caseID) : base(new Cost[10], 0)
        {
            CaseID = caseID;
        }

        public void Sort(CostSortType type)
        {
            Sort(0, ElementCount - 1, type);
            for (int i = 0; i < ElementCount; i++)
                Elements[i].Index = i;
        }

        private void Sort(int startIndex, int pivotIndex, CostSortType type)
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
         * CostSortType must be name, cost type, or cost catagory
         */
        public Cost Find(string value, CostSortType type)
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
                    if (type == CostSortType.Name)
                        compareValue = Elements[mid].Name;
                    if (type == CostSortType.CostType)
                        compareValue = Elements[mid].CostType;
                    if (type == CostSortType.CostCategory)
                        compareValue = Elements[mid].CostCategory;

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
         * CostSortType must be cost amount
         */
        public Cost Find(int value, CostSortType type)
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
                    if (type == CostSortType.CostAmount)
                        compareValue = Elements[mid].CostAmount;

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
         * CostSortType must be issue date
         */
        public Cost Find(DateTime value, CostSortType type)
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
                    if (type == CostSortType.IssueDate)
                        compareValue = Elements[mid].IssueDate;

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
            using (var reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/_cost__202412091112.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                for (int i = 0; csv.Read(); i++)
                {
                    if (csv.GetField("case_id") == CaseID)
                    {
                        string name = csv.GetField("cost_name");
                        string costType = csv.GetField("cost_type");
                        string costCategory = csv.GetField("cost_category");
                        int quantity = 0;
                        if (Int32.TryParse(csv.GetField("quantity"), out int result1))
                            quantity = result1;
                        int costAmount = 0;
                        if (Double.TryParse(csv.GetField("duration"), out double result2))
                            costAmount = (int)result2;
                        int duration = 0;
                        if (Int32.TryParse(csv.GetField("duration"), out int result3))
                            duration = result3;

                        DateTime issueDate;
                        issueDate = new DateTime(Int32.Parse(csv.GetField("issue_date").Substring(0, 4)), Int32.Parse(csv.GetField("issue_date").Substring(5, 2)), Int32.Parse(csv.GetField("issue_date").Substring(8, 2)), 0, 0, 0);

                        Cost record = new Cost(name, costType, costCategory, quantity, costAmount, duration, issueDate, i);
                        Add(record);
                    }
                }
            }
        }

        public void AppendCSV(Patient patient)
        {
            List<object> records = new List<object> { new { patient_id = patient.PatientID, hospital_id = "H1", patient_first_name = patient.FirstName, patient_last_name = patient.LastName, date_of_birth = patient.DOB.ToString("yyyy-MM-dd"), gender = patient.Gender } };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, };

            using (var stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/_cost__202412091112.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.NextRecord();
                csv.WriteRecords(records);
            }
        }
    }
    public class Cost
    {
        public string Name { get; set; }
        public string CostType { get; set; }
        public string CostCategory { get; set; }
        public int Index { get; set; }
        public int Quantity { get; set; }
        public int CostAmount { get; set; }
        public int DurationInHours { get; set; }
        public DateTime IssueDate { get; set; }

        public Cost(string name, string costType, string costCategory, int quantity, int costAmount, int durationInHours, DateTime issueDate, int index)
        {
            Name = name;
            CostType = costType;
            CostCategory = costCategory;
            Quantity = quantity;
            CostAmount = costAmount;
            DurationInHours = durationInHours;
            IssueDate = issueDate;
            Index = index;
        }

        public int CompareTo(Cost other, CostSortType type)
        {
            if (type == CostSortType.Name)
                return String.Compare(Name, other.Name, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == CostSortType.CostType)
                return String.Compare(CostType, other.CostType, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == CostSortType.CostCategory)
                return String.Compare(CostCategory, other.CostCategory, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == CostSortType.CostAmount && CostAmount > other.CostAmount)
                return CostAmount.CompareTo(other.CostAmount);
            if (type == CostSortType.IssueDate)
                return (IssueDate.DayOfYear + IssueDate.Year * 365) - (other.IssueDate.DayOfYear + other.IssueDate.Year * 365);
            return 0;
        }

        public override string ToString()
        {
            return Name + ": " + CostType + ", " + CostCategory + ", ¥" + CostAmount + ", " + IssueDate.ToString();
        }
    }

    public enum CostSortType
    {
        Name,
        CostType,
        CostCategory,
        CostAmount,
        IssueDate
    }
}
