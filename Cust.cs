using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projectร้านกะเพรา2
{
    public partial class Cust : Form
    {
        public Cust()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        //ฟังก์ชันนี้ใช้กับ TextBox ที่รับเฉพาะตัวอักษร (a-z, A-Z) เท่านั้น หากผู้ใช้พิมพ์ตัวเลขหรืออักขระพิเศษก็จะแสดงข้อความแจ้งเตือนและไม่ให้พิมพ์
        private void OnlyEnglishLetters_KeyPress(object sender, KeyPressEventArgs e)
        {
            // อนุญาตเฉพาะ a-z, A-Z และ backspace
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true; // ป้องกันไม่ให้พิมพ์
                MessageBox.Show("กรุณากรอกเฉพาะตัวอักษรภาษาอังกฤษเท่านั้น");
            }
        }
        //ส่วนในฟังก์ชั่นว่าได้เลือกเเถวใน
        private void button1_Click(object sender, EventArgs e)
        {
            string phone = textBox2.Text.Trim();

            // เช็คว่ามี 10 หลัก เป็นตัวเลข และขึ้นต้นด้วย 0
            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0\d{9}$"))
            {
                MessageBox.Show("กรุณากรอกเบอร์โทรให้ถูกต้อง (ต้องเป็นตัวเลข 10 หลัก และขึ้นต้นด้วย 0)");
                return;
            }
            try
            {
                // ตรวจสอบว่าเลือกแถวหรือยัง
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("กรุณาเลือกสินค้าที่ต้องการแก้ไข");
                    return;
                }

                // ตรวจสอบว่าทุกช่องไม่ว่าง
                string name = textBox1.Text.Trim();
                string surname = textBox5.Text.Trim();
                string tel = textBox2.Text.Trim();
                string username = textBox3.Text.Trim();
                string password = textBox4.Text.Trim();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(tel) || string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน");
                    return;
                }


                //ดึง id ของแถวที่เลือกโดยใช้คำสั่ง UPDATE เพื่อแก้ไขข้อมูลในแถว ถ้าอัปเดตสำเร็จก็จะแสดงข้อความ 
                int selectedRow = dataGridView1.CurrentCell.RowIndex;
                int editId = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells["id"].Value);

                using (MySqlConnection conn = databaseConnection())
                {
                    string sql = "UPDATE users SET `first_name` = @name, `last_name` = @surname, `phone_number` = @tel, `username` = @username, `password` = @password WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@surname", surname);
                    cmd.Parameters.AddWithValue("@tel", tel);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@id", editId);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rows > 0)
                    {
                        MessageBox.Show("แก้ไขข้อมูลสำเร็จ");
                        Cust_Load();

                        // ล้างข้อมูล
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                    }
                    else
                    {
                        MessageBox.Show("ไม่พบข้อมูลที่จะแก้ไข หรือไม่มีการเปลี่ยนแปลง");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }
    

        private void button3_Click(object sender, EventArgs e)
        {
            Admin Admin = new Admin();
            Admin.Show();
            this.Hide();
        }

        //เชื่อมต่อกับฐานข้อมูลไปยังฐานข้อมูลชื่อ admin
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=admin;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        
        private void Cust_Load()
        {
            //ดึงข้อมูลมาแสดง

            MySqlConnection conn = databaseConnection();  // เชื่อมต่อฐานข้อมูล
            DataSet ds = new DataSet();
            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users";  // สร้างคำสั่ง SQL ให้ดึงข้อมูลทั้งหมดจากตาราง users

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();
            dataGridView1.DataSource = ds.Tables[0].DefaultView;   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedRow = dataGridView1.CurrentCell.RowIndex; //แถวที่เลือก
            int deleteId = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells["id"].Value);
            MySqlConnection conn = databaseConnection();
            String sql = "DELETE FROM users WHERE id = '" + deleteId + "'";   //อันนี้ก็จะเป้นส่วนในการลบข้อมูลจากเเถวที่เลือก id
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
            if (rows > 0)
            {
                MessageBox.Show("ลบข้อมูลสำเร็จ");
                Cust_Load();

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
            }

        }

        //ฟังก์ชันใช้โหลดข้อมูลจากตาราง users ทั้งหมด แสดงใน dataGridView1
        private void Cust_Load(object sender, EventArgs e)
        {
            Cust_Load();
        }

        //ฟังก์ชันปุ่มเพิ่ม
        private void button4_Click(object sender, EventArgs e) 
        {
            string phone = textBox2.Text.Trim();

            // ตรวจสอบเช็คว่ามีเบอร์โทร 10 หลัก เป็นตัวเลข และขึ้นต้นด้วย 0
            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0\d{9}$"))
            {
                MessageBox.Show("กรุณากรอกเบอร์โทรให้ถูกต้อง (ต้องเป็นตัวเลข 10 หลัก และขึ้นต้นด้วย 0)");
                return;
            }
            MySqlConnection conn = databaseConnection(); //การเชื่อมต่อกับฐานข้อมูลโดยเรียกใช้ฟังก์ชัน databaseConnection
            String sql = "INSERT INTO users (first_name, last_name, phone_number, username, password) VALUES(@first_name, @last_name, @phonenumber, @username, @password)"; //คำสั่ง SQL สำหรับเพิ่มข้อมูลเข้าไปในตาราง users โดยใช้ parameter
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@first_name", textBox1.Text);
                cmd.Parameters.AddWithValue("@last_name", textBox5.Text);
                cmd.Parameters.AddWithValue("@phonenumber", textBox2.Text);
                cmd.Parameters.AddWithValue("@username", textBox3.Text);
                cmd.Parameters.AddWithValue("@password", textBox4.Text);

            conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();
                if (rows > 0)
                {
                    MessageBox.Show("เพิ่มข้อมูลสำเร็จ");
                    Cust_Load();
                // ล้างข้อมูลใน TextBox หลังจากเพิ่มข้อมูลสำเร็จ
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
            }
            
            else
            {
                MessageBox.Show("กรุณาเลือกไฟล์รูปภาพ");
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // ตรวจสอบว่าเลือกแถวที่ไม่ใช่ header
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex]; //ดึงข้อมูลทั้งแถว (row) ที่ถูกคลิก จากตาราง dataGridView1

                // กำหนดค่าจากแต่ละคอลัมน์ใน DataGridView ไปยัง TextBox
                textBox1.Text = row.Cells["first_name"].Value.ToString();
                textBox5.Text = row.Cells["last_name"].Value.ToString();
                textBox2.Text = row.Cells["phone_number"].Value.ToString();
                textBox3.Text = row.Cells["username"].Value.ToString();
                textBox4.Text = row.Cells["password"].Value.ToString();
            }
        }
    }
}
