namespace Medical_Store
{
    partial class sendCode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.codeVerifyLabel = new System.Windows.Forms.Label();
            this.codeSentLabel = new System.Windows.Forms.Label();
            this.codeBtn = new System.Windows.Forms.Button();
            this.emAddBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.codeTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.emAddTxt = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(431, 100);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Location = new System.Drawing.Point(106, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password Reset";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.codeVerifyLabel);
            this.panel2.Controls.Add(this.codeSentLabel);
            this.panel2.Controls.Add(this.codeBtn);
            this.panel2.Controls.Add(this.emAddBtn);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.codeTxt);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.emAddTxt);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(431, 245);
            this.panel2.TabIndex = 1;
            // 
            // codeVerifyLabel
            // 
            this.codeVerifyLabel.AutoSize = true;
            this.codeVerifyLabel.ForeColor = System.Drawing.Color.Red;
            this.codeVerifyLabel.Location = new System.Drawing.Point(312, 130);
            this.codeVerifyLabel.Name = "codeVerifyLabel";
            this.codeVerifyLabel.Size = new System.Drawing.Size(0, 13);
            this.codeVerifyLabel.TabIndex = 7;
            // 
            // codeSentLabel
            // 
            this.codeSentLabel.AutoSize = true;
            this.codeSentLabel.ForeColor = System.Drawing.Color.Green;
            this.codeSentLabel.Location = new System.Drawing.Point(312, 37);
            this.codeSentLabel.Name = "codeSentLabel";
            this.codeSentLabel.Size = new System.Drawing.Size(0, 13);
            this.codeSentLabel.TabIndex = 6;
            // 
            // codeBtn
            // 
            this.codeBtn.Location = new System.Drawing.Point(195, 169);
            this.codeBtn.Name = "codeBtn";
            this.codeBtn.Size = new System.Drawing.Size(110, 23);
            this.codeBtn.TabIndex = 5;
            this.codeBtn.Text = "Verify Code";
            this.codeBtn.UseVisualStyleBackColor = true;
            this.codeBtn.Click += new System.EventHandler(this.codeBtn_Click);
            // 
            // emAddBtn
            // 
            this.emAddBtn.Location = new System.Drawing.Point(195, 57);
            this.emAddBtn.Name = "emAddBtn";
            this.emAddBtn.Size = new System.Drawing.Size(110, 23);
            this.emAddBtn.TabIndex = 4;
            this.emAddBtn.Text = "Send Code";
            this.emAddBtn.UseVisualStyleBackColor = true;
            this.emAddBtn.Click += new System.EventHandler(this.emAddBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Enter Code";
            // 
            // codeTxt
            // 
            this.codeTxt.Location = new System.Drawing.Point(132, 127);
            this.codeTxt.Name = "codeTxt";
            this.codeTxt.Size = new System.Drawing.Size(174, 20);
            this.codeTxt.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enter Email Address";
            // 
            // emAddTxt
            // 
            this.emAddTxt.Location = new System.Drawing.Point(132, 30);
            this.emAddTxt.Name = "emAddTxt";
            this.emAddTxt.Size = new System.Drawing.Size(174, 20);
            this.emAddTxt.TabIndex = 0;
            // 
            // sendCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 295);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "sendCode";
            this.Text = "sendCode";
            //this.Load += new System.EventHandler(this.sendCode_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button codeBtn;
        private System.Windows.Forms.Button emAddBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox codeTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox emAddTxt;
        private System.Windows.Forms.Label codeSentLabel;
        private System.Windows.Forms.Label codeVerifyLabel;
    }
}