using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Medical_Store
{
    public partial class Categories_2 : Sample2
    {
        int edit = 0; //this 0 is an indication for save operation and 1 is an indication for update operation
        int catID;
        short stat;
        reterival r = new reterival();

        public Categories_2()
        {
            InitializeComponent();
        }

        private void Categories_2_Load_1(object sender, EventArgs e)
        {
            MainClass.disable_reset(leftPanel);
            r.show_categories(dataGridView1, catIDGV, catNameGV, StatusGV);
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
            if (categoryTxt.Text == "")
                catNameError.Visible = true;
            else
                catNameError.Visible = false;

            if (isactiveDD.SelectedIndex == -1)
                activeError.Visible = true;
            else
                activeError.Visible = false;

            if (catNameError.Visible || activeError.Visible)
            {
                MainClass.show_msg("Fields with * are mandatory", "Stop", "Error"); //Error is the type of msg
            }
            else
            {
                if (isactiveDD.SelectedIndex == 0)
                    stat = 1;
                else if (isactiveDD.SelectedIndex == 1)
                    stat = 0;
                if (edit == 0) // code for save operation
                {
                    insertion i = new insertion();
                    i.insertCategory(categoryTxt.Text, stat);
                    r.show_categories(dataGridView1, catIDGV, catNameGV, StatusGV);
                    MainClass.disable_reset(leftPanel);

                }
                else if (edit == 1) // code for update operation
                {
                    DialogResult dr = MessageBox.Show("Are you sure, you want to update record?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        updation u = new updation();
                        if (isactiveDD.SelectedIndex == 0)
                            stat = 1;
                        else if (isactiveDD.SelectedIndex == 1)
                            stat = 0;
                        u.updateCategory(catID, categoryTxt.Text, stat);
                        r.show_categories(dataGridView1, catIDGV, catNameGV, StatusGV);
                        MainClass.disable_reset(leftPanel);
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
                    d.delete(catID, "st_deleteCategory", "@id");
                    r.show_categories(dataGridView1, catIDGV, catNameGV, StatusGV);
                    MainClass.disable_reset(leftPanel);
                }
            }
        }

        public override void searchTxt_TextChanged(object sender, EventArgs e)
        {
            if (searchTxt.Text != "")
            {
                r.show_categories(dataGridView1, catIDGV, catNameGV, StatusGV, searchTxt.Text);
            }
            else
            {
                r.show_categories(dataGridView1, catIDGV, catNameGV, StatusGV);
            }
        }

        public override void viewBtn_Click(object sender, EventArgs e)
        {
            r.show_categories(dataGridView1, catIDGV, catNameGV, StatusGV);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                edit = 1;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                catID = Convert.ToInt32(row.Cells["catIDGV"].Value.ToString());
                categoryTxt.Text = row.Cells["catNameGV"].Value.ToString();
                isactiveDD.SelectedItem = row.Cells["StatusGV"].Value.ToString();
                MainClass.disable(leftPanel);
            }
        }
   }
}
