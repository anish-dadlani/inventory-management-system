namespace Medical_Store
{
    partial class Back_up_Restore
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
            this.backUpBtn = new System.Windows.Forms.Button();
            this.leftPanel.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Size = new System.Drawing.Size(491, 40);
            // 
            // backBtn
            // 
            this.backBtn.FlatAppearance.BorderSize = 0;
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.backUpBtn);
            this.leftPanel.Size = new System.Drawing.Size(200, 507);
            this.leftPanel.Controls.SetChildIndex(this.panel1, 0);
            this.leftPanel.Controls.SetChildIndex(this.panel4, 0);
            this.leftPanel.Controls.SetChildIndex(this.backUpBtn, 0);
            // 
            // rightPanel
            // 
            this.rightPanel.Size = new System.Drawing.Size(491, 507);
            // 
            // panel2
            // 
            this.panel2.Size = new System.Drawing.Size(491, 35);
            // 
            // backUpBtn
            // 
            this.backUpBtn.BackColor = System.Drawing.Color.IndianRed;
            this.backUpBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backUpBtn.FlatAppearance.BorderSize = 2;
            this.backUpBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backUpBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backUpBtn.Location = new System.Drawing.Point(3, 102);
            this.backUpBtn.Name = "backUpBtn";
            this.backUpBtn.Size = new System.Drawing.Size(194, 28);
            this.backUpBtn.TabIndex = 26;
            this.backUpBtn.Text = "BackUP";
            this.backUpBtn.UseVisualStyleBackColor = false;
            this.backUpBtn.Click += new System.EventHandler(this.backUpBtn_Click);
            // 
            // Back_up_Restore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 507);
            this.Name = "Back_up_Restore";
            this.Text = "Back_up_Restore";
            this.Load += new System.EventHandler(this.Back_up_Restore_Load);
            this.leftPanel.ResumeLayout(false);
            this.rightPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button backUpBtn;
    }
}