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

namespace Medical_Store
{
    public partial class Credit_Debit_Balance : Sample2
    {
        reterival r = new reterival();
        updation u = new updation();
        Int64 purchaseID, supplierID;
        float sum = 0, total = 0;

        public Credit_Debit_Balance()
        {
            InitializeComponent();
        }

        private void Credit_Debit_Balance_Load(object sender, EventArgs e)
        {
            userLabel.Text = reterival.EMP_NAME;
            base.editBtn.Enabled = false;
            base.delBtn.Enabled = false;
            r.showSupplierCDB(dataGridView1, purIDGV, SuppIDGV, totAmtGV, supplierGV, addressGV, givenGV, amtRemainGV, dateGV);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LendBarrowMoney obj = new LendBarrowMoney();
            MainClass.show_window(obj, this, Form1.ActiveForm);
        }

        public override void addBtn_Click(object sender, EventArgs e)
        {
            MainClass.enable(leftPanel);
            suppTxt.Enabled = false;
            totalAmtTxt.Enabled = false;
            GivenAmtTxt.Enabled = false;
            remainAmtTxt.Enabled = false;
            amountGiveTxt.Focus();
        }

        public override void editBtn_Click(object sender, EventArgs e)
        {

        }

        public override void savebtn_Click(object sender, EventArgs e)
        {
            if (amountGiveTxt.Text != "")
            {
                if (Convert.ToSingle(amountGiveTxt.Text) > Convert.ToSingle(remainAmtTxt.Text))
                {
                    MessageBox.Show("Remaining amount is less then the amount given.");
                    amountGiveTxt.Text = "";
                    amountGiveTxt.Focus();
                    sum = 0;
                }
                else if (Convert.ToSingle(amountGiveTxt.Text) == Convert.ToSingle(remainAmtTxt.Text))
                {
                    sum = Convert.ToSingle(amountGiveTxt.Text) + Convert.ToSingle(GivenAmtTxt.Text);
                    u.updatePurchaseSuppCDB(purchaseID, sum, null);
                    suppTxt.Text = "";
                    amountGiveTxt.Text = "";
                    remainAmtTxt.Text = "";
                    GivenAmtTxt.Text = "";
                    totalAmtTxt.Text = "";
                    sum = 0;
                }
                else if (Convert.ToSingle(amountGiveTxt.Text) < Convert.ToSingle(remainAmtTxt.Text))
                {
                    sum = Convert.ToSingle(remainAmtTxt.Text) - Convert.ToSingle(amountGiveTxt.Text);
                    total = Convert.ToSingle(amountGiveTxt.Text) + Convert.ToSingle(GivenAmtTxt.Text);
                    u.updatePurchaseSuppCDB(purchaseID, total, sum);
                    suppTxt.Text = "";
                    amountGiveTxt.Text = "";
                    remainAmtTxt.Text = "";
                    GivenAmtTxt.Text = "";
                    totalAmtTxt.Text = "";
                    sum = 0;
                }
            }
            MessageBox.Show("Payment Successfull.");
            MainClass.disable_reset(leftPanel);
            r.showSupplierCDB(dataGridView1, purIDGV, SuppIDGV, totAmtGV, supplierGV, addressGV, givenGV, amtRemainGV, dateGV);
        }

        public override void delBtn_Click(object sender, EventArgs e)
        {

        }

        public override void searchTxt_TextChanged(object sender, EventArgs e)
        {
            if (searchTxt.Text != "")
                r.showSupplierCDB(dataGridView1, purIDGV, SuppIDGV, totAmtGV, supplierGV, addressGV, givenGV, amtRemainGV, dateGV, searchTxt.Text);
            else
                r.showSupplierCDB(dataGridView1, purIDGV, SuppIDGV, totAmtGV, supplierGV, addressGV, givenGV, amtRemainGV, dateGV);
        }

        public override void viewBtn_Click(object sender, EventArgs e)
        {
            r.showSupplierCDB(dataGridView1, purIDGV, SuppIDGV, totAmtGV, supplierGV, addressGV, givenGV, amtRemainGV, dateGV);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            r.showSupplierCDB(dataGridView1, purIDGV, SuppIDGV, totAmtGV, supplierGV, addressGV, givenGV, amtRemainGV, dateGV);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                purchaseID = Convert.ToInt64(row.Cells["purIDGV"].Value.ToString());
                supplierID =  Convert.ToInt64(row.Cells["SuppIDGV"].Value.ToString());
                suppTxt.Text = row.Cells["supplierGV"].Value.ToString();
                totalAmtTxt.Text = row.Cells["totAmtGV"].Value.ToString();
                GivenAmtTxt.Text = row.Cells["givenGV"].Value.ToString();
                remainAmtTxt.Text = row.Cells["amtRemainGV"].Value.ToString();
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
            saveFileDialoge.FileName = "ReportView_Credit-Debit";
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
