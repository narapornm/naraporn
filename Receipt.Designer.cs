namespace Projectร้านกะเพรา2
{
    partial class QRnReceipt
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
            this.button1 = new System.Windows.Forms.Button();
            this.buttonreceipt = new System.Windows.Forms.Button();
            this.labelTotalAmount = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::Projectร้านกะเพรา2.Properties.Resources.Screenshot_2024_10_10_203545;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(687, 452);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 46);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonreceipt
            // 
            this.buttonreceipt.BackgroundImage = global::Projectร้านกะเพรา2.Properties.Resources.Screenshot_2024_10_10_211741;
            this.buttonreceipt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonreceipt.Location = new System.Drawing.Point(340, 406);
            this.buttonreceipt.Margin = new System.Windows.Forms.Padding(2);
            this.buttonreceipt.Name = "buttonreceipt";
            this.buttonreceipt.Size = new System.Drawing.Size(129, 46);
            this.buttonreceipt.TabIndex = 2;
            this.buttonreceipt.UseVisualStyleBackColor = true;
            this.buttonreceipt.Click += new System.EventHandler(this.buttonreceipt_Click);
            // 
            // labelTotalAmount
            // 
            this.labelTotalAmount.AutoSize = true;
            this.labelTotalAmount.Location = new System.Drawing.Point(546, 373);
            this.labelTotalAmount.Name = "labelTotalAmount";
            this.labelTotalAmount.Size = new System.Drawing.Size(35, 13);
            this.labelTotalAmount.TabIndex = 3;
            this.labelTotalAmount.Text = "label1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(293, 128);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(238, 225);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // QRnReceipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Projectร้านกะเพรา2.Properties.Resources.หน้าช่องทางการจ่ายเงิน__1_;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(790, 507);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelTotalAmount);
            this.Controls.Add(this.buttonreceipt);
            this.Controls.Add(this.button1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "QRnReceipt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QRnReceipt";
            this.Load += new System.EventHandler(this.QRnReceipt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonreceipt;
        private System.Windows.Forms.Label labelTotalAmount;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}