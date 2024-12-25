using System;
using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Policy;

namespace COA_ProjectPrototype 
{
    public class PatientArray : DynamicArray<Patient>
    {
        public PatientArray() : base(new Patient[10], 0){  }

        public void Sort()
        {
            Sort(0, ElementCount - 1);
            for (int i = 0; i < ElementCount; i++)
                Elements[i].Index = i;
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

        public Patient FindPatient(string name)
        {
            Sort();
            int min = 0;
            int max = Elements.Length - 1;
            while (max >= min)
            {
                int mid = (min + max) / 2;
                if (Elements[mid] == null || String.Compare(name, Elements[mid].LastName + Elements[mid].FirstName, comparisonType: StringComparison.OrdinalIgnoreCase) < 0)
                    max = mid - 1;
                else if (String.Compare(name, Elements[mid].LastName + Elements[mid].FirstName, comparisonType: StringComparison.OrdinalIgnoreCase) > 0)
                    min = mid + 1;
                else if ((Elements[mid].LastName + Elements[mid].FirstName).Equals(name))
                    return Elements[mid];
            }
            return null;
        }

        public PatientArray SubPatientArray(string input)
        {
            PatientArray array = new PatientArray();
            for(int i = 0;i < ElementCount; i++)
            {
                if(Elements[i].ToString().Substring(0, input.Length) == input)
                    array.Add(Elements[i]);
            }

            return array;
        }

        public void ReadCSV()
        {
            using (var reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/COA_employees_data.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                for (int i = 0; csv.Read(); i++)
                {
                    string firstName = csv.GetField("patient_first_name");
                    string lastName = csv.GetField("patient_last_name");
                    DateTime dob = DateTime.ParseExact(csv.GetField("date_of_birth"), "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    string gender = csv.GetField("gender");
                    Patient record = new Patient(firstName, lastName, gender, dob, i);
                    Add(record);
                }
            }
        }

        public void AppendCSV(Employee employee)
        { 
            List<object> records = new List<object> { new { user_id = employee.Username, hospital_id = "H1", user_first_name = employee.FirstName, employee_type = ((OtherEmployee)employee).Type, password = employee.Password } };

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

    public class Patient
    {
        // Patient information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; } 
        public int MRN { get; set; }
        public double Balance { get; set; }
        public DateTime DOB { get; set; }

        public int Index { get; set; }
        public CaseArray Cases { get; set; } 

        public Patient(string firstName, string lastName, string gender, DateTime dob, int index)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
            this.DOB = dob;
            this.Index = index;
        }
        
        public Patient(string firstName, string lastName, string email, string phone, int mrn, double balance)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            MRN = mrn;
            Balance = balance;
        }

        public void AddEvent(Case e)
        {
            Cases.Add(e);
        }

        public double CompareTo(Patient other)
        {
            return String.Compare(LastName+FirstName, other.LastName+other.FirstName, comparisonType: StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return FirstName + " " + LastName + ": " + Email + ", " + Phone + ", " + Balance;
        }
    }
}