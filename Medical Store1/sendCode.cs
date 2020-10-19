using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Medical_Store
{
    public partial class sendCode : Form
    {
        string randomCode;
        public static string to;
        public sendCode()
        {
            InitializeComponent();
        }

        private void emAddBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string pattern = null;
                pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

                if (emAddTxt.Text.ToString() != "")
                {
                    if (!(Regex.IsMatch(emAddTxt.Text, pattern)))
                    {
                        MessageBox.Show("Invalid Email Address.");
                        emAddTxt.Text = "";
                        emAddTxt.Focus();
                    }
                    else
                    {
                        if (reterival.getUserEmail(emAddTxt.Text))
                        {
                            string from, pass, messageBody;
                            Random rand = new Random();
                            randomCode = (rand.Next(999999)).ToString();
                            MailMessage message = new MailMessage();
                            to = emAddTxt.Text.ToString();
                            from = "healer.technology.9794@gmail.com";
                            pass = "a9d7m9t4";
                            messageBody = "Your Password Reset Code is " + randomCode;
                            message.To.Add(to);
                            message.From = new MailAddress(from);
                            message.Body = messageBody;
                            message.Subject = "Password Reset Code";
                            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                            smtp.EnableSsl = true;
                            smtp.Port = 587;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.Credentials = new NetworkCredential(from, pass);
                            smtp.Send(message);
                            codeSentLabel.Text = "Code Sent.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void codeBtn_Click(object sender, EventArgs e)
        {
            if (randomCode == codeTxt.Text.ToString())
            {
                sendCode sc = new sendCode();
                sc.Close();
                to = emAddTxt.Text;
                ResetPassword rp = new ResetPassword();
                rp.Show();
            }
            else
            {
                codeVerifyLabel.Text = "Invalid Code.";
            }
        }
    }
}
