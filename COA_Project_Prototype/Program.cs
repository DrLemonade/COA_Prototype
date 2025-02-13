﻿using COA_ProjectPrototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COA_Project_Prototype
{
    internal static class Program
    {

        public static UserArray users { get; set; }
        public static PatientArray patients { get; set; }
        public static EmployeeArray employees { get; set; }
        public static Form CurrentForm { get; set; }

        [STAThread]
        static void Main()
        {
            users = new UserArray();
            patients = new PatientArray();
            employees = new EmployeeArray();
            users.ReadCSV();
            patients.ReadCSV();
            employees.ReadCSV();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
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
