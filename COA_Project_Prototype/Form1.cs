﻿using COA_ProjectPrototype;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void logInButton_Click(object sender, EventArgs e)
        {
            User user = Program.Users.Find(usernameTextBox.Text, UserSortType.Username);
            if (user.Password.Equals(passwordTextBox.Text))
            {
                Program.CurrentForm = new homepage();
                this.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
