namespace Projectร้านกะเพรา2
{
    partial class Register
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
            this.textBox1first = new System.Windows.Forms.TextBox();
            this.textBox2last = new System.Windows.Forms.TextBox();
            this.textBox3phone = new System.Windows.Forms.TextBox();
            this.textBox1username = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2pass = new System.Windows.Forms.TextBox();
            this.textBox2confirmpass = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1first
            // 
            this.textBox1first.Location = new System.Drawing.Point(283, 182);
            this.textBox1first.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1first.Multiline = true;
            this.textBox1first.Name = "textBox1first";
            this.textBox1first.Size = new System.Drawing.Size(253, 30);
            this.textBox1first.TabIndex = 0;
            // 
            // textBox2last
            // 
            this.textBox2last.Location = new System.Drawing.Point(716, 192);
            this.textBox2last.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2last.Multiline = true;
            this.textBox2last.Name = "textBox2last";
            this.textBox2last.Size = new System.Drawing.Size(253, 30);
            this.textBox2last.TabIndex = 1;
            // 
            // textBox3phone
            // 
            this.textBox3phone.Location = new System.Drawing.Point(390, 277);
            this.textBox3phone.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox3phone.Multiline = true;
            this.textBox3phone.Name = "textBox3phone";
            this.textBox3phone.Size = new System.Drawing.Size(167, 30);
            this.textBox3phone.TabIndex = 2;
            // 
            // textBox1username
            // 
            this.textBox1username.Location = new System.Drawing.Point(845, 286);
            this.textBox1username.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1username.Multiline = true;
            this.textBox1username.Name = "textBox1username";
            this.textBox1username.Size = new System.Drawing.Size(183, 30);
            this.textBox1username.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.button1.Location = new System.Drawing.Point(667, 505);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 62);
            this.button1.TabIndex = 4;
            this.button1.Text = "ยืนยัน";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(231, 505);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(153, 62);
            this.button2.TabIndex = 5;
            this.button2.Text = "ย้อนกลับ";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2pass
            // 
            this.textBox2pass.Location = new System.Drawing.Point(368, 377);
            this.textBox2pass.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2pass.Multiline = true;
            this.textBox2pass.Name = "textBox2pass";
            this.textBox2pass.Size = new System.Drawing.Size(189, 33);
            this.textBox2pass.TabIndex = 6;
            // 
            // textBox2confirmpass
            // 
            this.textBox2confirmpass.Location = new System.Drawing.Point(905, 381);
            this.textBox2confirmpass.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2confirmpass.Multiline = true;
            this.textBox2confirmpass.Name = "textBox2confirmpass";
            this.textBox2confirmpass.Size = new System.Drawing.Size(132, 29);
            this.textBox2confirmpass.TabIndex = 7;
            // 
            // Register
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Projectร้านกะเพรา2.Properties.Resources.หน้าSIGN_UP__4_;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1059, 628);
            this.Controls.Add(this.textBox2confirmpass);
            this.Controls.Add(this.textBox2pass);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1username);
            this.Controls.Add(this.textBox3phone);
            this.Controls.Add(this.textBox2last);
            this.Controls.Add(this.textBox1first);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Register";
            this.Text = "Signup";
            this.Load += new System.EventHandler(this.Register_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1first;
        private System.Windows.Forms.TextBox textBox2last;
        private System.Windows.Forms.TextBox textBox3phone;
        private System.Windows.Forms.TextBox textBox1username;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2pass;
        private System.Windows.Forms.TextBox textBox2confirmpass;
    }
}