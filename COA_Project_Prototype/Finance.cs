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
    public class FinanceArray : DynamicArray<Finance>
    {
        private string TreatmentID { get; set; }
        public FinanceArray(string treatmentID) : base(new Finance[10], 0)
        {
            TreatmentID = treatmentID;
        }

        public void Sort(FinanceSortType type)
        {
            Sort(0, ElementCount - 1, type);
            for (int i = 0; i < ElementCount; i++)
                Elements[i].Index = i;
        }

        private void Sort(int startIndex, int pivotIndex, FinanceSortType type)
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
         * CostSortType must be Finance type or id
         */
        public Finance Find(string value, FinanceSortType type)
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
                    if (type == FinanceSortType.FinanceName)
                        compareValue = Elements[mid].FinanceName;
                    if(type == FinanceSortType.FinanceType)
                        compareValue = Elements[mid].FinanceType;
                    if(type == FinanceSortType.FinanceCategory)
                        compareValue = Elements[mid].FinanceCategory;
                    if(type == FinanceSortType.FinanceLabel)
                        compareValue = Elements[mid].FinanceLabel;
                    if (type == FinanceSortType.FinanceID)
                        compareValue = Elements[mid].FinanceID;

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
         * CostSortType must be Cost or Quantity
         */
        public Finance Find(int value, FinanceSortType type)
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
                    if (type == FinanceSortType.Cost)
                        compareValue = Elements[mid].Cost;
                    if (type == FinanceSortType.Quantity)
                        compareValue = Elements[mid].Quantity;

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

        public void ReadCSV()
        {
            using (var reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/finance_coa.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                for (int i = 0; csv.Read(); i++)
                {
                    if (csv.GetField("treatment_id") == TreatmentID)
                    {
                        string financeID = csv.GetField("finance_id");
                        string financeName = csv.GetField("cost_name");
                        string financeType = csv.GetField("cost_type");
                        string financeCategory = csv.GetField("cost_category");
                        string financeLabel = csv.GetField("cost_label");
                        int quantity = 0;
                        if (Double.TryParse(csv.GetField("quantity"), out double result1))
                            quantity = (int)result1;
                        int cost = 0;
                        if (Double.TryParse(csv.GetField("cost"), out double result2))
                            cost = (int)result2;
                        string sourceID = "";
                        if (financeType.Equals("Professional Cost"))
                            sourceID = csv.GetField("staff_id");
                        if (financeType.Equals("Facility Direct Cost"))
                            sourceID = csv.GetField("organization_id");

                        Finance record = new Finance(financeName, financeType, financeCategory, financeLabel, quantity, cost, sourceID, financeID, i);
                        Add(record);
                    }
                }
            }
        }

        public void AppendCSV(Finance Finance)
        {
            string staffID = "";
            string organizationID = "";
            if (Finance.FinanceType.Equals("Professional Cost"))
            {
                staffID = Finance.SourceID;
                organizationID = "";
            }
            else
            {
                staffID = "";
                organizationID = Finance.SourceID;
            }
                List<object> records = new List<object> { new { finance_id = Finance.FinanceID, treatment_id = TreatmentID, organization_id = organizationID, staff_id = staffID, cost_name = Finance.FinanceName, cost_type = Finance.FinanceType, cost_category = Finance.FinanceCategory, cost_label = Finance.FinanceLabel, quantity = Finance.Quantity, cost = Finance.Cost} };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, };

            using (var stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/_Finance__202502131233.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.NextRecord();
                csv.WriteRecords(records);
            }
        }
    }
    public class Finance
    {
        public string FinanceName { get; set; }
        public string FinanceType { get; set; } // oragnization or physician
        public string FinanceCategory { get; set; }
        public string FinanceLabel { get; set; }
        public int Quantity { get; set; }
        public int Cost { get; set; }
        public string FinanceID { get; set; }
        public string SourceID { get; set; }
        public int Index { get; set; }

        public Finance(string financeName, string financeType, string financeCategory, string financeLabel, int quantity, int cost, string financeID, string sourceID, int index)
        {
            FinanceName = financeName;
            FinanceType = financeType;
            FinanceCategory = financeCategory;
            FinanceLabel = financeLabel;
            FinanceID = financeID;
            SourceID = sourceID;
            Quantity = quantity;
            Cost = cost;
            Index = index;
        }

        public int CompareTo(Finance other, FinanceSortType type)
        {
            if (type == FinanceSortType.FinanceID)
                return String.Compare(FinanceID, other.FinanceID, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == FinanceSortType.FinanceName)
                return String.Compare(FinanceName, other.FinanceName, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == FinanceSortType.FinanceType)
                return String.Compare(FinanceType, other.FinanceType, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == FinanceSortType.FinanceCategory)
                return String.Compare(FinanceCategory, other.FinanceCategory, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == FinanceSortType.FinanceLabel)
                return String.Compare(FinanceLabel, other.FinanceLabel, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == FinanceSortType.Quantity && Quantity > other.Quantity)
                return Quantity.CompareTo(other.Quantity);
            if (type == FinanceSortType.Cost && Cost > other.Cost)
                return Cost.CompareTo(other.Cost);
            return 0;
        }

        public override string ToString()
        {
            return FinanceID + ": " + FinanceName + ", " + FinanceType + ": " + SourceID + ", " + FinanceCategory + ", " + FinanceLabel + ", quantity: " + Quantity + ", �" + Cost;
        }
    }

    public enum FinanceSortType
    {
        FinanceID,
        FinanceName,
        FinanceType,
        FinanceCategory,
        FinanceLabel,
        Quantity,
        Cost
    }
}
