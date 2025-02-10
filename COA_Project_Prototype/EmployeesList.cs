using COA_ProjectPrototype;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COA_Project_Prototype
{
    public partial class EmployeesList : Form
    {
        private EmployeeArray employeeArray;
        public EmployeesList()
        {
            InitializeComponent();
        }

        private void sortByComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sortByComboBox.SelectedIndex == 0)
                Program.employees.Sort(EmployeeSortType.Name);
            if (sortByComboBox.SelectedIndex == 1)
                Program.employees.Sort(EmployeeSortType.Specialization);
            if (sortByComboBox.SelectedIndex == 2)
                Program.employees.Sort(EmployeeSortType.EmployeeType);
            if (sortByComboBox.SelectedIndex == 3)
                Program.employees.Sort(EmployeeSortType.EmployeeID);

            employeeListView.Items.Clear();
            for (int i = 0; i < Program.employees.ElementCount; i++)
            {
                Employee employee = Program.employees.GetElement(i);
                employeeListView.Items.Add(new ListViewItem(new[] { employee.LastName, employee.FirstName, employee.Specialization, employee.EmployeeType, employee.EmployeeID }));
            }
        }

        public void EmployeesList_Load(object sender, EventArgs e)
        {
            employeeArray = Program.employees;
            employeeListView.Columns.Add("Last Name", 50);
            employeeListView.Columns.Add("First Name", 100);
            employeeListView.Columns.Add("Specialization", 150);
            employeeListView.Columns.Add("Employee Type", 200);
            employeeListView.Columns.Add("Staff ID", 250);

            for (int i = 0; i < Program.employees.ElementCount; i++)
            {
                Employee employee = Program.employees.GetElement(i);
                employeeListView.Items.Add(new ListViewItem(new[] { employee.LastName, employee.FirstName, employee.Specialization, employee.EmployeeType, employee.EmployeeID }));
            }

            sortByComboBox.SelectedIndex = 0;
        }

        private void employeeTextBox_TextChanged(object sender, EventArgs e)
        {
            employeeArray = Program.employees.SubEmployeeArray(employeeTextBox.Text);

            employeeListView.Items.Clear();
            for (int i = 0; i < employeeArray.ElementCount; i++)
            {
                Console.WriteLine(i);
                Employee employee = employeeArray.GetElement(i);
                employeeListView.Items.Add(new ListViewItem(new[] { employee.LastName, employee.FirstName, employee.Specialization, employee.EmployeeType, employee.EmployeeID }));
            }
        }
    }
}
