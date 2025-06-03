using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Projectร้านกะเพรา2
{
    public partial class Admin_Login : Form
    {
        public Admin_Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่า textbox1 คือ "ADMIN" และ textbox2 คือ "12345678"
            if (textBox1.Text == "ADMIN" && textBox2.Text == "12345678")
            {
                // แสดงกล่องข้อความแจ้งเตือนว่าผู้ใช้เข้าสู่ระบบสำเร็จ
                MessageBox.Show("เข้าสู่ระบบสำเร็จ!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Admin Admin = new Admin();
                Admin.Show();
                this.Hide();
            }
            else
            {
                // หากค่าไม่ตรงกับที่ต้องการ แสดงข้อความแจ้งเตือน
                MessageBox.Show("ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Home_pasg Home_pasg = new Home_pasg();
            Home_pasg.Show();
            this.Hide();
        }

        private void Admin_Login_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
