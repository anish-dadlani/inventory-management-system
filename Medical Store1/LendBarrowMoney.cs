using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;

namespace Medical_Store
{
    public partial class LendBarrowMoney : Sample2
    {
        Int64 saleID, customerID;
        reterival r = new reterival();
        deletion d = new deletion();
        insertion i = new insertion();
        updation u = new updation();
        Regex rg = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

        public LendBarrowMoney()
        {
            InitializeComponent();
        }

        private void LendBarrowMoney_Load(object sender, EventArgs e)
        {
            MainClass.disable_reset(leftPanel);
            base.editBtn.Enabled = false;
            base.delBtn.Enabled = false;
            base.viewBtn.Enabled = false;
            userLabel.Text = reterival.EMP_NAME;
            r.showCustomersMoneyRemaining(dataGridView1, saleIDGV, custIDGV, userGV, totAmtGV, totDisGV, givenGV, payGV, amtRemainGV, customerGV, addressGV, dateGV);
        }

        private void backBtn_Click_1(object sender, EventArgs e)
        {
            Credit_Debit_Balance obj = new Credit_Debit_Balance();
            MainClass.show_window(obj, this, Form1.ActiveForm);
        }

        public override void addBtn_Click(object sender, EventArgs e)
        {
            amountGivenText.Enabled = true;
            amountGivenText.Focus();
        }

        public override void editBtn_Click(object sender, EventArgs e)
        {

        }

        float sum = 0, total = 0;
        public override void savebtn_Click(object sender, EventArgs e)
        {
            if (amountGivenText.Text != "")
            {
                if (Convert.ToSingle(amountGivenText.Text) >= Convert.ToSingle(RemainAmountTxt.Text))
                {
                    sum = Convert.ToSingle(amountGivenText.Text) + Convert.ToSingle(PreviousAmtTxt.Text) - Convert.ToSingle(changeGiveText.Text);
                    total = Convert.ToSingle(amountGivenText.Text) + Convert.ToSingle(PreviousAmtTxt.Text);
                    u.updateSalesEarning(saleID, customerID, total, null, Convert.ToSingle(changeGiveText.Text));
                    customerNameTxt.Text = "";
                    PreviousAmtTxt.Text = "";
                    RemainAmountTxt.Text = "";
                    amountGivenText.Text = "";
                    changeGiveText.Text = "";
                }
                else if (Convert.ToSingle(amountGivenText.Text) < Convert.ToSingle(RemainAmountTxt.Text))
                {
                    sum = Convert.ToSingle(RemainAmountTxt.Text) - Convert.ToSingle(amountGivenText.Text);
                    total = Convert.ToSingle(amountGivenText.Text) + Convert.ToSingle(PreviousAmtTxt.Text);
                    u.updateSalesEarning(saleID, customerID, total, sum, null);
                    RemainAmountTxt.Text = Convert.ToString(sum);
                    amountGivenText.Text = "";
                }                
            }
            MessageBox.Show("Payment Successfull.");
            MainClass.disable_reset(leftPanel);
            r.showCustomersMoneyRemaining(dataGridView1, saleIDGV, custIDGV, userGV, totAmtGV, totDisGV, givenGV, payGV, amtRemainGV, customerGV, addressGV, dateGV);
        }

        public override void delBtn_Click(object sender, EventArgs e)
        {

        }

        public override void searchTxt_TextChanged(object sender, EventArgs e)
        {
            if (searchTxt.Text != "")
                r.showCustomersMoneyRemaining(dataGridView1, saleIDGV, custIDGV, userGV, totAmtGV, totDisGV, givenGV, payGV, amtRemainGV, customerGV, addressGV, dateGV, searchTxt.Text);
            else
                r.showCustomersMoneyRemaining(dataGridView1, saleIDGV, custIDGV, userGV, totAmtGV, totDisGV, givenGV, payGV, amtRemainGV, customerGV, addressGV, dateGV);
        }

        public override void viewBtn_Click(object sender, EventArgs e)
        {

        }

        private void amountGivenText_Validating(object sender, CancelEventArgs e)
        {
            if (amountGivenText.Text != "" && Convert.ToSingle(amountGivenText.Text) >= Convert.ToSingle(RemainAmountTxt.Text))
            {
                float amountGiven = Convert.ToSingle(amountGivenText.Text);
                float amountToReturn = amountGiven - Convert.ToSingle(RemainAmountTxt.Text);
                changeGiveText.Text = Math.Round(amountToReturn, 0).ToString();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                saleID = Convert.ToInt64(row.Cells["saleIDGV"].Value.ToString());
                customerID = Convert.ToInt64(row.Cells["custIDGV"].Value.ToString());
                customerNameTxt.Text = row.Cells["customerGV"].Value.ToString();
                RemainAmountTxt.Text = row.Cells["amtRemainGV"].Value.ToString();
                PreviousAmtTxt.Text = row.Cells["givenGV"].Value.ToString();
                amountGivenText.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Credit_Debit_Balance obj = new Credit_Debit_Balance();
            MainClass.show_window(obj, this, Form1.ActiveForm);
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
                xlWorkSheet.Cells[1, x] = dataGridView1.Columns[x - 1].HeaderText;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    xlWorkSheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
            }

            var saveFileDialoge = new SaveFileDialog();
            saveFileDialoge.FileName = "ReportView_Clients";
            saveFileDialoge.DefaultExt = ".xlsx";
            if (saveFileDialoge.ShowDialog() == DialogResult.OK)
                xlWorkBook.SaveAs(saveFileDialoge.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

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
