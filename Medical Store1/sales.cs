using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Medical_Store
{
    public partial class sales : Sample2
    {
        string[] prodARR = new string[9];
        string[] prodARR2 = new string[9];
        float gross = 0, sellingPrice = 0, sellingPriceUnit = 0;
        string packing_quan;
        object output;
        reterival r = new reterival();
        deletion d = new deletion();
        insertion i = new insertion();
        updation u = new updation();
        Regex rg = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

        public sales()
        {
            InitializeComponent();
        }

        private void sales_Load(object sender, EventArgs e)
        {
            base.addBtn.Enabled = false;
            base.editBtn.Enabled = false;
            base.savebtn.Enabled = false;
            base.delBtn.Enabled = false;
            userLabel.Text = reterival.EMP_NAME;
            r.getList("st_getCustommerList", custDD, "Customer Name", "ID");
        }

        bool productCheck;
        private void barcodeTxt_Validating(object sender, CancelEventArgs e)
        {
            if (barcodeTxt.Text != "")
            {
                GrossTotalTxt.Text = "";
                DiscountTxt.Text = "";
                amountGivenTxt.Text = "";
                changeGiventxt.Text = "";
                RemainingAmtTxt.Text = "";
                int qCount = 0, sQuantity = 0, nCount = 0, itemSQuantity = 0, itemQCount = 0, itemNCount = 0; 
                prodARR = r.getProductsWRBarcode(barcodeTxt.Text);
                object stockQuantity = r.getProductQuantity(Convert.ToInt64(prodARR[0]));
                object itemQuantity = r.getProductPackingQuantity(Convert.ToInt64(prodARR[0]));
                sellingPrice = Convert.ToSingle(prodARR[3]);

                packing_quan = prodARR[6].ToString();
                output = Regex.Replace(packing_quan, "[^0-9]+", string.Empty);
                //object output_String = Regex.Replace(packing_quan, "[^a-zA-Z]", "");
                //string output_str = Convert.ToString(output_String);

                if (Convert.ToInt32(stockQuantity) == 0 && Convert.ToInt64(itemQuantity) == 0)
                {
                    u.updateStock(Convert.ToInt64(prodARR[0]),0,0);
                    MessageBox.Show("Stock Not Available.");
                    barcodeTxt.Text = "";
                    barcodeTxt.Focus();
                }
                else if (prodARR[0] == null)
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
                    packing_quan = prodARR[6].ToString();
                    output = Regex.Replace(packing_quan, "[^0-9]+", string.Empty);
                    object output_String = Regex.Replace(packing_quan, "[^a-zA-Z]", "");
                    string output_str = Convert.ToString(output_String);

                    //if (Convert.ToInt32(itemQuantity) < Convert.ToInt64(output))
                    //{
 
                    //}
                    //else
                    //{
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (prodARR[0] == row.Cells["proIDGV"].Value.ToString())
                                qCount = qCount + Convert.ToInt32(row.Cells["quantityGV"].Value.ToString());
                        }

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (prodARR[0] == row.Cells["proIDGV"].Value.ToString())
                                itemQCount = itemQCount + Convert.ToInt32(row.Cells["noItemGV"].Value.ToString());
                        }
                    //}

                    sQuantity = Convert.ToInt32(r.getProductQuantity(Convert.ToInt64(prodARR[0])));
                    nCount = sQuantity - qCount;
                    if ((output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                    {

                    }
                    else
                    {
                        itemSQuantity = Convert.ToInt32(r.getProductPackingQuantity(Convert.ToInt64(prodARR[0])));
                        itemNCount = itemSQuantity - itemQCount;
                    }

                    if (nCount <= 0 && itemNCount <=0)
                        MessageBox.Show("Stock not Available.");
                    else
                    {
                        if (dataGridView1.RowCount == 0)
                            dataGridView1.Rows.Add(Convert.ToInt32(prodARR[0]), prodARR[1], 1, prodARR[6], 0, Convert.ToSingle(prodARR[3]), Math.Round(Convert.ToSingle(prodARR[4]), 2), Convert.ToSingle(prodARR[3]));
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
                                        if (row.Cells["quantityGV"].Value.ToString() != null)
                                        {
                                            row.Cells["quantityGV"].Value.ToString();
                                        }
                                        if (row.Cells["discGV"].Value.ToString() != null)
                                        {
                                            disc = Convert.ToSingle(prodARR[4]) * Convert.ToSingle(row.Cells["quantityGV"].Value.ToString());
                                            row.Cells["discGV"].Value = Math.Ceiling(disc);
                                        }
                                        float tot = (Convert.ToSingle(row.Cells["pupGV"].Value.ToString()) * Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()));
                                        row.Cells["totalGV"].Value = Math.Ceiling(tot);
                                    }
                                }
                            }
                            else
                                dataGridView1.Rows.Add(Convert.ToInt32(prodARR[0]), prodARR[1], 1, prodARR[6], 0, Convert.ToSingle(prodARR[3]), Math.Round(Convert.ToSingle(prodARR[4]), 2), Convert.ToSingle(prodARR[3]));
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
                GrossTotalTxt.Text = Math.Ceiling(gross).ToString();
                DiscountTxt.Text = Math.Ceiling(dis).ToString();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.ColumnIndex == 8)
                {
                    GrossTotalTxt.Text = "";
                    DiscountTxt.Text = "";
                    amountGivenTxt.Text = "";
                    changeGiventxt.Text = "";
                    RemainingAmtTxt.Text = "";
                    float gt, tot, disc;
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    int q = Convert.ToInt32(row.Cells["quantityGV"].Value.ToString());

                    if (q == 1)
                    {
                        gt = Convert.ToSingle(grosAmtLabel.Text);
                        gt = gt - Convert.ToSingle(row.Cells["totalGV"].Value.ToString());
                        grosAmtLabel.Text = gt.ToString();
                        dataGridView1.Rows.Remove(row);
                    }
                    else if (q > 1)
                    {
                        q--;
                        row.Cells["quantityGV"].Value = q;
                        //disc = Convert.ToSingle(row.Cells["discGV"].Value.ToString()) - Convert.ToSingle(prodARR[4]);
                        disc = Convert.ToSingle(row.Cells["discGV"].Value.ToString());
                        row.Cells["discGV"].Value = disc;
                        tot = Convert.ToSingle(row.Cells["quantityGV"].Value) * Convert.ToSingle(row.Cells["pupGV"].Value.ToString()); //- disc;
                        row.Cells["totalGV"].Value = tot;
                        foreach (DataGridViewRow item in dataGridView1.Rows)
                        {
                            gross += Convert.ToSingle(item.Cells["totalGV"].Value.ToString());
                        }
                        grosAmtLabel.Text = gross.ToString();
                        gross = 0;
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
                GrossTotalTxt.Text = Math.Ceiling(gross).ToString();
                DiscountTxt.Text = Math.Ceiling(dis).ToString();
            }
        }

        private void checkBtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                float dis = 0, gross = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    dis += Convert.ToSingle(row.Cells["discGV"].Value.ToString());
                    gross += Convert.ToSingle(row.Cells["totalGV"].Value.ToString());
                }
                GrossTotalTxt.Text = Math.Ceiling(gross).ToString();
                DiscountTxt.Text = Math.Ceiling(dis).ToString();
            }
        }

        private void amountGivenTxt_TextChanged(object sender, EventArgs e)
        {
            if (amountGivenTxt.Text != "")
            {
                if (!rg.Match(amountGivenTxt.Text).Success)
                {
                    amountGivenTxt.Text = "";
                    amountGivenTxt.Focus();
                }
            }
            else
            {
                RemainingAmtTxt.Text = "";
                changeGiventxt.Text = "";
            }
        }

        private void amountGivenTxt_Validating(object sender, CancelEventArgs e)
        {
            if (amountGivenTxt.Text != "" && GrossTotalTxt.Text != "")
            {
                if (!(Convert.ToSingle(GrossTotalTxt.Text) <= Convert.ToSingle(amountGivenTxt.Text)))
                {
                    amountGivenTxt.Text = amountGivenTxt.Text;
                    changeGiventxt.Text = "";
                    RemainingAmtTxt.Text = Convert.ToString(Convert.ToSingle(GrossTotalTxt.Text) - Convert.ToSingle(amountGivenTxt.Text));
                    custDD.Enabled = true;
                }
                else
                {
                    float amountGiven = Convert.ToSingle(amountGivenTxt.Text);
                    float amountToReturn = amountGiven - Convert.ToSingle(GrossTotalTxt.Text);
                    changeGiventxt.Text = Math.Round(amountToReturn, 0).ToString();
                    RemainingAmtTxt.Text="";
                    custDD.Enabled = false;
                }
            }
        }

        private void payBtn_Click(object sender, EventArgs e)
        {
            if (custDD.Enabled == true && (custDD.SelectedValue.ToString() == "-1" || custDD.SelectedValue.ToString() == "0"))
                MessageBox.Show("Please enter Customer Name. Amount Remaing to the Customer.");
            else
            {
                if (amountGivenTxt.Text != "" && DiscountTxt.Text != "" && GrossTotalTxt.Text != "" && PaymentDD.SelectedIndex != -1 && (changeGiventxt.Text != "" || RemainingAmtTxt.Text != ""))
                {
                    DialogResult dr = MessageBox.Show("\n\tTotal Amount: " + GrossTotalTxt.Text + "\n\tTotal Discount: " + DiscountTxt.Text + "\n\tAmount Given: " + amountGivenTxt.Text + "\n\tChange Given: " + changeGiventxt.Text + "\n\nAre you sure, you want to submit current sales?",
                    "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {

                        if (RemainingAmtTxt.Text == "")
                            i.insertSales(dataGridView1, "proIDGV", "quantityGV", "pupGV", "discGV", reterival.USER_ID, DateTime.Now, Convert.ToSingle(GrossTotalTxt.Text), Convert.ToSingle(DiscountTxt.Text), Convert.ToSingle(amountGivenTxt.Text), PaymentDD.SelectedItem.ToString(), "packGV", "noItemGV", Convert.ToSingle(changeGiventxt.Text), null, null);
                        else
                            i.insertSales(dataGridView1, "proIDGV", "quantityGV", "pupGV", "discGV", reterival.USER_ID, DateTime.Now, Convert.ToSingle(GrossTotalTxt.Text), Convert.ToSingle(DiscountTxt.Text), Convert.ToSingle(amountGivenTxt.Text), PaymentDD.SelectedItem.ToString(), "packGV", "noItemGV", null, Convert.ToSingle(RemainingAmtTxt.Text), Convert.ToInt64(custDD.SelectedValue));
                        
                        object incomeAmountCheck = r.getIncomeAmountDate(DateTime.Today);

                        if (incomeAmountCheck.GetType() == typeof(DBNull))
                        {
                            i.insertDailyIncome(Convert.ToSingle(GrossTotalTxt.Text), DateTime.Today);
                        }
                        else
                        {
                            object totalIncomeAmount;
                            float updatedIncomeAmount;
                            totalIncomeAmount = r.getIncomeAmountDate(DateTime.Today);
                            updatedIncomeAmount = Convert.ToSingle(totalIncomeAmount) + Convert.ToSingle(GrossTotalTxt.Text);
                            u.updateDailyIncome(updatedIncomeAmount, DateTime.Today);
                        }

                        MainClass.enable_reset(groupBox2);
                        dataGridView1.Rows.Clear();
                        grosAmtLabel.Text = "0.00";

                        salesReport sr = new salesReport();
                        sr.Show();
                    }
                }
            }
         }

        public override void viewBtn_Click(object sender, EventArgs e)
        {
            SalesDetails sd = new SalesDetails();
            MainClass.show_window(sd, this, Form1.ActiveForm);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                //check for logical error
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                prodARR2 = r.getProductsBYWRTID(Convert.ToInt64(row.Cells["proIDGV"].Value));

                string packing = row.Cells["packGV"].Value.ToString();
                object output = Regex.Replace(packing, "[^0-9]+", string.Empty);
                object output_String = Regex.Replace(packing, "[^a-zA-Z]", "");
                string output_str = Convert.ToString(output_String);

                float itemsP = Convert.ToSingle(prodARR2[3]);
                float sp = 0;

                if ((Convert.ToInt32(row.Cells["noItemGV"].Value) >= 1) && (output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                {
                    MessageBox.Show("Action can not be performed.");
                    row.Cells["noItemGV"].Value = 0;
                }
                else
                {
                    if (row.Cells["noItemGV"].Value != null && rg.Match(row.Cells["noItemGV"].Value.ToString()).Success && Convert.ToInt32(row.Cells["noItemGV"].Value) >= 1)// && Convert.ToInt32(row.Cells["quantityGV"].Value) == 1)
                    {
                        //itemsP = sellingPrice;
                        packing_quan = row.Cells["packGV"].Value.ToString();
                        output = Regex.Replace(packing_quan, "[^0-9]+", string.Empty);
                        row.Cells["pupGV"].Value = Math.Ceiling(itemsP / Convert.ToSingle(output));
                        row.Cells["quantityGV"].Value = 1;
                        sp = Convert.ToSingle(row.Cells["pupGV"].Value);
                        sp = sp - (sp * (Convert.ToSingle(row.Cells["discGV"].Value.ToString()) / 100));
                        row.Cells["pupGV"].Value = Math.Ceiling(sp);
                        row.Cells["totalGV"].Value = Math.Ceiling(sp * Convert.ToSingle(row.Cells["noItemGV"].Value));
                        gross = 0;
                        if (row.Cells["discGV"].Value != null && rg.Match(row.Cells["discGV"].Value.ToString()).Success)
                        {
                            sp = sp - (sp * (Convert.ToSingle(row.Cells["discGV"].Value.ToString()) / 100));
                            row.Cells["pupGV"].Value = Math.Ceiling(sp);
                            //sellingPriceUnit = sp / Convert.ToSingle(output);
                            row.Cells["totalGV"].Value = Math.Ceiling(sp * Convert.ToSingle(row.Cells["noItemGV"].Value));
                            gross = 0;
                        }
                    }
                    else if (Convert.ToInt32(row.Cells["quantityGV"].Value) > 1 && rg.Match(row.Cells["quantityGV"].Value.ToString()).Success)// && Convert.ToSingle(row.Cells["noItemGV"].Value.ToString()) == 0)
                    {
                        if (row.Cells["discGV"].Value != null && rg.Match(row.Cells["discGV"].Value.ToString()).Success)
                        {
                            row.Cells["noItemGV"].Value = 0;
                            sp = itemsP;
                            sp = sp - (sp * (Convert.ToSingle(row.Cells["discGV"].Value.ToString()) / 100));
                            row.Cells["pupGV"].Value = Math.Ceiling(sp);
                            row.Cells["totalGV"].Value = Math.Ceiling(sp * Convert.ToSingle(row.Cells["quantityGV"].Value));
                            gross = 0;
                        }
                    }
                    else if (Convert.ToInt32(row.Cells["quantityGV"].Value) == 1 && rg.Match(row.Cells["quantityGV"].Value.ToString()).Success)// && Convert.ToSingle(row.Cells["noItemGV"].Value.ToString()) == 0)
                    {
                        if (row.Cells["discGV"].Value != null && rg.Match(row.Cells["discGV"].Value.ToString()).Success)
                        {
                            row.Cells["noItemGV"].Value = 0;
                            sp = itemsP;
                            sp = sp - (sp * (Convert.ToSingle(row.Cells["discGV"].Value.ToString()) / 100));
                            row.Cells["pupGV"].Value = Math.Ceiling(sp);
                            row.Cells["totalGV"].Value = Math.Ceiling(sp * Convert.ToSingle(row.Cells["quantityGV"].Value));
                            gross = 0;
                        }
                    }

                }
                
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    gross += Convert.ToSingle(item.Cells["totalGV"].Value.ToString());
                }
                grosAmtLabel.Text = Math.Ceiling(gross).ToString();
                gross = 0;
            }

            if (dataGridView1.Rows.Count > 0)
            {
                float dis = 0, gross = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    dis += Convert.ToSingle(row.Cells["discGV"].Value.ToString());
                    gross += Convert.ToSingle(row.Cells["totalGV"].Value.ToString());
                }
                GrossTotalTxt.Text = Math.Ceiling(gross).ToString();
                DiscountTxt.Text = Math.Ceiling(dis).ToString();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                //check for logical error
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (Convert.ToInt32(row.Cells["quantityGV"].Value) > 1 && Convert.ToInt32(row.Cells["noItemGV"].Value) > 0)
                {
                    row.Cells["noItemGV"].Value = 0;
                }
                //else
                //{
                //    row.Cells["quantityGV"].Value = 1;
                //}
            }
        }
    }
}
