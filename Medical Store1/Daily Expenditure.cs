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
    public partial class Daily_Expenditure : Sample2
    {
        int edit = 0; //this 0 is an indication for save operation and 1 is an indication for update operation
        int expID;
        reterival r = new reterival();
        insertion i = new insertion();
        updation u = new updation();
        public Daily_Expenditure()
        {
            InitializeComponent();
        }

        private void Daily_Expenditure_Load(object sender, EventArgs e)
        {
            MainClass.disable_reset(leftPanel);
            r.showDailyExp(dataGridView1,expenseIDGV,descGV,amountGV,dateGV);
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
            if (descTxt.Text == "")
                descError.Visible = true;
            else
                descError.Visible = false;

            if (amountTxt.Text == "")
                amountError.Visible = true;
            else
                amountError.Visible = false;

            if (descError.Visible || amountError.Visible | dateError.Visible)
            {
                MainClass.show_msg("Fields with * are mandatory", "Stop", "Error"); //Error is the type of msg
            }
            else
            {
                float updated_expamount;
                if (edit == 0) // code for save operation
                
                {
                    insertion i = new insertion();
                    i.insertDailyExpenditure(descTxt.Text, Convert.ToSingle(amountTxt.Text), dateTimePicker2.Value);
                    object exp_id = r.getExpID();
                    object exp_amount = r.getExpAmount(dateTimePicker2.Value.Date);
                    if (exp_amount == null)
                    {
                        i.insertExpAmount(Convert.ToInt64(exp_id), Convert.ToSingle(amountTxt.Text), dateTimePicker2.Value);
                    }
                    else
                    {
                        updated_expamount = Convert.ToSingle(exp_amount) + Convert.ToSingle(amountTxt.Text);
                        u.updateExpAmount(updated_expamount, dateTimePicker2.Value.Date);
                    }
                    r.showDailyExp(dataGridView1, expenseIDGV, descGV, amountGV, dateGV);
                    MainClass.disable_reset(leftPanel);

                }
                else if (edit == 1) // code for update operation
                {
                    DialogResult dr = MessageBox.Show("Are you sure, you want to update record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        updation u = new updation();
                        u.updateDailyExpenditure(expID, descTxt.Text, amountTxt.Text, dateTimePicker2.Value);
                        r.showDailyExp(dataGridView1, expenseIDGV, descGV, amountGV, dateGV);
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
                    d.delete(expID, "st_deleteExpenditure", "@id");
                    r.showDailyExp(dataGridView1, expenseIDGV, descGV, amountGV, dateGV);
                    MainClass.disable_reset(leftPanel);
                }
            }
        }

        public override void searchTxt_TextChanged(object sender, EventArgs e)
        {
            if (searchTxt.Text != "")
                r.showDailyExp(dataGridView1,expenseIDGV,descGV,amountGV,dateGV, searchTxt.Text);
            else
                r.showDailyExp(dataGridView1, expenseIDGV, descGV, amountGV, dateGV);
        }

        public override void viewBtn_Click(object sender, EventArgs e)
        {
            Expenditure_Ledger el = new Expenditure_Ledger();
            MainClass.show_window(el, this, Form1.ActiveForm);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                edit = 1;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                expID = Convert.ToInt32(row.Cells["expenseIDGV"].Value.ToString());
                descTxt.Text = row.Cells["descGV"].Value.ToString();
                amountTxt.Text = row.Cells["amountGV"].Value.ToString();
                dateTimePicker2.Value = Convert.ToDateTime(row.Cells["dateGV"].Value.ToString());
                MainClass.disable(leftPanel);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);


            for (int x = 1; x < dataGridView1.Columns.Count + 1; x++)
            {
                xlWorkSheet.Cells[1, x] = dataGridView1.Columns[x - 1].HeaderText;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    xlWorkSheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
            }

            var saveFileDialoge = new SaveFileDialog();
            saveFileDialoge.FileName = "ReportView_DailyExpenditure";
            saveFileDialoge.DefaultExt = ".xlsx";
            if (saveFileDialoge.ShowDialog() == DialogResult.OK)
            {
                xlWorkBook.SaveAs(saveFileDialoge.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }

            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
