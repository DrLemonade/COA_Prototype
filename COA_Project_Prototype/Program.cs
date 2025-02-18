using COA_ProjectPrototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COA_Project_Prototype
{
    internal static class Program
    {

        public static UserArray Users { get; set; }
        public static PatientArray Patients { get; set; }
        public static EmployeeArray Employees { get; set; }
        public static Form CurrentForm { get; set; }

        [STAThread]
        static void Main()
        {
            Users = new UserArray();
            Patients = new PatientArray();
            Employees = new EmployeeArray();
            Users.ReadCSV();
            Patients.ReadCSV();
            Employees.ReadCSV();

            foreach (Patient patient in Patients)
                Console.WriteLine(patient.ToString());
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // loops running new forms. current form is set to another form in form classes
            CurrentForm = new Form1();
            while (true)
            {
                try
                {
                    Application.Run(CurrentForm);
                }
                catch (ObjectDisposedException ex)
                {
                    break;
                }
            }
        }
    }
}
