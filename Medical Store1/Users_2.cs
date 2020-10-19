using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Medical_Store
{
    public partial class Users_2 : Sample2
    {
        int edit = 0; //this 0 is an indication for save operation and 1 is an indication for update operation
        int userID;
        short stat;
        reterival r = new reterival();
        Regex reg = new Regex(@"^-?[0-9][0-9,\.]+$");
        public Users_2()
        {
            InitializeComponent();
        }

        private void Users_2_Load(object sender, EventArgs e)
        {
            MainClass.disable_reset(leftPanel);
            r.show_users(dataGridView1, userIDGV, NameGV, UserNameGV, PassGV, PhoneGV, EmailGV, StatusGV);
            userLabel.Text = reterival.EMP_NAME;
        }

        public override void addBtn_Click(object sender, EventArgs e)
        {
            MainClass.enable_reset(leftPanel);
            edit = 0;
        }

        public override void editBtn_Click(object sender, EventArgs e)
        {
            edit = 1;
            MainClass.enable(leftPanel);
        }

        public override void savebtn_Click(object sender, EventArgs e)
        {
            string pattern = null;
            pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

            if (nameTxt.Text == "")
                nameError.Visible = true;
            else
                nameError.Visible = false;

            if (usernameTxt.Text == "")
                usrError.Visible = true;
            else
                usrError.Visible = false;

            if (pwdTxt.Text == "")
                pwdError.Visible = true;
            else
                pwdError.Visible = false;

            if (phTxt.Text.Length < 11 || !(reg.IsMatch(phTxt.Text)) || !(phTxt.Text.StartsWith("03")))
            {
                MessageBox.Show("Invalid Phone No.");
                phTxt.Text = "";
                phTxt.Focus();
            }

            if (phTxt.Text == "")
                phError.Visible = true;
            else
                phError.Visible = false;

            if (!(Regex.IsMatch(emailTxt.Text, pattern)))
            {
                MessageBox.Show("Invalid Email Address.");
                emailTxt.Text = "";
                emailTxt.Focus();
            }

            if (emailTxt.Text == "")
                emailError.Visible = true;
            else
                emailError.Visible = false;

            if (statusCB.SelectedIndex == -1)
                statusError.Visible = true;
            else
                statusError.Visible = false;

            if (nameError.Visible || usrError.Visible || pwdError.Visible || phError.Visible || emailError.Visible || statusError.Visible)
            {
                MainClass.show_msg("Fields with * are mandatory", "Stop", "Error"); //Error is the type of msg
            }
            else
            {
                if (statusCB.SelectedIndex == 0)
                {
                    stat = 1;
                }
                else if (statusCB.SelectedIndex == 1)
                {
                    stat = 0;
                }
                if (edit == 0) // code for save operation
                {
                    insertion i = new insertion();
                    i.insertUsers(nameTxt.Text, usernameTxt.Text, pwdTxt.Text, emailTxt.Text, phTxt.Text, stat);
                    r.show_users(dataGridView1, userIDGV, NameGV, UserNameGV, PassGV, PhoneGV, EmailGV, StatusGV);
                    MainClass.disable_reset(leftPanel);

                }
                else if (edit == 1) // code for update operation
                {
                    DialogResult dr = MessageBox.Show("Are you sure, you want to update record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        updation u = new updation();

                        if (statusCB.SelectedIndex == 0)
                            stat = 1;
                        else if (statusCB.SelectedIndex == 1)
                            stat = 0;

                        u.updateUsers(userID, nameTxt.Text, usernameTxt.Text, pwdTxt.Text, emailTxt.Text, phTxt.Text, stat);
                        r.show_users(dataGridView1, userIDGV, NameGV, UserNameGV, PassGV, PhoneGV, EmailGV, StatusGV);
                        MainClass.disable_reset(leftPanel);
                    }
                }
            }

        }

        public override void delBtn_Click(object sender, EventArgs e)
        {
            if (edit == 1)
            {
                DialogResult dr = MessageBox.Show("Are you sure, you want to delete record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    deletion d = new deletion();
                    d.delete(userID, "st_deleteUser", "@id");
                    r.show_users(dataGridView1, userIDGV, NameGV, UserNameGV, PassGV, PhoneGV, EmailGV, StatusGV);
                    MainClass.disable_reset(leftPanel);
                }
            }
        }

        public override void searchTxt_TextChanged(object sender, EventArgs e)
        {
            if (searchTxt.Text != "")
            {
                r.show_users(dataGridView1, userIDGV, NameGV, UserNameGV, PassGV, PhoneGV, EmailGV, StatusGV, searchTxt.Text);
            }
            else
            {
                r.show_users(dataGridView1, userIDGV, NameGV, UserNameGV, PassGV, PhoneGV, EmailGV, StatusGV);
            }
        }

        public override void viewBtn_Click(object sender, EventArgs e)
        {
            r.show_users(dataGridView1, userIDGV, NameGV, UserNameGV, PassGV, PhoneGV, EmailGV, StatusGV);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                edit = 1;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                userID = Convert.ToInt32(row.Cells["userIDGV"].Value.ToString());
                nameTxt.Text = row.Cells["NameGV"].Value.ToString();
                usernameTxt.Text = row.Cells["UserNameGV"].Value.ToString();
                pwdTxt.Text = row.Cells["PassGV"].Value.ToString();
                phTxt.Text = row.Cells["PhoneGV"].Value.ToString();
                emailTxt.Text = row.Cells["EmailGV"].Value.ToString();
                statusCB.SelectedItem = row.Cells["StatusGV"].Value.ToString();
                MainClass.disable(leftPanel);
            }
        }
    }
}
