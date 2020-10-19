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
    public partial class ResetPassword : Form
    {
        reterival r = new reterival();
        public ResetPassword()
        {
            InitializeComponent();
        }

        private void codeBtn_Click(object sender, EventArgs e)
        {
            if (NewPassTxt.Text == NewPassConfTxt.Text)
            {
                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[users] SET [usr_password] = '" + NewPassTxt.Text + "' WHERE usr_email = '" + sendCode.to + "'", MainClass.con);
                MainClass.con.Open();
                cmd.ExecuteNonQuery();
                MainClass.con.Close();
                MessageBox.Show("Password Reset Successful.");
            }
            else
            {
                MessageBox.Show("Password do not match. Try Again");
            }
        }
    }
}
