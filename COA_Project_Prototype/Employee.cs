using COA_ProjectPrototype;
using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace COA_Project_Prototype
{
    public class EmployeeArray : DynamicArray<Employee>
    {
        public EmployeeArray() : base(new Employee[10], 0) { }


        public void Sort(EmployeeSortType type)
        {
            Sort(0, ElementCount - 1, type);
            for (int i = 0; i < ElementCount; i++)
                Elements[i].Index = i;
        }

        private void Sort(int startIndex, int pivotIndex, EmployeeSortType type)
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
         * PatientSortType must be name or patientID
         */
        public Employee Find(string value, EmployeeSortType type)
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
                    if (type == EmployeeSortType.EmployeeID)
                        compareValue = Elements[mid].EmployeeID;
                    if (type == EmployeeSortType.Name)
                        compareValue = Elements[mid].LastName + Elements[mid].FirstName;
                    if (type == EmployeeSortType.Specialization)
                        compareValue = Elements[mid].Specialization;
                    if (type == EmployeeSortType.EmployeeType)
                        compareValue = Elements[mid].EmployeeType;

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
         * creates an array of patients that is a subsection based on a string
         */
        public EmployeeArray SubEmployeeArray(string input)
        {
            EmployeeArray array = new EmployeeArray();
            for (int i = 0; i < ElementCount; i++)
            {
                for (int j = 0; j < Elements[i].ToString().Length - input.Length - 1; j++)
                {
                    if (Elements[i].ToString().Substring(j, input.Length) == input)
                        array.Add(Elements[i]);
                }
            }

            return array;
        }

        public void ReadCSV()
        {
            using (var reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/staff_coa.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                for (int i = 0; csv.Read(); i++)
                {
                    string employeeID = csv.GetField("staff_id");
                    string firstName = csv.GetField("staff_first_name");
                    string lastName = csv.GetField("staff_last_name");
                    string specialization = csv.GetField("specialization");
                    string employeeType = csv.GetField("employee_type");

                    Employee record = new Employee(employeeID, firstName, lastName, specialization, employeeType, i);
                    Add(record);
                }
            }

            Sort(EmployeeSortType.Name);
        }

        public void AppendCSV(Employee employee)
        {
            List<object> records = new List<object> { new { staff_id = employee.EmployeeID, hospital_id = "H1", department_id = "", staff_first_name = employee.FirstName, staff_last_name = employee.LastName, specialization = employee.Specialization, employee_type = employee.EmployeeType } };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, };

            using (var stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/staff_coa.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.NextRecord();
                csv.WriteRecords(records);
            }
        }
    }

    public class Employee
    {
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization {  get; set; }
        public string EmployeeType { get; set; }
        public int Index { get; set; }

        public Employee(string employeeID, string firstName, string lastName, string specialization, string employeeType, int index) 
        {
            EmployeeID = employeeID;
            FirstName = firstName;
            LastName = lastName;
            Specialization = specialization;
            EmployeeType = employeeType;
            Index = index;
        }

        public int CompareTo(Employee other, EmployeeSortType type)
        {
            if (type == EmployeeSortType.EmployeeID)
                return String.Compare(EmployeeID, other.EmployeeID, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == EmployeeSortType.Name)
                return String.Compare(LastName + FirstName, other.LastName + other.FirstName, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == EmployeeSortType.Specialization)
                return String.Compare(Specialization, other.Specialization, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == EmployeeSortType.EmployeeType)
                return String.Compare(EmployeeType, other.EmployeeType, comparisonType: StringComparison.OrdinalIgnoreCase);
            return 0;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName + ": " + ", " + Specialization + ", " + EmployeeType + ", " + EmployeeID + ", " + Index;
        }
    }

    public enum EmployeeSortType 
    {
        EmployeeID, Name, Specialization, EmployeeType
    }
}
