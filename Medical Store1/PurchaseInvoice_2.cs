using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Transactions;

namespace Medical_Store
{
    public partial class PurchaseInvoice_2 : Sample2
    {
        reterival r = new reterival();
        insertion i = new insertion();
        updation u = new updation();
        Int64 productID;
        int itemMulQuan = 1;
        float gt, gross = 0;
        string packing_quan;
        string[] prodARR = new string[4];
        object output;
        Regex rg = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
        Regex reg = new Regex(@"^-?[0-9][0-9,\.]+$");

        public PurchaseInvoice_2()
        {
            InitializeComponent();
        }

        private void PurchaseInvoice_2_Load(object sender, EventArgs e)
        {
            MainClass.disable_reset(leftPanel);
            r.getList("st_getSupplierList", supDD, "Company", "ID");
            userLabel.Text = reterival.EMP_NAME;
            base.editBtn.Enabled = false;
            base.delBtn.Enabled = false;
            productTxt.Enabled = false;
            totAmountTxt.Enabled = false;
            remainAmtTxt.Enabled = false; 
        }

        private void quantityTxt_TextChanged(object sender, EventArgs e)
        {
            if (quantityTxt.Text != "")
            {
                if (rg.Match(quantityTxt.Text).Success)
                {
                    float quantity, price, total;
                    quantity = Convert.ToSingle(quantityTxt.Text);
                    price = Convert.ToSingle(perUnitPriceTxt.Text);
                    total = quantity * price;
                    totLabel.Text = total.ToString("########.##");
                }
                else
                    quantityTxt.SelectAll();
            }
            else
                totLabel.Text = "";
        }

