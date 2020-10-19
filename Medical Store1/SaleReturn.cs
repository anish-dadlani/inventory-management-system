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
using System.Collections;

namespace Medical_Store
{
    public partial class SaleReturn : Sample2
    {
        reterival r = new reterival();
        updation u = new updation();
        insertion i = new insertion();
        deletion d = new deletion();
        Regex rg = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
        float amtReturn, amtTotal;
        public SaleReturn()
        {
            InitializeComponent();
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            if (saleIDTxt.Text != "")
            {
                if (rg.Match(saleIDTxt.Text).Success)
                {
                    r.showSaleDataReturn(Convert.ToInt64(saleIDTxt.Text), dataGridView1, saleIDGV, barcodeGV, productGV, quantityGV, ppGV, totDisGV, totAmtGV, givenGV, returnGV, ppDisGv, ppTotGV, dateGV, userGv, payTypeGV, proIDGV, packingGV, itemNoGV);
                    if (dataGridView1.RowCount > 0)
                    {
                        dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.Rows[0].Cells["dateGV"].Value);
                        userTxt.Text = dataGridView1.Rows[0].Cells["userGv"].Value.ToString();
                        PayTypeTxt.Text = dataGridView1.Rows[0].Cells["payTypeGV"].Value.ToString();
                    }
                }
                else
                {
                    saleIDTxt.Text = "";
                    saleIDTxt.Focus();
                    dateTimePicker1.Value = DateTime.Now;
                    userTxt.Text = "";
                    PayTypeTxt.Text = "";
                }
            }
            else
            {
                dateTimePicker1.Value = DateTime.Now;
                userTxt.Text = "";
                PayTypeTxt.Text = "";
            }
        }

        private void SaleReturn_Load(object sender, EventArgs e)
        {
            base.addBtn.Enabled = false;
            base.editBtn.Enabled = false;
            base.delBtn.Enabled = false;
            base.viewBtn.Enabled = false;
            userLabel.Text = reterival.EMP_NAME;
        }

