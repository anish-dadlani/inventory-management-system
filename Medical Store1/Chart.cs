using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Medical_Store
{
    public partial class Chart : Sample2
    {
        int index;
        reterival r = new reterival();
        public Chart()
        {
            InitializeComponent();
        }

        private void Chart_Load(object sender, EventArgs e)
        {
            userLabel.Text = reterival.EMP_NAME;
            chart1.Hide();
            base.addBtn.Enabled = false;
            base.editBtn.Enabled = false;
            base.savebtn.Enabled = false;
            base.delBtn.Enabled = false;
            r.showStatistics(dataGridView1, yearGV, monthGV, salesGV, earningGV, monthNOGV);
        }

        private void product_quantity_chart()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getAllStock", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                chart1.DataSource = ds;
                chart1.Series["Graph"].XValueMember = "Product";
                chart1.Series["Graph"].YValueMembers = "Available Stock";
                chart1.Titles.Add("Product - Quantity");
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        private void daily_sales_chart()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getSalesDailyWise", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                chart1.DataSource = ds;
                chart1.Series["Graph"].XValueMember = "Sales Count";
                chart1.Series["Graph"].YValueMembers = "Total Amount";
                chart1.Titles.Add("Daily Sales");
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        private void monthly_sales_chart()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getSalesMonthWise", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                chart1.DataSource = ds;
                chart1.Series["Graph"].XValueMember = "Month Name";
                chart1.Series["Graph"].YValueMembers = "Total Amount";
                chart1.Titles.Add("Monthly Sales");
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        private void yearly_sales_chart()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("st_getSalesYearWise", MainClass.con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                chart1.DataSource = ds;
                chart1.Series["Graph"].XValueMember = "Year";
                chart1.Series["Graph"].YValueMembers = "Total Amount";
                chart1.Titles.Add("Yearly Sales");
            }
            catch (Exception ex)
            {
                MainClass.show_msg(ex.Message, "Error", "Error");
            }
        }

        private void ChartDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = ChartDD.SelectedIndex;
            switch (index)
            {
                case 0:
                    panel6.Hide();
                    chart1.Titles.Clear();
                    chart1.Show();
                    product_quantity_chart();
                    break;
                case 1:
                    panel6.Hide();
                    chart1.Titles.Clear();
                    chart1.Show();
                    daily_sales_chart();
                    break;
                case 2:
                    panel6.Hide();
                    chart1.Titles.Clear();
                    chart1.Show();
                    monthly_sales_chart();
                    break;
                case 3:
                    panel6.Hide();
                    chart1.Titles.Clear();
                    chart1.Show();
                    yearly_sales_chart();
                    break;
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

        }

        public override void delBtn_Click(object sender, EventArgs e)
        {

        }

        public override void searchTxt_TextChanged(object sender, EventArgs e)
        {

        }

        public override void viewBtn_Click(object sender, EventArgs e)
        {
            panel6.Show();
            r.showStatistics(dataGridView1, yearGV, monthGV, salesGV, earningGV, monthNOGV);
        }
    }
}
