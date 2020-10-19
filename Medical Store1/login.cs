using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Medical_Store
{
    public partial class login : Sample
    {
        public login()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (username_text.Text == "")
                usrNameError.Visible = true;
            else
                usrNameError.Visible = false;

            if (password_text.Text == "")
                pass_Error.Visible = true;
            else
                pass_Error.Visible = false;

            if (usrNameError.Visible || pass_Error.Visible)
            {
                MainClass.show_msg("Fields with * are mandatory", "Stop", "Error"); //Error is the type of msg
            }
            else
            {
                if (reterival.getUserDetails(username_text.Text, password_text.Text))
                {
                   HomeScreen hm = new HomeScreen();
                   MainClass.show_window(hm, this, Form1.ActiveForm);
                }
            }
        }

        private void username_text_TextChanged(object sender, EventArgs e)
        {
            if (username_text.Text == "")
                usrNameError.Visible = true;
            else
                usrNameError.Visible = false;
        }

        private void password_text_TextChanged(object sender, EventArgs e)
        {
            if (password_text.Text == "")
                pass_Error.Visible = true;
            else
                pass_Error.Visible = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            sendCode sc = new sendCode();
            sc.Show();
        }
    }
}
