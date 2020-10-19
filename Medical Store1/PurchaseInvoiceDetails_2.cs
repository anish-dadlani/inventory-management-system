using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Transactions;

namespace Medical_Store
{
    public partial class PurchaseInvoiceDetails_2 : Sample2
    {
        reterival r = new reterival();
        insertion i = new insertion();
        updation u = new updation();
        deletion d = new deletion();

        public PurchaseInvoiceDetails_2()
        {
            InitializeComponent();
        }

        private void PurchaseInvoiceDetails_2_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            r.getListWithTwoParam("st_getPurchaseInvoiceList", PurchaseInvoiceDD, "Company", "ID", "@month", invoiceDatePicker.Value.Month, "@year", invoiceDatePicker.Value.Year);
            userLabel.Text = reterival.EMP_NAME;
            base.editBtn.Enabled = false;
            base.delBtn.Enabled = false;
            base.addBtn.Enabled = false;
            base.viewBtn.Enabled = false;
            base.savebtn.Enabled = false;
        }

        private void invoiceDatePicker_ValueChanged(object sender, EventArgs e)
        {
            r.getListWithTwoParam("st_getPurchaseInvoiceList", PurchaseInvoiceDD, "Company", "ID", "@month", invoiceDatePicker.Value.Month, "@year", invoiceDatePicker.Value.Year);
        }

        public override void backBtn_Click(object sender, EventArgs e)
        {
            PurchaseInvoice_2 obj = new PurchaseInvoice_2();
            MainClass.show_window(obj, this, Form1.ActiveForm);
        }

        private void PurchaseInvoiceDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PurchaseInvoiceDD.SelectedIndex != -1 && PurchaseInvoiceDD.SelectedIndex != 0)
            {
                float gt = 0;
                r.show_purchase_invoice_details(Convert.ToInt64(PurchaseInvoiceDD.SelectedValue.ToString()), dataGridView1, mPIDGV, proIDGV, ProductGV, quantityGV, pupGV, totalGV);
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    gt = gt + Convert.ToSingle(row.Cells["totalGV"].Value.ToString());
                }
                grosAmtLabel.Text = gt.ToString();
                gt = 0;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int q;
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.ColumnIndex == 6)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    DialogResult dr = MessageBox.Show("Are you sure, you want to delete " + row.Cells["ProductGV"].Value.ToString() + "from Purchase invoice?\n\t\t\tWARNING\nDELETION OF PRODUCT WILL EFFECT STOCK.", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        using (TransactionScope sc = new TransactionScope())
                        {
                            i.insertDeletedItem(Convert.ToInt64(PurchaseInvoiceDD.SelectedValue.ToString()), Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()), Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()), reterival.USER_ID, DateTime.Today);
                            object ob = r.getProductQuantity(Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()));
                            if (ob != null)
                            {
                                q = Convert.ToInt32(ob);
                                q = q - Convert.ToInt32(row.Cells["quantityGV"].Value.ToString());
                                u.updateStock(Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()), q);
                                float tot = Convert.ToSingle(grosAmtLabel.Text) - Convert.ToSingle(row.Cells["totalGV"].Value.ToString());
                                grosAmtLabel.Text = tot.ToString();
                                d.delete(Convert.ToInt64(row.Cells["mPIDGV"].Value.ToString()), "st_deleteProductFromPID", "@mPID");
                                dataGridView1.Rows.Remove(row);
                            }
                            sc.Complete();
                        }
                    }
                }
            }
        }
    }
}
