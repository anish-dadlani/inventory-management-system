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
using CrystalDecisions.CrystalReports.Engine;

namespace Medical_Store
{
    public partial class SalesDetails : Sample2
    {
        reterival r = new reterival();
        deletion d = new deletion();
        insertion i = new insertion();
        updation u = new updation();
        Regex rg = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
        private object[] EarningTotalView = new object[2];
        private object[] EarningTodayView = new object[2];
        public SalesDetails()
        {
            InitializeComponent();
        }

        private void SalesDetails_Load(object sender, EventArgs e)
        {
            base.addBtn.Enabled = false;
            base.editBtn.Enabled = false;
            base.savebtn.Enabled = false;
            base.delBtn.Enabled = false;
            base.viewBtn.Enabled = false;
            userLabel.Text = reterival.EMP_NAME;
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            string RemMinusTotal, RemMinuToday;
            r.showSalesInvoices(dateTimePicker1.Value, dataGridView1, saleIDGV, userGV, totAmtGV, totDisGV, givenGV, returnGV, payGV, dateGV, userIDgv);
            EarningTotalView = r.showEarningTotal();
            EarningTodayView = r.showEarningToday(dateTimePicker1.Value);
            if (Convert.ToString(EarningTodayView[1]) == "")
            {
                RemMinuToday = EarningTodayView[0].ToString();
            }
            else
                RemMinuToday = Convert.ToString(Convert.ToSingle(EarningTodayView[0].ToString()) - Convert.ToSingle(EarningTodayView[1].ToString()));
            if (Convert.ToString(EarningTotalView[1]) == "")
                RemMinusTotal = EarningTotalView[0].ToString();
            else
                RemMinusTotal = Convert.ToString(Convert.ToSingle(EarningTotalView[0].ToString()) - Convert.ToSingle(EarningTotalView[1].ToString()));

            earningTxt.Text = RemMinusTotal.ToString();
            remaTotalTxt.Text = EarningTotalView[1].ToString();
            earnTodayTxt.Text = RemMinuToday.ToString();
            remSelectedTxt.Text = EarningTodayView[1].ToString();
        }

        private void backBtn_Click_1(object sender, EventArgs e)
        {
            sales obj = new sales();
            MainClass.show_window(obj, this, Form1.ActiveForm);
        }

        public override void addBtn_Click(object sender, EventArgs e)
        {

        }

        public override void editBtn_Click(object sender, EventArgs e)
        {

        }

        public override void savebtn_Click(object sender, EventArgs e)
        {

        }

        public override void delBtn_Click(object sender, EventArgs e)
        {

        }

        public override void searchTxt_TextChanged(object sender, EventArgs e)
        {
            if (searchTxt.Text != "")
            {
                r.showSalesInvoices(dateTimePicker1.Value, dataGridView1, saleIDGV, userGV, totAmtGV, totDisGV, givenGV, returnGV, payGV, dateGV, userIDgv, searchTxt.Text);
            }
            else
            {
                r.showSalesInvoices(dateTimePicker1.Value, dataGridView1, saleIDGV, userGV, totAmtGV, totDisGV, givenGV, returnGV, payGV, dateGV, userIDgv);
            }
        }

        public override void viewBtn_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LendBarrowMoney obj = new LendBarrowMoney();
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
            saveFileDialoge.FileName = "ReportView_SalesDetails";
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

        public static int saleID = 0;
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex !=-1 && e.ColumnIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                saleID = Convert.ToInt32(row.Cells["saleIDGV"].Value.ToString());
                salesReport sr = new salesReport();
                sr.Show();
            }
        }
    }
}
