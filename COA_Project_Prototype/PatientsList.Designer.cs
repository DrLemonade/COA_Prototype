namespace COA_Project_Prototype
{
    partial class PatientsList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.patientListView = new System.Windows.Forms.ListView();
            this.patientsListLabel = new System.Windows.Forms.Label();
            this.sortByComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // patientListView
            // 
            this.patientListView.HideSelection = false;
            this.patientListView.Location = new System.Drawing.Point(12, 78);
            this.patientListView.Name = "patientListView";
            this.patientListView.Size = new System.Drawing.Size(776, 360);
            this.patientListView.TabIndex = 0;
            this.patientListView.UseCompatibleStateImageBehavior = false;
            this.patientListView.View = System.Windows.Forms.View.Details;
            // 
            // patientsListLabel
            // 
            this.patientsListLabel.AutoSize = true;
            this.patientsListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patientsListLabel.Location = new System.Drawing.Point(12, 24);
            this.patientsListLabel.Name = "patientsListLabel";
            this.patientsListLabel.Size = new System.Drawing.Size(132, 37);
            this.patientsListLabel.TabIndex = 1;
            this.patientsListLabel.Text = "Patients";
            // 
            // sortByComboBox
            // 
            this.sortByComboBox.FormattingEnabled = true;
            this.sortByComboBox.Items.AddRange(new object[] {
            "Name",
            "Date of Birth",
            "Patient ID"});
            this.sortByComboBox.Location = new System.Drawing.Point(667, 39);
            this.sortByComboBox.Name = "sortByComboBox";
            this.sortByComboBox.Size = new System.Drawing.Size(121, 21);
            this.sortByComboBox.TabIndex = 2;
            this.sortByComboBox.SelectedIndexChanged += new System.EventHandler(this.sortByComboBox_SelectedIndexChanged);
            // 
            // PatientsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.sortByComboBox);
            this.Controls.Add(this.patientsListLabel);
            this.Controls.Add(this.patientListView);
            this.Name = "PatientsList";
            this.Text = "PatientsList";
            this.Load += new System.EventHandler(this.PatientsList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView patientListView;
        private System.Windows.Forms.Label patientsListLabel;
        private System.Windows.Forms.ComboBox sortByComboBox;
    }
}