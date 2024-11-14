using System;

namespace COA_ProjectPrototype {
    public class PatientArray : DynamicArray
    {
        public Patient[] patients;
        public int PatientCount { get; private set; }

        public PatientArray()
        {
            patients = new Patient[10];
            patientCount = 0;
        }

        public void Add(Patient patient)
        {
            Insert(patient, patientCount);
        }

        public void Insert(Patient patient, )
    }

    public class Patient
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }

        public Patient(string name, string email)
        {
            this.Name = name;
            this.Email = email;
        }
        
        public Patient(string name, string email, string phone)
        {
            this.Name = name;
            this.Email = email;
            this.Phone = phone;
        }
    }
}