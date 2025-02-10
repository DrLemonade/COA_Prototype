namespace COA_Project_Prototype
{
    partial class EmployeesList
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
            this.sortByComboBox = new System.Windows.Forms.ComboBox();
            this.employeesListLabel = new System.Windows.Forms.Label();
            this.employeeListView = new System.Windows.Forms.ListView();
            this.employeeTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // sortByComboBox
            // 
            this.sortByComboBox.FormattingEnabled = true;
            this.sortByComboBox.Items.AddRange(new object[] {
            "Name",
            "Specialization",
            "Employee Type",
            "Employee ID"});
            this.sortByComboBox.Location = new System.Drawing.Point(667, 12);
            this.sortByComboBox.Name = "sortByComboBox";
            this.sortByComboBox.Size = new System.Drawing.Size(121, 21);
            this.sortByComboBox.TabIndex = 5;
            this.sortByComboBox.SelectedIndexChanged += new System.EventHandler(this.sortByComboBox_SelectedIndexChanged);
            // 
            // employeesListLabel
            // 
            this.employeesListLabel.AutoSize = true;
            this.employeesListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.employeesListLabel.Location = new System.Drawing.Point(12, 9);
            this.employeesListLabel.Name = "employeesListLabel";
            this.employeesListLabel.Size = new System.Drawing.Size(173, 37);
            this.employeesListLabel.TabIndex = 4;
            this.employeesListLabel.Text = "Employees";
            // 
            // employeeListView
            // 
            this.employeeListView.HideSelection = false;
            this.employeeListView.Location = new System.Drawing.Point(12, 99);
            this.employeeListView.Name = "employeeListView";
            this.employeeListView.Size = new System.Drawing.Size(776, 333);
            this.employeeListView.TabIndex = 3;
            this.employeeListView.UseCompatibleStateImageBehavior = false;
            this.employeeListView.View = System.Windows.Forms.View.Details;
            // 
            // employeeTextBox
            // 
            this.employeeTextBox.Location = new System.Drawing.Point(12, 73);
            this.employeeTextBox.Name = "employeeTextBox";
            this.employeeTextBox.Size = new System.Drawing.Size(776, 20);
            this.employeeTextBox.TabIndex = 6;
            this.employeeTextBox.TextChanged += new System.EventHandler(this.employeeTextBox_TextChanged);
            // 
            // EmployeesList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.employeeTextBox);
            this.Controls.Add(this.sortByComboBox);
            this.Controls.Add(this.employeesListLabel);
            this.Controls.Add(this.employeeListView);
            this.Name = "EmployeesList";
            this.Text = "EmployeesList";
            this.Load += new System.EventHandler(this.EmployeesList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox sortByComboBox;
        private System.Windows.Forms.Label employeesListLabel;
        private System.Windows.Forms.ListView employeeListView;
        private System.Windows.Forms.TextBox employeeTextBox;
    }
}