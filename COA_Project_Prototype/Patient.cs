using System;

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
                if (Elements[mid] == null || String.Compare(name, Elements[mid].Name, comparisonType: StringComparison.OrdinalIgnoreCase) < 0)
                    max = mid - 1;
                else if (String.Compare(name, Elements[mid].Name, comparisonType: StringComparison.OrdinalIgnoreCase) > 0)
                    min = mid + 1;
                else if (Elements[mid].Name.Equals(name))
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
    }

    public class Patient
    {
        // Patient information
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int MRN { get; set; }
        public double Balance { get; set; }

        public int Index { get; set; }
        public CaseArray Cases { get; set; } 

        public Patient(string name, string email)
        {
            Name = name;
            Email = email;
        }
        
        public Patient(string name, string email, string phone, int mrn, double balance)
        {
            Name = name;
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
            return String.Compare(Name, other.Name, comparisonType: StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return Name + ": " + Email + ", " + Phone + ", " + Balance;
        }
    }
}