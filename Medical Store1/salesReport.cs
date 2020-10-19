using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace Medical_Store
{
    public partial class salesReport : Form
    {
        reterival r = new reterival();
        ReportDocument rd;
        public salesReport()
        {
            InitializeComponent();
        }

        private void salesReport_Load(object sender, EventArgs e)
        {
            rd = new ReportDocument();
            if (SalesDetails.saleID == 0)
            {
                r.showReport(rd, crystalReportViewer2, "st_getSalesReciptUser ", "@user", reterival.USER_ID);
            }
            else
            {
                r.showReport(rd, crystalReportViewer2, "st_getSalesReciptUserSaleID ", "@saleiD", SalesDetails.saleID);
                SalesDetails.saleID = 0;
            }
        }

        private void salesReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rd != null)
            {
                rd.Close();
            }
        }
    }
}
