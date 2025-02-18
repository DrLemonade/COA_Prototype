using System;
using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Policy;
using System.Diagnostics;
using COA_Project_Prototype;

namespace COA_ProjectPrototype 
{
    public class PatientArray : DynamicArray<Patient>
    {
        public PatientArray() : base(new Patient[10], 0){  }

        public void Sort(PatientSortType type)
        {
            Sort(0, ElementCount - 1, type);
            for (int i = 0; i < ElementCount; i++)
                Elements[i].Index = i;
        }

        private void Sort(int startIndex, int pivotIndex, PatientSortType type)
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
        public Patient Find(string value, PatientSortType type)
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
                    if (type == PatientSortType.Name)
                        compareValue = Elements[mid].LastName+ Elements[mid].FirstName;
                    if (type == PatientSortType.PatientID)
                        compareValue = Elements[mid].PatientID;

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
         * Binary search based on a DateTime value.
         * PatientSortType must be DOB
         */
        public Patient Find(DateTime value, PatientSortType type)
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
                    if (type == PatientSortType.DOB)
                        compareValue = Elements[mid].DOB;

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

        /**
         * creates an array of patients that is a subsection based on a string
         */
        public PatientArray SubPatientArray(string input)
        {
            PatientArray array = new PatientArray();
            for(int i = 0; i < ElementCount; i++)
            {
                for(int j = 0; j < Elements[i].ToString().Length - input.Length - 1; j++)
                {
                    if (Elements[i].ToString().Substring(j, input.Length) == input)
                        array.Add(Elements[i]);
                }
            }

            return array;
        }

        public void ReadCSV()
        {
            using (var reader = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/_patient__202502131232.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                for (int i = 0; csv.Read(); i++)
                {
                    string firstName = csv.GetField("patient_first_name");
                    string lastName = csv.GetField("patient_last_name");
                    DateTime dob;
                    dob = new DateTime(Int32.Parse(csv.GetField("date_of_birth").Substring(0, 4)), Int32.Parse(csv.GetField("date_of_birth").Substring(5, 2)), Int32.Parse(csv.GetField("date_of_birth").Substring(8, 2)), 0, 0, 0);
                    
                    string gender = csv.GetField("gender");
                    string patientID = csv.GetField("patient_id");

                    Patient record = new Patient(firstName, lastName, gender, dob, patientID, i);
                    Add(record);
                }
            }

            Sort(PatientSortType.Name);
        }

        public void AppendCSV(Patient patient)
        { 
            List<object> records = new List<object> { new { patient_id = patient.PatientID, hospital_id = "H1", patient_first_name = patient.FirstName, patient_last_name = patient.LastName, date_of_birth = patient.DOB.ToString("yyyy-MM-dd"), gender = patient.Gender } };

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, };

            using (var stream = File.Open(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/_patient__202502131232.csv", FileMode.Append))
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

        public string PatientID { get; set; }
        public int Index { get; set; }
        public CaseArray Cases { get; set; } 

        public Patient(string firstName, string lastName, string gender, DateTime dob, string patientID, int index)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
            this.DOB = dob;
            this.PatientID = patientID;
            this.Index = index;

            Cases = new CaseArray(patientID);
            Cases.ReadCSV();
        }

        public void AddCase(Case e)
        {
            Cases.Add(e);
        }

        public double CompareTo(Patient other, PatientSortType type)
        {
            if (type == PatientSortType.Name)   
                return String.Compare(LastName+FirstName, other.LastName+other.FirstName, comparisonType: StringComparison.OrdinalIgnoreCase);
            if (type == PatientSortType.DOB)
                return (DOB.DayOfYear + DOB.Year * 365) - (other.DOB.DayOfYear + other.DOB.Year * 365);
            if (type == PatientSortType.PatientID)
                return String.Compare(PatientID, other.PatientID, comparisonType: StringComparison.OrdinalIgnoreCase);
            return 0;
        }

        public override string ToString()
        {
            string str = FirstName + " " + LastName + ": " + Index + ", " + Gender + ", " + DOB + "\n";
            foreach (Case c in Cases)
                str += c.ToString() + "\n";
            return str;
        }
    }
    public enum PatientSortType
    {
        Name, DOB, PatientID
    }
}