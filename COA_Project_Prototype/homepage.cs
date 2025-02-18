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
    public partial class homepage : Form
    {
        public homepage()
        {
            InitializeComponent();
        }

        private void homepage_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void usernameLabel_Click(object sender, EventArgs e)
        {

        }

        private void employeeButton_Click(object sender, EventArgs e)
        {
            Program.CurrentForm = new EmployeesList();
            this.Close();
        }

        private void logOutButton_Click(object sender, EventArgs e)
        {
            Program.CurrentForm = new Form1();
            this.Close();
        }

        private void patientsButton_Click(object sender, EventArgs e)
        {
            Program.CurrentForm = new PatientsList();
            this.Close();
        }
    }
}
