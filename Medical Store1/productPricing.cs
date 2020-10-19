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
    public partial class productPricing : Sample2
    {
        reterival r = new reterival();
        updation u = new updation();
        Regex rg = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

        public productPricing()
        {
            InitializeComponent();
            r.getList("st_getCategoriesDataList", catDD, "Category", "ID");
        }

        private void productPricing_Load(object sender, EventArgs e)
        {
            base.addBtn.Enabled = false;
            base.editBtn.Enabled = false;
            base.viewBtn.Enabled = false;
            base.delBtn.Enabled = false;
            dataGridView1.AutoGenerateColumns = false;
            userLabel.Text = reterival.EMP_NAME;
        }

        private void catDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (catDD.SelectedIndex != -1 && catDD.SelectedIndex != 0)
                r.showProductsWRTCategory(Convert.ToInt32(catDD.SelectedValue.ToString()), dataGridView1, proIDGV, ProductGV, packingGV, buyingPriceGV, finalPriceGV, discountGV, profitMarginGV, pispGV);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
            {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells["profitMarginGV"].Value != null && rg.Match(row.Cells["profitMarginGV"].Value.ToString()).Success)
                {
                    string packing = row.Cells["packingGV"].Value.ToString();
                    object output = Regex.Replace(packing, "[^0-9]+", string.Empty);
                    object output_String = Regex.Replace(packing, "[^a-zA-Z]", "");
                    string output_str = Convert.ToString(output_String);

                    float itemSP = 0;
                    float itemSpDisc = 0;
                    float buyingPrice = Convert.ToSingle(row.Cells["buyingPriceGV"].Value.ToString());
                    float profitMargin = Convert.ToSingle(row.Cells["profitMarginGV"].Value.ToString()) / 100;
                    
                    float amountToIncrease = profitMargin * buyingPrice;
                    float discountPer;
                    float finalSellingPrice = buyingPrice + amountToIncrease;

                    if (row.Cells["discountGV"].Value != null && rg.Match(row.Cells["discountGV"].Value.ToString()).Success)
                    {
                        discountPer = finalSellingPrice * (Convert.ToSingle(row.Cells["discountGV"].Value.ToString()) / 100);
                    }
                    else
                    {
                        discountPer = 0;
                    }             
     
                    row.Cells["finalPriceGV"].Value = Math.Ceiling(finalSellingPrice - discountPer);
                    itemSpDisc = Convert.ToSingle(row.Cells["finalPriceGV"].Value);


                    if ((output_str == "ML" || output_str == "ml" || output_str == "Ml") && !(output_str == "Tab" || output_str == "TAB" || output_str == "tab" || output_str == "Tablet" || output_str == "tablet" || output_str == "TABLET" || output_str == "Tablets" || output_str == "tablets" || output_str == "TABLETS" || output_str == "Cap" || output_str == "cap" || output_str == "CAP" || output_str == "Capsules" || output_str == "CAPSULES" || output_str == "capsules" || output_str == "Capsule" || output_str == "CAPSULE" || output_str == "capsule"))
                    {
                        row.Cells["pispGV"].Value = 0;
                        itemSP = 0;
                    }
                    else
                    {
                        itemSP = itemSpDisc / Convert.ToSingle(output);
                        row.Cells["pispGV"].Value = Math.Ceiling(itemSP);
                    }
                }
                else
                {
                    row.Cells["finalPriceGV"].Value = null;
                    row.Cells["discountGV"].Value = null;
                    row.Cells["profitMarginGV"].Value = null;
                    row.Cells["pispGV"].Value = null;
                } 
            }
        }

        public override void addBtn_Click(object sender, EventArgs e)
        {

        }

        public override void editBtn_Click(object sender, EventArgs e)
        {

        }

        public override void savebtn_Click(object sender, EventArgs e)
        {
            int check = 0;
            if (catDD.SelectedIndex != -1 && catDD.SelectedIndex != 0)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if ((bool)row.Cells["selectGV"].FormattedValue == true)
                    {
                        check++;
                        float disc, pm, bp, sp, itemSP;
                        Int64 pID;
                        
                        pID = Convert.ToInt32(row.Cells["proIDGV"].Value.ToString());
                        bp = Convert.ToSingle(row.Cells["buyingPriceGV"].Value.ToString());
                        
                        disc = row.Cells["discountGV"].Value == null?0:Convert.ToSingle(row.Cells["discountGV"].Value.ToString());
                        pm = row.Cells["profitMarginGV"].Value == null?0:Convert.ToSingle(row.Cells["profitMarginGV"].Value.ToString());
                        itemSP = row.Cells["pispGV"].Value == null ? 0 : Convert.ToSingle(row.Cells["pispGV"].Value.ToString());
                        
                        if (disc == 0 && pm == 0)
                            sp = bp;
                        else
                            sp = Convert.ToSingle(row.Cells["finalPriceGV"].Value.ToString());

                        u.updateProductPrice(pID, bp, sp, disc, pm, itemSP);
                    }
                }
                if (check > 0)
                {
                    MainClass.show_msg("Product Pricing updated successfully", "Success", "Success");
                    check = 0;
                }
                else
                {
                    MainClass.show_msg("Please select any product", "Error", "Error");
                    check = 0;
                }
            }
        }

        public override void delBtn_Click(object sender, EventArgs e)
        {

        }

        public override void searchTxt_TextChanged(object sender, EventArgs e)
        {
            if (searchTxt.Text != "")
                r.showProductsWRTCategory(Convert.ToInt32(catDD.SelectedValue.ToString()), dataGridView1, proIDGV, ProductGV, packingGV, buyingPriceGV, finalPriceGV, discountGV, profitMarginGV, pispGV, searchTxt.Text);
            else
                r.showProductsWRTCategory(Convert.ToInt32(catDD.SelectedValue.ToString()), dataGridView1, proIDGV, ProductGV, packingGV, buyingPriceGV, finalPriceGV, discountGV, profitMarginGV, pispGV);
        }

        public override void viewBtn_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