        private void cartBtn_Click(object sender, EventArgs e)
        {
            if (supDD.SelectedIndex == -1)
                companyError.Visible = true;
            else
                companyError.Visible = false;

            if (!(reg.IsMatch(quantityTxt.Text)))
            {
                MessageBox.Show("Enter Numeric Digits Only");
                quantityTxt.Text = "";
                quantityTxt.Focus();
            }

            if (quantityTxt.Text == "")
                quntError.Visible = true;
            else
                quntError.Visible = false;

            if (barcodeTxt.Text == "")
                bcdError.Visible = true;
            else
                bcdError.Visible = false;

            if (companyError.Visible || quntError.Visible || bcdError.Visible)
                MainClass.show_msg("Fields with * are mandatory", "Stop", "Error"); //error is the type of msg
            else
            {
                dataGridView1.Rows.Add(productID, productTxt.Text, quantityTxt.Text, perUnitPriceTxt.Text, totLabel.Text);
                gt = gt + Convert.ToSingle(totLabel.Text);
                grosAmtLabel.Text = gt.ToString();
                productID = 0;
                productTxt.Text = "";
                perUnitPriceTxt.Text = "";
                barcodeTxt.Text = "";
                totLabel.Text = "0.00";
                quantityTxt.Text = "";
                Array.Clear(prodARR, 0, prodARR.Length);
            }
            
            totAmountTxt.Text = Convert.ToString(gt);
            gross = Convert.ToSingle(totAmountTxt.Text);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.ColumnIndex == 5)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    gt = gt - Convert.ToSingle(row.Cells["totalGV"].Value.ToString());
                    grosAmtLabel.Text = gt.ToString();
                    dataGridView1.Rows.Remove(row);
                }
            }
        }

        public override void addBtn_Click(object sender, EventArgs e)
        {
            MainClass.enable_reset(leftPanel);
        }

        public override void editBtn_Click(object sender, EventArgs e)
        {
            MainClass.enable(leftPanel);
        }

        int co;
        Int64 purInvoiceID;
        public override void savebtn_Click(object sender, EventArgs e)
        {
            if (!(reg.IsMatch(amountGivenTxt.Text)))
            {
                MessageBox.Show("Enter Numeric Digits Only");
                amountGivenTxt.Text = "";
                amountGivenTxt.Focus();
            }

            if (amountGivenTxt.Text == "")
                amountGivenError.Visible = true;
            else
                amountGivenError.Visible = false;
            
            if (amountGivenError.Visible)
                MainClass.show_msg("Fields with * are mandatory", "Stop", "Error"); //error is the type of msg
            else
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    using (TransactionScope sc = new TransactionScope())
                    {
                        if (amountGivenTxt.Text != "")
                        {
                            if (Convert.ToSingle(amountGivenTxt.Text) >= gross)
                                purInvoiceID = i.insertPurchaseInvoice(datePicker.Value, reterival.USER_ID, Convert.ToInt32(supDD.SelectedValue), gross, Convert.ToSingle(amountGivenTxt.Text), null);
                            else if (Convert.ToSingle(amountGivenTxt.Text) < gross)
                                purInvoiceID = i.insertPurchaseInvoice(datePicker.Value, reterival.USER_ID, Convert.ToInt32(supDD.SelectedValue), gross, Convert.ToSingle(amountGivenTxt.Text), Convert.ToSingle(remainAmtTxt.Text));
                        }

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            object productPacking = r.getProductQuantityWRTID(Convert.ToInt64(row.Cells["proIDGV"].Value));
                            packing_quan = productPacking.ToString();
                            output = Regex.Replace(packing_quan, "[^0-9]+", string.Empty);
                            object output_String = Regex.Replace(packing_quan, "[^a-zA-Z]", "");
                            string output_str = Convert.ToString(output_String);

                            co += i.insertPurchaseInvoiceDetails(purInvoiceID, Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()), Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()), Convert.ToSingle(row.Cells["totalGV"].Value.ToString()));
                            object[] arr = r.checkProductPriceExistance(Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()));
                            if (arr[2] != null)
                            {
                                //check for error
                                float disPercentage = Convert.ToSingle(row.Cells["pupGV"].Value.ToString()) * Convert.ToSingle(arr[3].ToString()) / 100;
                                float profitPercentage = Convert.ToSingle(row.Cells["pupGV"].Value.ToString()) * Convert.ToSingle(arr[4]) / 100;
                                float totalAmount = Convert.ToSingle(row.Cells["pupGV"].Value.ToString()) + profitPercentage + disPercentage;
                                float itemSPP = Convert.ToSingle(row.Cells["pupGV"].Value.ToString()) / Convert.ToSingle(output);
                                u.updateProductPrice(Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()), Convert.ToSingle(row.Cells["pupGV"].Value.ToString()), totalAmount, disPercentage, profitPercentage,itemSPP);
                            }
                            else
                                i.insertProductPrice(Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()), Convert.ToSingle(row.Cells["pupGV"].Value.ToString()));
                            int q;
                            object ob = r.getProductQuantity(Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()));
                            if (ob != null)
                            {
                                if ((output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                                {
                                    q = Convert.ToInt32(ob);
                                    q = q + Convert.ToInt32(row.Cells["quantityGV"].Value.ToString());
                                    u.updateStock(Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()), q, null);
                                }
                                else
                                {
                                    q = Convert.ToInt32(ob);
                                    q = q + Convert.ToInt32(row.Cells["quantityGV"].Value.ToString());
                                    itemMulQuan = Convert.ToInt32(output) * q;
                                    u.updateStock(Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()), q, Convert.ToInt32(itemMulQuan));
                                }
                            }
                            else
                            {
                                if ((output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                                {
                                    i.insertStock(Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()), Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()), null);
                                }
                                else
                                {
                                    itemMulQuan = Convert.ToInt32(output) * Convert.ToInt32(row.Cells["quantityGV"].Value.ToString());
                                    i.insertStock(Convert.ToInt32(row.Cells["proIDGV"].Value.ToString()), Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()), itemMulQuan);
                                }
                            }
                        }
                        if (co > 0)
                            MainClass.show_msg("Purchase Invoice Created Successfully", "Success", "Success");
                        else
                            MainClass.show_msg("Unable to Create Purchase Invoice", "Error", "Error");
                        sc.Complete();
                    }
                }
                dataGridView1.Rows.Clear();
                grosAmtLabel.Text = "0";
                MainClass.disable_reset(leftPanel);
            }
        }

        public override void delBtn_Click(object sender, EventArgs e)
        {

        }

        public override void searchTxt_TextChanged(object sender, EventArgs e)
        {

        }

        public override void viewBtn_Click(object sender, EventArgs e)
        {
            PurchaseInvoiceDetails_2 pid = new PurchaseInvoiceDetails_2();
            MainClass.show_window(pid, this, Form1.ActiveForm);

        }

        private void perUnitPriceTxt_TextChanged(object sender, EventArgs e)
        {
            if (perUnitPriceTxt.Text != "")
            {
                if (!rg.Match(perUnitPriceTxt.Text).Success)
                {
                    perUnitPriceTxt.Text = "";
                    perUnitPriceTxt.Focus();
                }

            }
        }
        
        private void barcodeTxt_Validating(object sender, CancelEventArgs e)
        {
            if (barcodeTxt.Text != "")
            {
                prodARR = r.getPWRBarcode(barcodeTxt.Text);
                productID = Convert.ToInt32(prodARR[0]);
                productTxt.Text = prodARR[1];

                string barco = prodARR[2];
                productTxt.Enabled = false;
                totAmountTxt.Enabled = false;
                remainAmtTxt.Enabled = false;
                if (barco != null)
                    perUnitPriceTxt.Focus();
            }
            else
            {
                productID = 0;
                productTxt.Text = "";
                perUnitPriceTxt.Text = "";
                Array.Clear(prodARR, 0, prodARR.Length);
            }
        }

        float sum = 0;
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
                remainAmtTxt.Text = "";
        }

        private void amountGivenTxt_Validating(object sender, CancelEventArgs e)
        {
            if (amountGivenTxt.Text != "")
            {
                if (Convert.ToSingle(amountGivenTxt.Text) < Convert.ToSingle(totAmountTxt.Text))
                {
                    sum = Convert.ToSingle(totAmountTxt.Text) - Convert.ToSingle(amountGivenTxt.Text);
                    remainAmtTxt.Text = sum.ToString();
                }
                else if (Convert.ToSingle(amountGivenTxt.Text) > Convert.ToSingle(totAmountTxt.Text))
                {
                    MessageBox.Show("Given Amount is greater then the Total Amount.");
                    amountGivenTxt.Text = "";
                    amountGivenTxt.Focus();
                    remainAmtTxt.Text = "";
                }
            }
        }
    }
}
