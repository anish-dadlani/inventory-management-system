using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Medical_Store
{
    public partial class Retail_Sales : Sample2
    {
        string[] prodARR = new string[9];
        float gross = 0;
        bool productCheck;
        reterival r = new reterival();
        deletion d = new deletion();
        insertion i = new insertion();
        updation u = new updation();
        Regex rg = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

        public Retail_Sales()
        {
            InitializeComponent();
        }

        private void Retail_Sales_Load(object sender, EventArgs e)
        {
            base.addBtn.Enabled = false;
            base.editBtn.Enabled = false;
            base.savebtn.Enabled = false;
            base.delBtn.Enabled = false;
            userLabel.Text = reterival.EMP_NAME;
            r.getList("st_getCustommerList", custDD, "Customer Name", "ID");
        }

        private void barcodeTxt_Validating(object sender, CancelEventArgs e)
        {
            if (barcodeTxt.Text != "")
            {
                GrossTotalTxt.Text = "";
                DiscountTxt.Text = "";
                amountGivenTxt.Text = "";
                changeGiventxt.Text = "";
                RemainingAmtTxt.Text = "";
                int qCount = 0, sQuantity = 0, nCount = 0;
                prodARR = r.getProductsWRBarcode(barcodeTxt.Text);

                if (prodARR[0] == null)
                {
                    MessageBox.Show("Product with given Barcode not Available");
                    barcodeTxt.Text = "";
                    barcodeTxt.Focus();
                }
                else if (prodARR[3] == "")
                {
                    MessageBox.Show("You have not set the selling price for this product.\nSet it in the Product Pricing Option.");
                    barcodeTxt.Text = "";
                    barcodeTxt.Focus();
                }
                else if (prodARR[7] != "" && Convert.ToDateTime(prodARR[7]) < DateTime.Now)
                {
                    MessageBox.Show("This Product is Expired.");
                    barcodeTxt.Text = "";
                    barcodeTxt.Focus();
                }
                else
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (prodARR[0] == row.Cells["proIDGV"].Value.ToString())
                            qCount = qCount + Convert.ToInt32(row.Cells["quantityGV"].Value.ToString());
                    }
                    sQuantity = Convert.ToInt32(r.getProductPackingQuantity(Convert.ToInt64(prodARR[0])));
                    nCount = sQuantity - qCount;
                    if (nCount <= 0)
                        MessageBox.Show("Stock not Available.");
                    else
                    {
                        if (dataGridView1.RowCount == 0)
                            dataGridView1.Rows.Add(Convert.ToInt32(prodARR[0]), prodARR[1], 1, prodARR[6], Convert.ToSingle(prodARR[3]), Math.Round(Convert.ToSingle(prodARR[4]), 2), Convert.ToSingle(prodARR[3]), Convert.ToSingle(prodARR[8]));
                        else
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (row.Cells["proIDGV"].Value.ToString() == prodARR[0])
                                {
                                    productCheck = true;
                                    break;
                                }
                                else
                                    productCheck = false;
                            }
                            if (productCheck == true)
                            {
                                foreach (DataGridViewRow row in dataGridView1.Rows)
                                {
                                    if (row.Cells["proIDGV"].Value.ToString() == prodARR[0])
                                    {
                                        float disc = 0;
                                        //row.Cells["quantityGV"].Value = Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()) + 1;
                                        if (row.Cells["discGV"].Value.ToString() != null)
                                        {
                                            disc = Convert.ToSingle(prodARR[8]) * Convert.ToSingle(row.Cells["quantityGV"].Value.ToString());
                                            row.Cells["discGV"].Value = Math.Round(disc, 2);
                                        }
                                        float tot = (Convert.ToSingle(row.Cells["itemSPGV"].Value.ToString()) * Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()));
                                        row.Cells["totalGV"].Value = tot;
                                    }
                                }
                            }
                            else
                                dataGridView1.Rows.Add(Convert.ToInt32(prodARR[0]), prodARR[1], 1, prodARR[6], Convert.ToSingle(prodARR[3]), Math.Round(Convert.ToSingle(prodARR[4]), 2), Convert.ToSingle(prodARR[3]), Convert.ToInt32(prodARR[8]));
                        }
                        foreach (DataGridViewRow item in dataGridView1.Rows)
                        {
                            gross += Convert.ToSingle(item.Cells["totalGV"].Value.ToString());
                        }

                        grosAmtLabel.Text = Math.Round(gross, 2).ToString(); // convert into celing
                        gross = 0;
                        barcodeTxt.Focus();
                        barcodeTxt.Text = "";
                    }
                }
            }
            if (dataGridView1.Rows.Count > 0)
            {
                float dis = 0, gross = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    dis += Convert.ToSingle(row.Cells["discGV"].Value.ToString());
                    gross += Convert.ToSingle(row.Cells["totalGV"].Value.ToString());
                }
                GrossTotalTxt.Text = Math.Round(gross, 0).ToString();
                DiscountTxt.Text = Math.Round(dis, 0).ToString();
            }
        }
    }
}
