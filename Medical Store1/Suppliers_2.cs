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
    public partial class Suppliers_2 : Sample2
    {
        int edit = 0; //this 0 is an indication for save operation and 1 is an indication for update operation
        int supplierID;
        short stat;
        reterival r = new reterival();
        Regex reg = new Regex(@"^-?[0-9][0-9,\.]+$");
        public Suppliers_2()
        {
            InitializeComponent();
        }

        private void Suppliers_2_Load(object sender, EventArgs e)
        {
            MainClass.disable_reset(leftPanel);
            userLabel.Text = reterival.EMP_NAME;
            r.show_suppliers(dataGridView1, suppIDGV, companyGV, personGV, mobileGV, phone2GV, addressGV, ntnGV, StatusGV);
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
            if (supTxt.Text == "")
                supError.Visible = true;
            else
                supError.Visible = false;

            if (cnt_PersonTxt.Text == "")
                personError.Visible = true;
            else
                personError.Visible = false;

            if (mobTxt.Text.Length < 11 || !(reg.IsMatch(mobTxt.Text)) || !(mobTxt.Text.StartsWith("03")))
            {
                MessageBox.Show("Invalid Mobile No.");
                mobTxt.Text = "";
                mobTxt.Focus();
            }

            if (phTxt.Text.Length < 11 || !(reg.IsMatch(phTxt.Text)) || !(phTxt.Text.StartsWith("0")))
            {
                MessageBox.Show("Invalid Phone No.");
                phTxt.Text = "";
                phTxt.Focus();
            }

            if (mobTxt.Text == "")
                mobError.Visible = true;
            else
                mobError.Visible = false;

            if (addressTxt.Text == "")
                addressError.Visible = true;
            else
                addressError.Visible = false;

            if (statusDD.SelectedIndex == -1)
                statusError.Visible = true;
            else
                statusError.Visible = false;

            if (!(reg.IsMatch(ntnTxt.Text)))
            {
                MessageBox.Show("Invalid NTN No.");
                ntnTxt.Text = "";
                ntnTxt.Focus();
            }

            if (supError.Visible || personError.Visible || mobError.Visible || addressError.Visible || statusError.Visible)
            {
                MainClass.show_msg("Fields with * are mandatory", "Stop", "Error"); //Error is the type of msg
            }
            else
            {
                if (statusDD.SelectedIndex == 0)
                    stat = 1;
                else if (statusDD.SelectedIndex == 1)
                    stat = 0;
                if (edit == 0) // code for save operation
                {
                    insertion i = new insertion();
                    if (phTxt.Text == "" && ntnTxt.Text != "")
                        i.insertSupplier(supTxt.Text, cnt_PersonTxt.Text, mobTxt.Text, addressTxt.Text, stat, null, ntnTxt.Text);
                    else if (phTxt.Text != "" && ntnTxt.Text == "")
                        i.insertSupplier(supTxt.Text, cnt_PersonTxt.Text, mobTxt.Text, addressTxt.Text, stat, phTxt.Text, null);
                    else if (phTxt.Text == "" && ntnTxt.Text == "")
                        i.insertSupplier(supTxt.Text, cnt_PersonTxt.Text, mobTxt.Text, addressTxt.Text, stat, null, null);
                    else
                        i.insertSupplier(supTxt.Text, cnt_PersonTxt.Text, mobTxt.Text, addressTxt.Text, stat, phTxt.Text, ntnTxt.Text);
                    r.show_suppliers(dataGridView1, suppIDGV, companyGV, personGV, mobileGV, phone2GV, addressGV, ntnGV, StatusGV);
                    MainClass.disable_reset(leftPanel);

                }
                else if (edit == 1) // code for update operation
                {
                    DialogResult dr = MessageBox.Show("Are you sure, you want to update record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        updation u = new updation();

                        if (statusDD.SelectedIndex == 0)
                            stat = 1;
                        else if (statusDD.SelectedIndex == 1)
                            stat = 0;

                        if (phTxt.Text == "" && ntnTxt.Text != "")
                            u.updateSupplier(supplierID, supTxt.Text, cnt_PersonTxt.Text, mobTxt.Text, addressTxt.Text, stat, null, ntnTxt.Text);
                        else if (phTxt.Text != "" && ntnTxt.Text == "")
                            u.updateSupplier(supplierID, supTxt.Text, cnt_PersonTxt.Text, mobTxt.Text, addressTxt.Text, stat, phTxt.Text, null);
                        else if (phTxt.Text == "" && ntnTxt.Text == "")
                            u.updateSupplier(supplierID, supTxt.Text, cnt_PersonTxt.Text, mobTxt.Text, addressTxt.Text, stat, null, null);
                        else
                            u.updateSupplier(supplierID, supTxt.Text, cnt_PersonTxt.Text, mobTxt.Text, addressTxt.Text, stat, phTxt.Text, ntnTxt.Text);
                        r.show_suppliers(dataGridView1, suppIDGV, companyGV, personGV, mobileGV, phone2GV, addressGV, ntnGV, StatusGV);
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
                    d.delete(supplierID, "st_deleteSupplier", "@suppID");
                    r.show_suppliers(dataGridView1, suppIDGV, companyGV, personGV, mobileGV, phone2GV, addressGV, ntnGV, StatusGV);
                }
            }
        }

        public override void searchTxt_TextChanged(object sender, EventArgs e)
        {
            if (searchTxt.Text != "")
            {
                r.show_suppliers(dataGridView1, suppIDGV, companyGV, personGV, mobileGV, phone2GV, addressGV, ntnGV, StatusGV, searchTxt.Text);
            }
            else
            {
                r.show_suppliers(dataGridView1, suppIDGV, companyGV, personGV, mobileGV, phone2GV, addressGV, ntnGV, StatusGV);
            }
        }

        public override void viewBtn_Click(object sender, EventArgs e)
        {
            r.show_suppliers(dataGridView1, suppIDGV, companyGV, personGV, mobileGV, phone2GV, addressGV, ntnGV, StatusGV);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                edit = 1;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                supplierID = Convert.ToInt32(row.Cells["suppIDGV"].Value.ToString());
                supTxt.Text = row.Cells["companyGV"].Value.ToString();
                cnt_PersonTxt.Text = row.Cells["personGV"].Value.ToString();
                mobTxt.Text = row.Cells["mobileGV"].Value.ToString();
                phTxt.Text = row.Cells["phone2GV"].Value.ToString();
                ntnTxt.Text = row.Cells["ntnGV"].Value.ToString();
                addressTxt.Text = row.Cells["addressGV"].Value.ToString();
                statusDD.SelectedItem = row.Cells["StatusGV"].Value.ToString();
                MainClass.disable(leftPanel);
            }
        }
    }
}
