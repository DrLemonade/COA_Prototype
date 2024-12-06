using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace COA_ProjectPrototype 
{
    public class EmployeeArray : DynamicArray <Employee>
    {
        public EmployeeArray() : base(new Employee[10], 0){}

        public void Sort(EmployeeSortType type)
        {
            Sort(0, ElementCount - 1, type);
            for(int i = 0; i < ElementCount; i++)
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
                    if (type == EmployeeSortType.Name)
                        compareValue = Elements[mid].Name;
                    if (type == EmployeeSortType.Username)
                        compareValue = Elements[mid].Username;

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

        public void ReadCSV()
        {
            using (var reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/COA_employees_data.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                for(int i = 0;  csv.Read(); i++)
                {
                    Employee record = null;
                    string name = csv.GetField("name");
                    string username = csv.GetField("username");
                    string password = csv.GetField("password");
                    switch (csv.GetField("employee_type"))
                    {
                        case "Executive": record = new Executive(name, username, password, i); break;
                        case "DeptChair": record = new DeptChair(name, username, password, i); break;
                        case "ProviderManager": record = new ProviderManager(name, username, password, i); break;
                        case "Provider": record = new Provider(name, username, password, i); break;
                        case "Analyst": record = new Analyst(name, username, password, i); break;
                        case "SiteAdministrator": record = new SiteAdministrator(name, username, password, i); break;
                        case "DataAdministrator": record = new DataAdministrator(name, username, password, i); break;
                        case "TenantAdministrator": record = new TenantAdministrator(name, username, password, i); break;
                        default: record = new OtherEmployee(name, username, password, i, csv.GetField("employee_type")); break;
                    };
                    Add(record);
                }
            }
        }

        public void AppendCSV(Employee employee)
        {
            List<object> records = null;
            if (employee is Executive)
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "Executive" } };
            if (employee is DeptChair)
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "DeptChair" } };
            if (employee is ProviderManager)
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "ProviderManager" } };
            if (employee is Provider)
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "Provider" } };
            if (employee is Analyst)
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "Analyst" } };
            if (employee is SiteAdministrator)
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "SiteAdministrator" } };
            if (employee is DataAdministrator)
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "DataAdministrator" } };
            if (employee is TenantAdministrator)
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "TenantAdministrator" } };
            if (employee is OtherEmployee)
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = ((OtherEmployee)employee).Type } };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, };

            using (var stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/COA_employees_data.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.NextRecord();
                csv.WriteRecords(records);
            }
        }
    }

    public abstract class Employee
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }


        public Employee(string name, string username, string password, int index)
        {
            this.Name = name;
            this.Username = username;
            this.Password = password;
            this.Index = index;
        }
        public double CompareTo(Employee other, EmployeeSortType type)
        {
            if (type == EmployeeSortType.Name)
                return String.Compare(Name, other.Name, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == EmployeeSortType.Username)
                return String.Compare(Username, other.Username, comparisonType: StringComparison.OrdinalIgnoreCase);
            return 0;
        }
    }

    public class OtherEmployee : Employee
    {
        public string Type { get; set; }

        public OtherEmployee(string name, string username, string password, int index, string type) : base(name, username, password, index)
        {
            Type = type;
        }

        public override string ToString()
        {
            return Name + ": " + Username + ", " + Password + ", " + Type;
        }
    }

    public class Executive : Employee
    {
        public Executive(string name, string username, string password, int index) : base(name, username, password, index)
        {

        }

        public override string ToString()
        {
            return Name + ": " + Username + ", " + Password;
        }
    }

    public class DeptChair : Employee
    {
        public DeptChair(string name, string username, string password, int index) : base(name, username, password, index)
        {

        }

        public override string ToString()
        {
            return Name + ": " + Username + ", " + Password;
        }
    }

    public class ProviderManager : Employee
    {
        public ProviderManager(string name, string username, string password, int index) : base(name, username, password, index)
        {

        }

        public override string ToString()
        {
            return Name + ": " + Username + ", " + Password;
        }
    }


    public class Provider : Employee
    {
        public Provider(string name, string username, string password, int index) : base(name, username, password, index)
        {

        }

        public override string ToString()
        {
            return Name + ": " + Username + ", " + Password;
        }
    }

    public class Analyst : Employee
    {
        public Analyst(string name, string username, string password, int index) : base(name, username, password, index)
        {

        }

        public override string ToString()
        {
            return Name + ": " + Username + ", " + Password;
        }
    }

    public class SiteAdministrator : Employee
    {
        public SiteAdministrator(string name, string username, string password, int index) : base(name, username, password, index)
        {

        }

        public override string ToString()
        {
            return Name + ": " + Username + ", " + Password;
        }
    }

    public class DataAdministrator : Employee
    {
        public DataAdministrator(string name, string username, string password, int index) : base(name, username, password, index)
        {

        }

        public override string ToString()
        {
            return Name + ": " + Name + ", " + Password;
        }
    }

    public class TenantAdministrator : Employee
    {
        public TenantAdministrator(string name, string username, string password, int index) : base(name, username, password, index)
        {

        }

        public override string ToString()
        {
            return Name + ": " + Username + ", " + Password;
        }
    }

    public enum EmployeeSortType 
    {
        Name,
        Username
    }
}