        float totalUpdate, returnUpdate;
        string[] arr = new string[9];
        //bool flag = false;
        public override void savebtn_Click(object sender, EventArgs e)
        {
            if (refundTxt.Text != "" && ht.Count > 0 && saleIDTxt.Text != "")
            {
                DialogResult dr = MessageBox.Show("Are you sure, you want to proceed?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (TransactionScope sc = new TransactionScope())
                    {
                        int x = 0;
                        foreach (DictionaryEntry de in ht)
                        {
                            x = x + i.insertRefundReturn(Convert.ToInt64(saleIDTxt.Text), DateTime.Now, reterival.USER_ID, Convert.ToInt64(de.Key), Convert.ToInt16(de.Value), Convert.ToSingle(refundTxt.Text));
                            int currentQuantity = (int)r.getProductQuantity(Convert.ToInt64(de.Key));
                            int currentItemTabQuantity = (int)r.getProductPackingQuantity(Convert.ToInt64(de.Key));

                            arr = r.getProductsBYWRTID(Convert.ToInt64(de.Key));

                            string packing = arr[6].ToString();
                            object output = Regex.Replace(packing, "[^0-9]+", string.Empty);
                            object output_String = Regex.Replace(packing, "[^a-zA-Z]", "");
                            string output_str = Convert.ToString(output_String);

                            int finalQuantity = currentQuantity + Convert.ToInt16(de.Value);
                            int finalItemTabQty = currentItemTabQuantity + Convert.ToInt16(de.Value);
                            int finalItemQuantity = currentItemTabQuantity + Convert.ToInt32(output);
                            
                            foreach (DataGridViewRow rows in dataGridView1.Rows)
                            {
                                //u.updateSaleReturn(Convert.ToInt64(saleIDTxt.Text), Convert.ToInt64(rows.Cells["proIDGV"].Value), Convert.ToInt32(rows.Cells["quantityGV"].Value), Convert.ToInt32(rows.Cells["itemNoGV"].Value));
                                
                                if (Convert.ToInt32(rows.Cells["itemNoGV"].Value) >= 1 && Convert.ToInt32(rows.Cells["quantityGV"].Value) > 1)
                                {
                                    if (Convert.ToInt64(rows.Cells["proIDGV"].Value) == Convert.ToInt64(de.Key))
                                        u.updateStock(Convert.ToInt64(de.Key), finalQuantity, finalItemQuantity);
                                }
                                else if (Convert.ToInt32(rows.Cells["itemNoGV"].Value) >= 1 && Convert.ToInt32(rows.Cells["quantityGV"].Value) == 1 && Convert.ToInt32(rows.Cells["itemNoGV"].Value) < Convert.ToInt32(output))
                                {
                                    //correct logical error
                                    if (Convert.ToInt64(rows.Cells["proIDGV"].Value) == Convert.ToInt64(de.Key))
                                    {
                                        int updatedQuanity = Convert.ToInt32(output) * currentQuantity;
                                        int updatedItemQty = Convert.ToInt32(rows.Cells["itemNoGV"].Value) + currentItemTabQuantity;
                                        updatedQuanity = updatedQuanity + Convert.ToInt32(output);
                                        if (updatedQuanity != updatedItemQty)
                                        {
                                            int updatedcurrentQuantity = currentQuantity + 1;
                                            u.updateStock(Convert.ToInt64(de.Key), updatedcurrentQuantity, finalItemTabQty);
                                        }
                                        else
                                        {
                                            //int updatedcurrentQuantity = currentQuantity + 1;
                                            u.updateStock(Convert.ToInt64(de.Key), currentQuantity, finalItemTabQty);
                                        }
                                    }
                                }
                                else if (Convert.ToInt32(rows.Cells["itemNoGV"].Value) == 0 && Convert.ToInt32(rows.Cells["quantityGV"].Value) >= 1)
                                {
                                    if (Convert.ToInt64(rows.Cells["proIDGV"].Value) == Convert.ToInt64(de.Key))
                                    {
                                        if ((output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                                        {
                                            u.updateStock(Convert.ToInt64(de.Key), finalQuantity, 0);
                                        }
                                        else
                                        {
                                            int updatedQuanity = Convert.ToInt32(output) * currentQuantity;
                                            int updatedItemQty = Convert.ToInt32(rows.Cells["itemNoGV"].Value) + currentItemTabQuantity;
                                            updatedQuanity = updatedQuanity - Convert.ToInt32(output);
                                            if (updatedQuanity != updatedItemQty)
                                            {
                                                //int updatedcurrentQuantity = currentQuantity + 1;
                                                u.updateStock(Convert.ToInt64(de.Key), currentQuantity, finalItemTabQty);
                                            }
                                            else
                                            {
                                                int updatedcurrentQuantity = currentQuantity + 1;
                                                u.updateStock(Convert.ToInt64(de.Key), updatedcurrentQuantity, finalItemTabQty);
                                            }
                                            //u.updateStock(Convert.ToInt64(de.Key), currentQuantity, finalItemTabQty);
                                            d.deleteSaleDetails(Convert.ToInt64(saleIDTxt.Text), Convert.ToInt64(rows.Cells["proIDGV"].Value));
                                            dataGridView1.Rows.Remove(rows);
                                        }
                                    }
                                        
                                }
                                else if (Convert.ToInt32(rows.Cells["itemNoGV"].Value) == 0 && Convert.ToInt32(rows.Cells["quantityGV"].Value) == 0)
                                {
                                    if (Convert.ToInt64(rows.Cells["proIDGV"].Value) == Convert.ToInt64(de.Key))
                                    {
                                        if ((output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                                        {
                                            u.updateStock(Convert.ToInt64(de.Key), finalQuantity, 0);
                                            u.updateSaleReturn(Convert.ToInt64(saleIDTxt.Text), Convert.ToInt64(rows.Cells["proIDGV"].Value), Convert.ToInt32(rows.Cells["quantityGV"].Value), Convert.ToInt32(rows.Cells["itemNoGV"].Value));
                                            d.deleteSaleDetails(Convert.ToInt64(saleIDTxt.Text), Convert.ToInt64(rows.Cells["proIDGV"].Value));
                                            dataGridView1.Rows.Remove(rows);
                                        }
                                        else
                                        {
                                            u.updateStock(Convert.ToInt64(de.Key), finalQuantity, finalItemQuantity);
                                            u.updateSaleReturn(Convert.ToInt64(saleIDTxt.Text), Convert.ToInt64(rows.Cells["proIDGV"].Value), Convert.ToInt32(rows.Cells["quantityGV"].Value), Convert.ToInt32(rows.Cells["itemNoGV"].Value));
                                            d.deleteSaleDetails(Convert.ToInt64(saleIDTxt.Text), Convert.ToInt64(rows.Cells["proIDGV"].Value));
                                            dataGridView1.Rows.Remove(rows);
                                        }
                                    }
                                }
                            }
                            foreach (DataGridViewRow row2 in dataGridView1.Rows)
                            {
                                u.updateSaleReturn(Convert.ToInt64(saleIDTxt.Text), Convert.ToInt64(row2.Cells["proIDGV"].Value), Convert.ToInt32(row2.Cells["quantityGV"].Value), Convert.ToInt32(row2.Cells["itemNoGV"].Value));
                            }
                            u.updateSaleData(Convert.ToInt64(saleIDTxt.Text), totalUpdate, returnUpdate);
                            object totalIncomeAmount;
                            float updatedIncomeAmount;
                            object incomeAmountCheck = r.getIncomeAmountDate(DateTime.Today);
                            
                            if (incomeAmountCheck.GetType() == typeof(DBNull))
                            {
                                //i.insertDailyExpenditure("Sale Refund", Convert.ToSingle(refundTxt.Text), DateTime.Now);
                                i.insertDailyIncome(-Convert.ToSingle(refundTxt.Text), DateTime.Now);
                            }
                            else
                            {
                                totalIncomeAmount = r.getIncomeAmountDate(DateTime.Today);
                                updatedIncomeAmount = Convert.ToSingle(totalIncomeAmount) - Convert.ToSingle(refundTxt.Text);
                                u.updateDailyIncome(updatedIncomeAmount, DateTime.Today);
                            }
                        }
                        if (x > 0)
                        {
                            MainClass.show_msg("Return and Refund Successfull", "Success", "Success");
                            x = 0;
                            ht.Clear();
                        }
                        barcodeTxt.Text = "";
                        refundTxt.Text = "";
                        sc.Complete();
                    }
                }
            }
            else
                MainClass.show_msg("Please Provide Complete Details", "Error", "Error");

        }

        float amountReturn = 0;
        string[] arr2 = new string[9];
        Hashtable ht = new Hashtable();

        private void barcodeTxt_Validating(object sender, CancelEventArgs e)
        {
            if (barcodeTxt.Text != "")
            {
                if (dataGridView1.RowCount > 0)
                {
                    using (TransactionScope sc = new TransactionScope())
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (barcodeTxt.Text == row.Cells["barcodeGV"].Value.ToString())
                            {
                                DialogResult dr = MessageBox.Show("Are you sure you want to Return " + row.Cells["productGV"].Value.ToString() + "?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dr == DialogResult.Yes)
                                
                                {
                                    Int64 product_id = Convert.ToInt64(row.Cells["proIDGV"].Value.ToString());
                                    arr2 = r.getProductsBYWRTID(product_id);
                                    float product_price = Convert.ToSingle(row.Cells["ppGV"].Value.ToString());
                                    int product_quantity = 0; //= Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()) - 1;
                                    int item_qty = 0;
                                    string packing = arr2[6].ToString();
                                    object output = Regex.Replace(packing, "[^0-9]+", string.Empty);
                                    object output_String = Regex.Replace(packing, "[^a-zA-Z]", "");
                                    string output_str = Convert.ToString(output_String);

                                    amtTotal = Convert.ToSingle(row.Cells["totAmtGV"].Value.ToString());

                                    if (Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()) == 1  && Convert.ToInt32(row.Cells["itemNoGV"].Value.ToString()) >= 1 && Convert.ToInt32(row.Cells["itemNoGV"].Value.ToString()) < Convert.ToInt32(output))
                                    {
                                        if ((output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                                        {
                                            product_quantity = 1;
                                            item_qty = Convert.ToInt32(row.Cells["itemNoGV"].Value) - Convert.ToInt32(output);
                                        }
                                        else
                                        {
                                            product_quantity = 1;
                                            item_qty = Convert.ToInt32(row.Cells["itemNoGV"].Value) - 1;
                                        }
                                    }
                                    else if (Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()) >= 1 && Convert.ToInt32(row.Cells["itemNoGV"].Value.ToString()) >= 1)
                                    {
                                        product_quantity = Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()) - 1;
                                        item_qty = Convert.ToInt32(row.Cells["itemNoGV"].Value) - Convert.ToInt32(output);
                                    }
                                    else if (Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()) >= 1 && Convert.ToInt32(row.Cells["itemNoGV"].Value.ToString()) == 0)
                                    {
                                        product_quantity = Convert.ToInt32(row.Cells["quantityGV"].Value.ToString()) - 1;
                                        item_qty = 0;
                                    }
                                    
                                    //check for error
                                    if (row.Cells["returnGV"].Value == null || row.Cells["returnGV"].Value.ToString() == "")
                                    {
                                        MessageBox.Show("The customer has still remaining amount to pay...");
                                    }
                                    else
                                        amtReturn = Convert.ToSingle(row.Cells["returnGV"].Value.ToString());
                                    
                                    totalUpdate = amtTotal - product_price;
                                    returnUpdate = amtReturn + product_price;

                                    amountReturn += product_price;
                                    refundTxt.Text = Math.Ceiling(amountReturn).ToString();
                                    amountReturn = 0;
                                    if (product_quantity == 0 && item_qty == 0)
                                    {
                                        row.Cells["quantityGV"].Value = product_quantity;
                                        row.Cells["itemNoGV"].Value = item_qty;
                                        row.Cells["ppTotGV"].Value = Convert.ToSingle(row.Cells["ppGV"].Value.ToString()) * product_quantity;
                                        if (ht.ContainsKey(row.Cells["proIDGV"].Value))
                                        {
                                            Int64 proIDht = Convert.ToInt64(row.Cells["proIDGV"].Value.ToString());
                                            ht[proIDht] = Convert.ToInt32(ht[proIDht]) - 1;
                                        }
                                        else
                                        {
                                            ht.Add(row.Cells["proIDGV"].Value, 1);
                                        }
                                        //dataGridView1.Rows.Remove(row);
                                    }
                                    else if (product_quantity >= 1 && item_qty == 0)
                                    {
                                        row.Cells["quantityGV"].Value = product_quantity;
                                        row.Cells["itemNoGV"].Value = item_qty;

                                        if ((output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                                        {
                                            row.Cells["ppTotGV"].Value = Convert.ToSingle(row.Cells["ppGV"].Value.ToString()) * product_quantity;
                                        }
                                        else
                                        {
                                            row.Cells["ppTotGV"].Value = Convert.ToSingle(row.Cells["ppGV"].Value.ToString()) * item_qty;
                                        }

                                        if (ht.ContainsKey(row.Cells["proIDGV"].Value))
                                        {
                                            Int64 proIDht = Convert.ToInt64(row.Cells["proIDGV"].Value.ToString());
                                            ht[proIDht] = Convert.ToInt32(ht[proIDht]) - 1;
                                        }
                                        else
                                        {
                                            ht.Add(row.Cells["proIDGV"].Value, 1);
                                        }
                                    }
                                    else if (product_quantity > 1 && item_qty >= 1)
                                    {
                                        row.Cells["quantityGV"].Value = product_quantity;
                                        row.Cells["itemNoGV"].Value = item_qty;
                                        row.Cells["ppTotGV"].Value = Convert.ToSingle(row.Cells["ppGV"].Value.ToString()) * product_quantity;
                                        if (ht.ContainsKey(row.Cells["proIDGV"].Value))
                                        {
                                            Int64 proIDht = Convert.ToInt64(row.Cells["proIDGV"].Value.ToString());
                                            ht[proIDht] = Convert.ToInt32(ht[proIDht]) + 1;
                                        }
                                        else
                                        {
                                            ht.Add(row.Cells["proIDGV"].Value, 1);
                                        }
                                    }
                                    else if (product_quantity == 1 && item_qty >= 1)
                                    {
                                        row.Cells["itemNoGV"].Value = item_qty;
                                        row.Cells["quantityGV"].Value = product_quantity;

                                        if ((output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                                        {
                                            row.Cells["ppTotGV"].Value = Convert.ToSingle(row.Cells["ppGV"].Value.ToString()) * product_quantity;
                                            if (ht.ContainsKey(row.Cells["proIDGV"].Value))
                                            {
                                                Int64 proIDht = Convert.ToInt64(row.Cells["proIDGV"].Value.ToString());
                                                ht[proIDht] = Convert.ToInt32(ht[proIDht]) + 1;
                                            }
                                            else
                                            {
                                                ht.Add(row.Cells["proIDGV"].Value, 1);
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(output) == item_qty)
                                            {
                                                row.Cells["ppTotGV"].Value = Convert.ToSingle(row.Cells["ppGV"].Value.ToString()) * product_quantity;
                                                if (ht.ContainsKey(row.Cells["proIDGV"].Value))
                                                {
                                                    Int64 proIDht = Convert.ToInt64(row.Cells["proIDGV"].Value.ToString());
                                                    ht[proIDht] = Convert.ToInt32(ht[proIDht]) + Convert.ToInt32(output);
                                                }
                                                else
                                                {
                                                    ht.Add(row.Cells["proIDGV"].Value, Convert.ToInt32(output));
                                                }
                                            }
                                            else
                                            {
                                                row.Cells["ppTotGV"].Value = Convert.ToSingle(row.Cells["ppGV"].Value.ToString()) * item_qty;
                                                if (ht.ContainsKey(row.Cells["proIDGV"].Value))
                                                {
                                                    Int64 proIDht = Convert.ToInt64(row.Cells["proIDGV"].Value.ToString());
                                                    ht[proIDht] = Convert.ToInt32(ht[proIDht]) + 1;
                                                }
                                                else
                                                {
                                                    ht.Add(row.Cells["proIDGV"].Value, 1);
                                                }
                                            }   
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
