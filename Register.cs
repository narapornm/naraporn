using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projectร้านกะเพรา2
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }


        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=admin;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่าไม่มีช่องกรอกว่าง
            if (textBox1first.Text == "" || textBox2last.Text == "" || textBox3phone.Text == "" || textBox1username.Text == "" || textBox2pass.Text == "" || textBox2confirmpass.Text == "")
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน", "การลงทะเบียนล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ตรวจสอบว่า first_name, last_name ไม่มีตัวเลขและอักขระพิเศษ
            if (!Regex.IsMatch(textBox1first.Text, @"^[a-zA-Zก-ฮ]+$") || !Regex.IsMatch(textBox2last.Text, @"^[a-zA-Zก-ฮ]+$"))
            {
                MessageBox.Show("ชื่อและนามสกุลต้องไม่ประกอบด้วยตัวเลขหรืออักขระพิเศษ", "การลงทะเบียนล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ตรวจสอบว่า phone_number เป็นตัวเลข 10 หลัก
            if (!Regex.IsMatch(textBox3phone.Text, @"^\d{10}$"))
            {
                MessageBox.Show("หมายเลขโทรศัพท์ต้องเป็นตัวเลข 10 หลัก", "การลงทะเบียนล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ตรวจสอบว่า username ไม่มีอักขระพิเศษ
            if (!Regex.IsMatch(textBox1username.Text, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("ชื่อผู้ใช้ไม่สามารถมีอักขระพิเศษได้", "การลงทะเบียนล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ตรวจสอบว่า password มีความยาวมากกว่า 8 ตัวอักษร
            if (textBox2pass.Text.Length <= 8)
            {
                MessageBox.Show("รหัสผ่านต้องมีความยาวมากกว่า 8 ตัวอักษร", "การลงทะเบียนล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ตรวจสอบว่า password และ confirm password ตรงกัน
            if (textBox2pass.Text != textBox2confirmpass.Text)
            {
                MessageBox.Show("รหัสผ่านไม่ตรงกัน กรุณากรอกรหัสผ่านใหม่", "การลงทะเบียนล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2pass.Text = "";
                textBox2confirmpass.Text = "";
                textBox2pass.Focus();
                return;
            }

            // ถ้าผ่านการตรวจสอบทั้งหมด
            MySqlConnection con = databaseConnection();
            con.Open();
            string register = "INSERT INTO users (first_name, last_name, phone_number, username, password) VALUES ('" + textBox1first.Text + "','" + textBox2last.Text + "','" + textBox3phone.Text + "','" + textBox1username.Text + "','" + textBox2pass.Text + "')";
            MySqlCommand cmd = new MySqlCommand(register, con);
            cmd.ExecuteNonQuery();
            con.Close();

            // ล้างข้อมูลในฟอร์ม
            textBox1first.Text = "";
            textBox2last.Text = "";
            textBox3phone.Text = "";
            textBox1username.Text = "";
            textBox2pass.Text = "";
            textBox2confirmpass.Text = "";

            // แสดงข้อความการลงทะเบียนสำเร็จ
            MessageBox.Show("สร้างบัญชีสำเร็จ", "การลงทะเบียนสำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // เปิดหน้าจอเข้าสู่ระบบและซ่อนหน้าจอนี้
            Login Login = new Login();
            Login.Show();
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login Login = new Login();
            Login.Show();
            this.Hide();
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }




        //private void textBox2_TextChanged(object sender, EventArgs e)
        //{
        //    // เก็บค่าที่ผู้ใช้กรอกลงไป
        //    string input = textBox2last.Text;

        //    // ตรวจสอบว่ามีตัวอักษรที่ไม่ใช่ตัวเลขหรือไม่
        //    if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^[0-9]*$"))
        //    {
        //        MessageBox.Show("กรุณากรอกเฉพาะตัวเลขเท่านั้น", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        textBox2last.Text = string.Empty;  // ล้างค่าใน TextBox
        //        return;
        //    }

        //    // ตรวจสอบความยาวของเบอร์โทรศัพท์ไม่เกิน 10 หลัก
        //    if (input.Length > 10)
        //    {
        //        MessageBox.Show("เบอร์โทรศัพท์ต้องมีไม่เกิน 10 หลัก", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        textBox2last.Text = input.Substring(0, 10);  // ตัดตัวอักษรให้เหลือ 10 หลัก
        //        textBox2last.SelectionStart = textBox2last.Text.Length;  // เลื่อนเคอร์เซอร์ไปท้ายสุด
        //    }
        //}




        //private void Register_Load(object sender, EventArgs e)
        //{
        //    textBox1first.Focus();
        //}
    }
}
