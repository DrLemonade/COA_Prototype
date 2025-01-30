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
    public partial class PatientsList : Form
    {
        public PatientsList()
        {
            InitializeComponent();
        }

        public void PatientsList_Load(object sender, EventArgs e)
        {
            patientListView.Columns.Add("Last Name", 50);
            patientListView.Columns.Add("First Name", 100);
            patientListView.Columns.Add("Gender", 150);
            patientListView.Columns.Add("Date Of Birth", 200);
            patientListView.Columns.Add("Patient ID", 250);

            for (int i = 0; i < Program.patients.ElementCount; i++)
            {
                COA_ProjectPrototype.Patient patient = Program.patients.GetElement(i);
                patientListView.Items.Add(new ListViewItem(new[] { patient.LastName, patient.FirstName, patient.Gender, patient.DOB.ToString(), patient.PatientID }));
            }

            sortByComboBox.SelectedIndex = 0;
        }

        private void sortByComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sortByComboBox.SelectedIndex == 0)
                Program.patients.Sort(COA_ProjectPrototype.PatientSortType.Name);
            if (sortByComboBox.SelectedIndex == 1)
                Program.patients.Sort(COA_ProjectPrototype.PatientSortType.DOB);
            if (sortByComboBox.SelectedIndex == 2)
                Program.patients.Sort(COA_ProjectPrototype.PatientSortType.PatientID);

            patientListView.Items.Clear();
            for (int i = 0; i < Program.patients.ElementCount; i++)
            {
                COA_ProjectPrototype.Patient patient = Program.patients.GetElement(i);
                patientListView.Items.Add(new ListViewItem(new[] { patient.LastName, patient.FirstName, patient.Gender, patient.DOB.ToString(), patient.PatientID }));
            }
        }
    }
}
