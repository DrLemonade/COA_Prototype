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
    public partial class PatientsList : Form
    {
        private PatientArray patientArray;
        public PatientsList()
        {
            InitializeComponent();
        }

        public void PatientsList_Load(object sender, EventArgs e)
        {
            patientArray = Program.patients;

            patientListView.Columns.Add("Last Name", 50);
            patientListView.Columns.Add("First Name", 100);
            patientListView.Columns.Add("Gender", 150);
            patientListView.Columns.Add("Date Of Birth", 200);
            patientListView.Columns.Add("Patient ID", 250);

            for (int i = 0; i < patientArray.ElementCount; i++)
            {
                Patient patient = patientArray.GetElement(i);
                patientListView.Items.Add(new ListViewItem(new[] { patient.LastName, patient.FirstName, patient.Gender, patient.DOB.ToString(), patient.PatientID }));
            }

            sortByComboBox.SelectedIndex = 0;
        }

        private void sortByComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sortByComboBox.SelectedIndex == 0)
                patientArray.Sort(PatientSortType.Name);
            if (sortByComboBox.SelectedIndex == 1)
                patientArray.Sort(PatientSortType.DOB);
            if (sortByComboBox.SelectedIndex == 2)
                patientArray.Sort(PatientSortType.PatientID);

            patientListView.Items.Clear();
            for (int i = 0; i < patientArray.ElementCount; i++)
            {
                Patient patient = patientArray.GetElement(i);
                patientListView.Items.Add(new ListViewItem(new[] { patient.LastName, patient.FirstName, patient.Gender, patient.DOB.ToString(), patient.PatientID }));
            }
        }

        private void patientSearchBox_TextChanged(object sender, EventArgs e)
        {
            patientArray = (PatientArray)Program.patients.SubPatientArray(patientSearchBox.Text);

            patientListView.Items.Clear();
            for (int i = 0; i < patientArray.ElementCount; i++)
            {
                Console.WriteLine(i);
                Patient patient = patientArray.GetElement(i);
                patientListView.Items.Add(new ListViewItem(new[] { patient.LastName, patient.FirstName, patient.Gender, patient.DOB.ToString(), patient.PatientID }));
            }
        }
    }
}
