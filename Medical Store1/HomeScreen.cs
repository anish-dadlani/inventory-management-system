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
    public partial class HomeScreen : Sample
    {
        public HomeScreen()
        {
            InitializeComponent();
        }

        private void userBtn_Click(object sender, EventArgs e)
        {
            Users_2 u = new Users_2();
            MainClass.show_window(u, this, Form1.ActiveForm);
        }

        private void catBtn_Click(object sender, EventArgs e)
        {
            Categories_2 c = new Categories_2();
            MainClass.show_window(c, this, Form1.ActiveForm);
        }

        private void prodBtn_Click(object sender, EventArgs e)
        {
            Products_2 p = new Products_2();
            MainClass.show_window(p, this, Form1.ActiveForm);
        }

        private void HomeScreen_Load(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.logoutToolStripMenuItem.Enabled = true;
            userLabel.Text = reterival.EMP_NAME;
        }

        private void suppBtn_Click(object sender, EventArgs e)
        {
            Suppliers_2 s = new Suppliers_2();
            MainClass.show_window(s, this, Form1.ActiveForm);
        }

        private void piBtn_Click(object sender, EventArgs e)
        {
            PurchaseInvoice_2 pi = new PurchaseInvoice_2();
            MainClass.show_window(pi, this, Form1.ActiveForm);
        }

        private void stockBtn_Click(object sender, EventArgs e)
        {
            stocks st = new stocks();
            MainClass.show_window(st, this, Form1.ActiveForm);
        }

        private void saleBtn_Click(object sender, EventArgs e)
        {
            sales sal = new sales();
            MainClass.show_window(sal, this, Form1.ActiveForm);
        }

        private void prodPriceBtn_Click(object sender, EventArgs e)
        {
            productPricing pr = new productPricing();
            MainClass.show_window(pr, this, Form1.ActiveForm);
        }

        private void saleRtrnBtn_Click(object sender, EventArgs e)
        {
            SaleReturn sr = new SaleReturn();
            MainClass.show_window(sr, this, Form1.ActiveForm);
        }

        private void chartBtn_Click(object sender, EventArgs e)
        {
            Chart chart = new Chart();
            MainClass.show_window(chart, this, Form1.ActiveForm);
        }

        private void clientBtn_Click(object sender, EventArgs e)
        {
            Clients clients = new Clients();
            MainClass.show_window(clients, this, Form1.ActiveForm);
        }

        private void creditDebitBtn_Click(object sender, EventArgs e)
        {
            Credit_Debit_Balance cdb = new Credit_Debit_Balance();
            MainClass.show_window(cdb, this, Form1.ActiveForm);
        }

        private void BRBtn_Click(object sender, EventArgs e)
        {
            //Back_up_Restore br = new Back_up_Restore();
            //MainClass.show_window(br, this, Form1.ActiveForm);
            MessageBox.Show("Please Wait, Under Development");
        }

        private void DEBtn_Click(object sender, EventArgs e)
        {
            Daily_Expenditure de = new Daily_Expenditure();
            MainClass.show_window(de, this, Form1.ActiveForm);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
