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
    public partial class Products_2 : Sample2
    {
        int edit = 0; //this 0 is an indication for save operation and 1 is an indication for update operation
        Int64 productID;
        reterival r = new reterival();

        public Products_2()
        {
            InitializeComponent();
        }

        private void Products_2_Load(object sender, EventArgs e)
        {
            MainClass.disable_reset(leftPanel);
            r.getList("st_getCategoriesDataList", catDD, "Category", "ID");
            r.show_products(dataGridView1, proIDGV, ProductGV, PackGV, ExpiryGV, CategoryGV, BarcodeGV, CatIDGV);
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
            if (proTxt.Text == "")
                proNameError.Visible = true;
            else
                proNameError.Visible = false;

            if (barcodeTxt.Text == "")
                barcdError.Visible = true;
            else
                barcdError.Visible = false;

            if (packingTxt.Text == "")
                packingError.Visible = true;
            else
                packingError.Visible = false;

            if (expirypicker.Value < DateTime.Now)
            {
                dateError.Visible = true;
                MainClass.show_msg("Invalid Date.", "Stop", "Error");
            }
            else
                dateError.Visible = false;

            if (expirypicker.Value.Date == DateTime.Now.Date)
                dateError.Visible = false;

            if (catDD.SelectedIndex == -1 || catDD.SelectedIndex == 0)
                catError.Visible = true;
            else
                catError.Visible = false;

            if (proNameError.Visible || barcdError.Visible || catError.Visible || packingError.Visible)
            {
                MainClass.show_msg("Fields with * are mandatory", "Stop", "Error"); //Error is the type of msg
            }
            else
            {
                if (edit == 0) // code for save operation
                {
                    insertion i = new insertion();
                    if (expirypicker.Value.Date == DateTime.Now.Date)
                    {
                        i.insertProduct(proTxt.Text, barcodeTxt.Text, Convert.ToInt32(catDD.SelectedValue), packingTxt.Text);
                        r.show_products(dataGridView1, proIDGV, ProductGV, PackGV, ExpiryGV, CategoryGV, BarcodeGV, CatIDGV);
                        MainClass.disable_reset(leftPanel);
                    }
                    else
                    {
                        i.insertProduct(proTxt.Text, barcodeTxt.Text, Convert.ToInt32(catDD.SelectedValue), packingTxt.Text, expirypicker.Value);
                        r.show_products(dataGridView1, proIDGV, ProductGV, PackGV, ExpiryGV, CategoryGV, BarcodeGV, CatIDGV);
                        MainClass.disable_reset(leftPanel);
                    }
                }
                else if (edit == 1) // code for update operation
                {
                    DialogResult dr = MessageBox.Show("Are you sure, you want to update record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        updation u = new updation();
                        if (expirypicker.Value.Date == DateTime.Now.Date)
                        {
                            u.updateProduct(productID, proTxt.Text, barcodeTxt.Text, Convert.ToInt32(catDD.SelectedValue), packingTxt.Text);
                            r.show_products(dataGridView1, proIDGV, ProductGV, PackGV, ExpiryGV, CategoryGV, BarcodeGV, CatIDGV);
                            MainClass.disable_reset(leftPanel);
                        }
                        else
                        {
                            u.updateProduct(productID, proTxt.Text, barcodeTxt.Text, Convert.ToInt32(catDD.SelectedValue), packingTxt.Text, expirypicker.Value);
                            r.show_products(dataGridView1, proIDGV, ProductGV, PackGV, ExpiryGV, CategoryGV, BarcodeGV, CatIDGV);
                            MainClass.disable_reset(leftPanel);
                        }
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
                    d.delete(productID, "st_deleteProduct", "@id");
                    MainClass.disable_reset(leftPanel);
                    r.show_products(dataGridView1, proIDGV, ProductGV, PackGV, ExpiryGV, CategoryGV, BarcodeGV, CatIDGV);
                }
            }
        }

        public override void searchTxt_TextChanged(object sender, EventArgs e)
        {
            if (searchTxt.Text != "")
                r.show_products(dataGridView1, proIDGV, ProductGV, PackGV, ExpiryGV, CategoryGV, BarcodeGV, CatIDGV, searchTxt.Text);
            else
                r.show_products(dataGridView1, proIDGV, ProductGV, PackGV, ExpiryGV, CategoryGV, BarcodeGV, CatIDGV);
        }

        public override void viewBtn_Click(object sender, EventArgs e)
        {
            r.show_products(dataGridView1, proIDGV, ProductGV, PackGV, ExpiryGV, CategoryGV, BarcodeGV, CatIDGV);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                edit = 1;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                productID = Convert.ToInt32(row.Cells["proIDGV"].Value.ToString());
                proTxt.Text = row.Cells["ProductGV"].Value.ToString();
                barcodeTxt.Text = row.Cells["BarcodeGV"].Value.ToString();
                if (row.Cells["ExpiryGV"].FormattedValue.ToString() == "")
                    expirypicker.Value = DateTime.Now;
                else
                    expirypicker.Value = Convert.ToDateTime(row.Cells["ExpiryGV"].Value.ToString());
                catDD.SelectedValue = row.Cells["CatIDGV"].Value.ToString();
                packingTxt.Text = row.Cells["PackGV"].Value.ToString();
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
            saveFileDialoge.FileName = "ReportView_Products";
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
