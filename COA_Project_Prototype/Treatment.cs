﻿using COA_ProjectPrototype;
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
    public class TreatmentArray : DynamicArray<Treatment>
    {
        private string CaseID { get; set; }
        public TreatmentArray(string caseID) : base(new Treatment[10], 0)
        {
            CaseID = caseID;
        }

        public void Sort(TreatmentSortType type)
        {
            Sort(0, ElementCount - 1, type);
            for (int i = 0; i < ElementCount; i++)
                Elements[i].Index = i;
        }

        private void Sort(int startIndex, int pivotIndex, TreatmentSortType type)
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
         * TreatmentSortType must be treatment type or id or staff id
         */
        public Treatment Find(string value, TreatmentSortType type)
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
                    if (type == TreatmentSortType.TreatmentType)
                        compareValue = Elements[mid].TreatmentType;
                    if (type == TreatmentSortType.EmployeeID)
                        compareValue = Elements[mid].EmployeeID;
                    if (type == TreatmentSortType.TreatmentID)
                        compareValue = Elements[mid].TreatmentID;

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
         * TreatmentSortType must be amount or outcome
         */
        public Treatment Find(int value, TreatmentSortType type)
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
                    if (type == TreatmentSortType.Amount)
                        compareValue = Elements[mid].Amount;
                    if (type == TreatmentSortType.Outcome)
                        compareValue = Elements[mid].Outcome;

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
         * TreatmentSortType must be start date or end date
         */
        public Treatment Find(DateTime value, TreatmentSortType type)
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
                    if (type == TreatmentSortType.StartDate)
                        compareValue = Elements[mid].StartDate;
                    if (type == TreatmentSortType.EndDate)
                        compareValue = Elements[mid].EndDate;

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
            using (var reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/treatment_coa.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                for (int i = 0; csv.Read(); i++)
                {
                    if (csv.GetField("case_id") == CaseID)
                    {
                        string treatmentID = csv.GetField("treatment_id");
                        string staffID = csv.GetField("staff_id");
                        string treatmentType = csv.GetField("treatment_type");
                        int outcome = 0;
                        if (Double.TryParse(csv.GetField("outcome"), out double result1))
                            outcome = (int)result1;

                        DateTime startDate;
                        startDate = new DateTime(Int32.Parse(csv.GetField("start_date").Substring(0, 4)), Int32.Parse(csv.GetField("start_date").Substring(5, 2)), Int32.Parse(csv.GetField("start_date").Substring(8, 2)), 0, 0, 0);
                        DateTime endDate;
                        endDate = new DateTime(Int32.Parse(csv.GetField("end_date").Substring(0, 4)), Int32.Parse(csv.GetField("end_date").Substring(5, 2)), Int32.Parse(csv.GetField("end_date").Substring(8, 2)), 0, 0, 0);

                        Treatment record = new Treatment(treatmentType, outcome, startDate, startDate, staffID, treatmentID, i);
                        Add(record);
                    }
                }
            }
        }

        public void AppendCSV(Treatment treatment)
        {
            List<object> records = new List<object> { new { treatment_id = treatment.TreatmentID, case_id = CaseID, treatment_type = treatment.TreatmentType, start_date = treatment.StartDate.ToString("yyyy-MM-dd"), end_date = treatment.EndDate.ToString("yyyy-MM-dd"), mortality_flag = false, outcome = treatment.Outcome, treatment_cost = treatment.Amount } };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, };

            using (var stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/treatment_coa.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.NextRecord();
                csv.WriteRecords(records);
            }
        }
    }
    public class Treatment
    {
        public string TreatmentType { get; set; }
        public int Outcome { get; set; }
        public int Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EmployeeID { get; set; }
        public string TreatmentID { get; set; }
        public int Index { get; set; }

        public Dictionary<string, FinanceArray> Finances { get; private set; }

        public Treatment(string treatmentType, int outcome, DateTime startDate, DateTime endDate, string employeeID, string treatmentID, int index)
        {
            TreatmentType = treatmentType;
            Outcome = outcome;
            StartDate = startDate;
            EndDate = endDate;
            EmployeeID = employeeID;
            TreatmentID = treatmentID;
            Index = index;
            Finances = new Dictionary<string, FinanceArray>();
            FinanceArray tempArr = new FinanceArray(treatmentID);
            tempArr.ReadCSV();

            for (int i = 0; i < tempArr.ElementCount; i++)
            {
                if (Finances.TryGetValue(tempArr.GetElement(i).FinanceName, out FinanceArray arr))
                    arr.Add(tempArr.GetElement(i));
                else
                {
                    arr = new FinanceArray(TreatmentID);
                    Finances.Add(tempArr.GetElement(i).FinanceName, arr);
                    arr.Add(tempArr.GetElement(i));
                }
            }
            UpdateTotal();
        }

        public void UpdateTotal()
        {
            int amount = 0;
            foreach (KeyValuePair<string, FinanceArray> financeArray in Finances)
                for (int i = 0; i < financeArray.Value.ElementCount; i++)
                    amount += financeArray.Value.GetElement(i).Cost;

            Amount = amount;
        }

        public int CompareTo(Treatment other, TreatmentSortType type)
        {
            if (type == TreatmentSortType.TreatmentID)
                return String.Compare(TreatmentID, other.TreatmentID, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == TreatmentSortType.EmployeeID)
                return String.Compare(EmployeeID, other.EmployeeID, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == TreatmentSortType.TreatmentType)
                return String.Compare(TreatmentType, other.TreatmentType, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == TreatmentSortType.Outcome && Outcome > other.Outcome)
                return Outcome.CompareTo(other.Outcome);
            if (type == TreatmentSortType.Amount && Amount > other.Amount)
                return Amount.CompareTo(other.Amount);
            if (type == TreatmentSortType.StartDate)
                return (StartDate.DayOfYear + StartDate.Year * 365) - (other.StartDate.DayOfYear + other.StartDate.Year * 365);
            if (type == TreatmentSortType.EndDate)
                return (EndDate.DayOfYear + EndDate.Year * 365) - (other.EndDate.DayOfYear + other.EndDate.Year * 365);
            return 0;
        }

        public override string ToString()
        {
            return  TreatmentID + ": " + TreatmentType + ", primary physician: " + EmployeeID + ", outcome: " + Outcome + ", ¥" + Amount + ", " + StartDate.ToString() + ", " + EndDate.ToString();
        }
    }

    public enum TreatmentSortType
    {
        TreatmentID,
        EmployeeID,
        TreatmentType,
        Outcome,
        Amount,
        StartDate,
        EndDate
    }
}
