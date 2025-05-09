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

namespace COA_ProjectPrototype {
    public class UserArray : DynamicArray <User>
    {
        public UserArray() : base(new User[10], 0){}

        public void Sort(UserSortType type)
        {
            Sort(0, ElementCount - 1, type);
            for(int i = 0; i < ElementCount; i++)
                Elements[i].Index = i;
        }

        private void Sort(int startIndex, int pivotIndex, UserSortType type)
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

        public User Find(string value, UserSortType type)
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
                    if (type == UserSortType.Name)
                        compareValue = Elements[mid].LastName + Elements[mid].FirstName;
                    if (type == UserSortType.Username)
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
            using (var reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/users_coa.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                for(int i = 0;  csv.Read(); i++)
                {
                    User record = null;
                    string username = csv.GetField("user_id");
                    while(username.IndexOf(" ") > 0)
                        username = username.Remove(username.IndexOf(" "));
                    string firstName = csv.GetField("user_first_name");
                    string lastName = csv.GetField("user_last_name");
                    string password = csv.GetField("password");
                    switch (csv.GetField("user_type"))
                    {
                        case "E": record = new Executive(firstName, lastName, username, password, i); break;
                        case "DC": record = new DeptChair(firstName, lastName, username, password, i); break;
                        case "PM": record = new ProviderManager(firstName, lastName, username, password, i); break;
                        case "P": record = new Provider(firstName, lastName, username, password, i); break;
                        case "A": record = new Analyst(firstName, lastName, username, password, i); break;
                        case "SA": record = new SiteAdministrator(firstName, lastName, username, password, i); break;
                        case "DA": record = new DataAdministrator(firstName, lastName, username, password, i); break;
                        case "TA": record = new TenantAdministrator(firstName, lastName, username, password, i); break;
                        default: record = new OtherEmployee(firstName, lastName, username, password, i, csv.GetField("user_type")); break;
                    };
                    Add(record);
                }
            }
        }

        public void AppendCSV(User employee)
        {
            List<object> records = null;
            if (employee is Executive)
                records = new List<object> { new { user_id = employee.Username, hospital_id = "H1", user_first_name = employee.FirstName, employee_type = "E", password = employee.Password } };
            if (employee is DeptChair)
                records = new List<object> { new { user_id = employee.Username, hospital_id = "H1", user_first_name = employee.FirstName, employee_type = "DC", password = employee.Password } };
            if (employee is ProviderManager)
                records = new List<object> { new { user_id = employee.Username, hospital_id = "H1", user_first_name = employee.FirstName, employee_type = "PM", password = employee.Password } };
            if (employee is Provider)
                records = new List<object> { new { user_id = employee.Username, hospital_id = "H1", user_first_name = employee.FirstName, employee_type = "P", password = employee.Password } };
            if (employee is Analyst)
                records = new List<object> { new { user_id = employee.Username, hospital_id = "H1", user_first_name = employee.FirstName, employee_type = "A", password = employee.Password } };
            if (employee is SiteAdministrator)
                records = new List<object> { new { user_id = employee.Username, hospital_id = "H1", user_first_name = employee.FirstName, employee_type = "SA", password = employee.Password } };
            if (employee is DataAdministrator)
                records = new List<object> { new { user_id = employee.Username, hospital_id = "H1", user_first_name = employee.FirstName, employee_type = "DA", password = employee.Password } };
            if (employee is TenantAdministrator)
                records = new List<object> { new { user_id   = employee.Username, hospital_id = "H1", user_first_name = employee.FirstName, employee_type = "TA", password = employee.Password } };
            if (employee is OtherEmployee)
                records = new List<object> { new { user_id = employee.Username, hospital_id = "H1", user_first_name = employee.FirstName, employee_type = ((OtherEmployee)employee).Type, password = employee.Password } };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, };

            using (var stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/users_coa.csv", FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.NextRecord();
                csv.WriteRecords(records);
            }
        }
    }

    public abstract class User
    {
        public int Index { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }


        public User(string firstName, string lastName, string username, string password, int index)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Username = username;
            this.Password = password;
            this.Index = index;
        }
        public double CompareTo(User other, UserSortType type)
        {
            if (type == UserSortType.Name)
                return String.Compare(LastName + FirstName, other.LastName + other.FirstName, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == UserSortType.Username)
                return String.Compare(Username, other.Username, comparisonType: StringComparison.OrdinalIgnoreCase);
            return 0;
        }
    }

    public class OtherEmployee : User
    {
        public string Type { get; set; }

        public OtherEmployee(string firstName, string lastName, string username, string password, int index, string type) : base(firstName, lastName, username, password, index)
        {
            Type = type;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName + ": " + Username + ", " + Password + ", " + Type;
        }
    }

    public class Executive : User
    {
        public Executive(string firstName, string lastName, string username, string password, int index) : base(firstName, lastName, username, password, index)
        {

        }

        public override string ToString()
        {
            return FirstName + " " + LastName + ": " + Username + ", " + Password;
        }
    }

    public class DeptChair : User
    {
        public DeptChair(string firstName, string lastName, string username, string password, int index) : base(firstName, lastName, username, password, index)
        {

        }

        public override string ToString()
        {
            return FirstName + " " + LastName + ": " + Username + ", " + Password;
        }
    }

    public class ProviderManager : User
    {
        public ProviderManager(string firstName, string lastName, string username, string password, int index) : base(firstName, lastName, username, password, index)
        {

        }

        public override string ToString()
        {
            return FirstName + " " + LastName + ": " + Username + ", " + Password;
        }
    }


    public class Provider : User
    {
        public Provider(string firstName, string lastName, string username, string password, int index) : base(firstName, lastName, username, password, index)
        {

        }

        public override string ToString()
        {
            return FirstName + " " + LastName + ": " + Username + ", " + Password;
        }
    }

    public class Analyst : User
    {
        public Analyst(string firstName, string lastName, string username, string password, int index) : base(firstName, lastName, username, password, index)
        {

        }

        public override string ToString()
        {
            return FirstName + " " + LastName + ": " + Username + ", " + Password;
        }
    }

    public class SiteAdministrator : User
    {
        public SiteAdministrator(string firstName, string lastName, string username, string password, int index) : base(firstName, lastName, username, password, index)
        {

        }

        public override string ToString()
        {
            return FirstName + " " + LastName + ": " + Username + ", " + Password;
        }
    }

    public class DataAdministrator : User
    {
        public DataAdministrator(string firstName, string lastName, string username, string password, int index) : base(firstName, lastName, username, password, index)
        {

        }

        public override string ToString()
        {
            return FirstName + " " + LastName + ": " + Username + ", " + Password;
        }
    }

    public class TenantAdministrator : User
    {
        public TenantAdministrator(string firstName, string lastName, string username, string password, int index) : base(firstName, lastName, username, password, index)
        {

        }

        public override string ToString()
        {
            return FirstName + " " + LastName + ": " + Username + ", " + Password;
        }
    }

    public enum UserSortType 
    {
        Name,
        Username
    }
}
