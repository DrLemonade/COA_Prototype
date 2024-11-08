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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            EmployeeArray employeeArray = new EmployeeArray();
            employeeArray.ReadCSV();
            employeeArray.Sort(false);
            Console.WriteLine(employeeArray.ToString());
            employeeArray.LogIn("jdoe01").ToString();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
