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
    public partial class stocks : Sample2
    {
        reterival r = new reterival();
        public stocks()
        {
            InitializeComponent();
        }

        private void stocks_Load(object sender, EventArgs e)
        {
            base.addBtn.Enabled = false;
            base.editBtn.Enabled = false;
            base.savebtn.Enabled = false;
            base.delBtn.Enabled = false;
            r.show_stock(dataGridView1, proIDGV, ProductGV, BarcodeGV, ExpiryGV, bpGV, spGV, catGV, qtyGV, StatusGV, finalGV, totalSellAmountGV, packingGV, itemPackQGV);
            StockReportView = r.showStockReport();
            InvestmentTxt.Text = StockReportView[2].ToString();
            ProductsTxt.Text = StockReportView[0].ToString();
            ProfitTxt.Text = StockReportView[3].ToString();
            stockTxt.Text = StockReportView[1].ToString();
            userLabel.Text = reterival.EMP_NAME;
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
                r.show_stock(dataGridView1, proIDGV, ProductGV, BarcodeGV, ExpiryGV, bpGV, spGV, catGV, qtyGV, StatusGV, finalGV, totalSellAmountGV, packingGV, itemPackQGV, searchTxt.Text);
            else
                r.show_stock(dataGridView1, proIDGV, ProductGV, BarcodeGV, ExpiryGV, bpGV, spGV, catGV, qtyGV, StatusGV, finalGV, totalSellAmountGV, packingGV, itemPackQGV);
        }

        private object[] StockReportView = new object[4];
        public override void viewBtn_Click(object sender, EventArgs e)
        {
            r.show_stock(dataGridView1, proIDGV, ProductGV, BarcodeGV, ExpiryGV, bpGV, spGV, catGV, qtyGV, StatusGV, finalGV, totalSellAmountGV, packingGV, itemPackQGV); 
            StockReportView = r.showStockReport();
            InvestmentTxt.Text = StockReportView[2].ToString();
            ProductsTxt.Text = StockReportView[0].ToString();
            ProfitTxt.Text = StockReportView[3].ToString();
            stockTxt.Text = StockReportView[1].ToString();
        }

        private object[] StockReportViewExpireProduct = new object[4];
        private void ExpiredBtn_Click(object sender, EventArgs e)
        {
            r.show_stock_Expired(dataGridView1, proIDGV, ProductGV, BarcodeGV, ExpiryGV, bpGV, spGV, catGV, qtyGV, StatusGV, finalGV, totalSellAmountGV, packingGV, itemPackQGV);
            StockReportViewExpireProduct = r.showStockReportExpireProduct();
            InvestmentTxt.Text = StockReportViewExpireProduct[2].ToString();
            ProductsTxt.Text = StockReportViewExpireProduct[0].ToString();
            ProfitTxt.Text = StockReportViewExpireProduct[3].ToString();
            stockTxt.Text = StockReportViewExpireProduct[1].ToString();
        }

        private object[] StockReportViewAbouttoExpireProduct = new object[4];
        private void AboutToExpireBtn_Click(object sender, EventArgs e)
        {
            r.show_stock_About_to_Expire(dataGridView1, proIDGV, ProductGV, BarcodeGV, ExpiryGV, bpGV, spGV, catGV, qtyGV, StatusGV, finalGV, totalSellAmountGV, packingGV, itemPackQGV);
            StockReportViewAbouttoExpireProduct = r.showStockReportAbouttoExpireProduct();
            InvestmentTxt.Text = StockReportViewAbouttoExpireProduct[2].ToString();
            ProductsTxt.Text = StockReportViewAbouttoExpireProduct[0].ToString();
            ProfitTxt.Text = StockReportViewAbouttoExpireProduct[3].ToString();
            stockTxt.Text = StockReportViewAbouttoExpireProduct[1].ToString();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var now = DateTime.Now;
                var expirationDate = DateTime.Parse(row.Cells[4].Value.ToString());
                var threeMonths = now.AddMonths(3);
                var twoMonths = now.AddMonths(2);
                var oneMonth = now.AddMonths(1);

                if (expirationDate < oneMonth)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
                else if (expirationDate < twoMonths)
                {
                    row.DefaultCellStyle.BackColor = Color.SeaShell;
                }
                else if(expirationDate < threeMonths)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
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
            saveFileDialoge.FileName = "ReportView_Stock";
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
