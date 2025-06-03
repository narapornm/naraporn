using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Projectร้านกะเพรา2
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public string Username { get; set; }

        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=admin;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        private void button1_Click(object sender, EventArgs e) //ปุ่มย้อนกลับ
        {
            Home_pasg Form3 = new Home_pasg();
            Form3.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) //จะไปหน้าสมัครสมาชิก
        {
            Register Form3 = new Register();
            Form3.Show();
            this.Hide();
        }

        private void buttonback89_Click(object sender, EventArgs e) //ปุ่มย้อนกลับมาหน้าเเรก
        {
            new Home_pasg().Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)  //ปุ่มลบรหัส
        {
            textBox1usernameeee.Text = ""; //ลบ
            textBox2passssss.Text = "";
            textBox1usernameeee.Focus();
        }

        private void button2_Click(object sender, EventArgs e)  //ปุ่มยืนยัน
        {
            MySqlConnection con = databaseConnection();  // เชื่อมต่อกับฐานข้อมูล MySQL
            con.Open();

            // สร้างคำสั่ง SQL เพื่อตรวจสอบข้อมูลการเข้าสู่ระบบของผู้ใช้
            // โดยตรวจสอบค่าของช่อง username และ password ในฐานข้อมูล MySQL ว่าตรงกับข้อมูลที่ผู้ใช้ป้อน
            string login = "SELECT * FROM users WHERE username= '" + textBox1usernameeee.Text + "' and password= '" + textBox2passssss.Text + "'";
            MySqlCommand cmd = new MySqlCommand(login, con);
            MySqlDataReader dr = cmd.ExecuteReader();  // ตัวเก็บ

            if (dr.Read()) // ตรวจสอบว่ามีผู้ใช้ที่ตรงกับข้อมูลที่รับมาหรือไม่ 
            {
                Username = dr["username"].ToString(); // ดึงชื่อผู้ใช้จากผลลัพธ์                                            
                food_menu food_menu = new food_menu
                {
                    username1 = Username,
                };
                food_menu.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void textBox2passssss_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
    

