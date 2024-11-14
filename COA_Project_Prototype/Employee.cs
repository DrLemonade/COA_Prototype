using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using CsvHelper;
using CsvHelper.Configuration;

namespace COA_ProjectPrototype {
    public class EmployeeArray : DynamicArray <Employee>
    {
        private Employee[] employees;
        public int EmployeeCount { get; private set; }

        public EmployeeArray()
        {
            employees = new Employee[10];
            EmployeeCount = 0;
        }

        public void Add(Employee employee)
        {
            Insert(employee, EmployeeCount);
        }

        public void Insert(Employee employee, int index)
        {
            if (index < 0 || index > EmployeeCount)
                throw new IndexOutOfRangeException();
            if (EmployeeCount == employees.Length)
                DoubleBackingArray();
            for (int i = EmployeeCount; i > index; i--)
                employees[i + 1] = employees[1];
            employees[index] = employee;
            EmployeeCount++;
        }

        private void DoubleBackingArray()
        {
            Employee[] newArray = new Employee[employees.Length * 2];
            for (int i = 0; i < employees.Length; i++) {
                newArray[i] = employees[i];
            }
            employees = newArray;
        }

        public void Remove(int index)
        {
            if (index < 0 || index > EmployeeCount)
                throw new IndexOutOfRangeException();

            for (int i = index + 1; i < EmployeeCount; i++)
                employees[i - 1] = employees[i];

            employees[EmployeeCount] = null;
            EmployeeCount--;
        }

        public void Sort(bool byName)
        {
            Sort(0, EmployeeCount - 1, byName);
            for(int i = 0; i < EmployeeCount; i++)
                employees[i].Index = i;
        }

        private void Sort(int startIndex, int pivotIndex, bool byName)
        {
            if (startIndex >= pivotIndex)
                return;

            int i = startIndex - 1;
            int j = startIndex;
            while (j <= pivotIndex)
            {
                if (employees[j].CompareTo(employees[pivotIndex], byName) <= 0)
                {
                    i++;
                    (employees[j], employees[i]) = (employees[i], employees[j]);
                }
                j++;
            }

            Sort(startIndex, i - 1, byName);
            Sort(i + 1, pivotIndex, byName);
        }

        public Employee LogIn(string username)
        {
            Sort(false);
            int min = 0;
            int max = employees.Length - 1;
            while (max >= min)
            {
                int mid = (min + max) / 2;
                if (employees[mid] == null || String.Compare(username, employees[mid].Username, comparisonType: StringComparison.OrdinalIgnoreCase) < 0)
                    max = mid - 1;
                else if (String.Compare(username, employees[mid].Username, comparisonType: StringComparison.OrdinalIgnoreCase) > 0)
                    min = mid + 1;
                else if (employees[mid].Username.Equals(username))
                    return employees[mid];
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
                int i = 0;
                while (csv.Read())
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
                        case "TenantAdministrator": record = new TenantAdministrator(name, csv.GetField("username"), password, i); break;
                    };
                    Add(record);
                    i++;
                }
            }
        }

        public void AppendCSV(Employee employee)
        {
            List<object> records = null;
            if (employee is Executive)
            {
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "Executive" } };
            }
            if (employee is DeptChair)
            {
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "DeptChair" } };
            }
            if (employee is ProviderManager)
            {
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "ProviderManager" } };
            }
            if (employee is Provider)
            {
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "Provider" } };
            }
            if (employee is Analyst)
            {
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "Analyst" } };
            }
            if (employee is SiteAdministrator)
            {
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "SiteAdministrator" } };
            }
            if (employee is DataAdministrator)
            {
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "DataAdministrator" } };
            }
            if (employee is TenantAdministrator)
            {
                records = new List<object> { new { name = employee.Name, username = employee.Username, password = employee.Password, employee_type = "TenantAdministrator" } };
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/COA_employees_data.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.NextRecord();
                csv.WriteRecords(records);
            }
        }

        public Employee GetEmployee(int index) { return employees[index]; }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < EmployeeCount; i++)
            {
                result += " \n" + employees[i].Index + " " + employees[i].ToString();
            }

            return result;
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
        public double CompareTo(Employee other, bool byName)
        {
            if (byName)
                return String.Compare(Name, other.Name, comparisonType: StringComparison.OrdinalIgnoreCase);
            else
                return String.Compare(Username, other.Username, comparisonType: StringComparison.OrdinalIgnoreCase);
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
}
