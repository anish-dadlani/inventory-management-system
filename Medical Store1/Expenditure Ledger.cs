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
    public partial class Expenditure_Ledger : Sample2
    {
        reterival r = new reterival();
        insertion i = new insertion();
        updation u = new updation();

        public Expenditure_Ledger()
        {
            InitializeComponent();
        }

        private void Expenditure_Ledger_Load(object sender, EventArgs e)
        {
            MainClass.disable_reset(leftPanel);
            userLabel.Text = reterival.EMP_NAME;
            r.showIncomeExpeneseBalance(dataGridView1, dateGV, incomeGV, expenseGV, netGV);
        }
    }
}
