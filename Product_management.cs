using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Projectร้านกะเพรา2
{
    public partial class Product_management : Form
    {
        public Product_management()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Admin Admin = new Admin();
            Admin.Show();
            this.Hide();
        }

        private void Product_management_Load(object sender, EventArgs e)
        {
            showEquipment();  //ฟังก์ชันเพื่อดึงและแสดงข้อมูลสินค้าจากฐานข้อมูล
        }

        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=admin;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        //ดึงข้อมูลจากฐานข้อมูล MySQL และแสดงผลใน DataGridView
        private void showEquipment()
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();
            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM product";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();
            dataproduct.DataSource = ds.Tables[0].DefaultView; //นำข้อมูลใน DataSet มาแสดงใน DataGridView ที่ชื่อ dataproduct
        }

       

        
        private byte[] imageBytes; // ประกาศตัวแปรชั้น global

       

       

        private void dataproduct_CellClick(object sender, DataGridViewCellEventArgs e) //โหลดข้อมูลจากเซลล์ในแถวที่ถูกเลือก
        {
            dataproduct.CurrentRow.Selected = true;
            textBoxname.Text = dataproduct.Rows[e.RowIndex].Cells["name"].FormattedValue.ToString();
            textBoxprice.Text = dataproduct.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
            textBoxquantity.Text = dataproduct.Rows[e.RowIndex].Cells["quantity"].FormattedValue.ToString();

        }

       


        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog()) //OpenFileDialog เพื่อให้ผู้ใช้สามารถเลือกไฟล์จากเครื่อง
            {
                openFileDialog1.Filter = "Image Files (.jpg, *.jpeg, *.png, *.gif)|.jpg; *.jpeg; *.png; *.gif"; // ตั้งค่า Filter ให้เลือกไฟล์ภาพเท่านั้น

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // เมื่อผู้ใช้เลือกไฟล์ภาพแล้ว
                    string imagePath = openFileDialog1.FileName;

                    // อ่านไฟล์ภาพเป็น byte array แล้วเก็บไว้ในตัวแปร global "imageBytes"
                    imageBytes = File.ReadAllBytes(imagePath);

                    // เพื่อดึงชื่อไฟล์ (ไม่รวมเส้นทาง) และแสดงใน textBoxpic
                    textBoxpic.Text = Path.GetFileName(imagePath);

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่า imageBytes ไม่ใช่ค่า null หรือข้อมูลว่างเปล่าก่อนที่จะทำการเพิ่มข้อมูล
            if (imageBytes != null && imageBytes.Length > 0)
            {
                MySqlConnection conn = databaseConnection();
                String sql = "INSERT INTO product (name, price, quantity, picture) VALUES(@name, @price, @quantity, @picture)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", textBoxname.Text);
                cmd.Parameters.AddWithValue("@price", textBoxprice.Text);
                cmd.Parameters.AddWithValue("@quantity", textBoxquantity.Text);
                cmd.Parameters.AddWithValue("@picture", imageBytes);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();
                if (rows > 0)
                {
                    MessageBox.Show("เพิ่มข้อมูลสำเร็จ");
                    showEquipment();

                    // ล้างข้อมูลใน TextBox หลังจากเพิ่มข้อมูลสำเร็จ
                    textBoxname.Clear();
                    textBoxprice.Clear();
                    textBoxquantity.Clear();
                    textBoxpic.Clear();
                }
            }
            else
            {
                MessageBox.Show("กรุณาเลือกไฟล์รูปภาพ");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int selectedRow = dataproduct.CurrentCell.RowIndex; //แถวที่เลือก
            int deleteId = Convert.ToInt32(dataproduct.Rows[selectedRow].Cells["id"].Value);
            MySqlConnection conn = databaseConnection();
            String sql = "DELETE FROM product WHERE id = '" + deleteId + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
            if (rows > 0)
            {
                MessageBox.Show("ลบข้อมูลสำเร็จ");
                showEquipment();   // โหลดข้อมูลสินค้าใหม่และแสดงใน DataGridView

                // ล้างข้อมูลใน TextBox 
                textBoxname.Clear();
                textBoxprice.Clear();
                textBoxquantity.Clear();
                textBoxpic.Clear();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // ตรวจสอบว่าเลือกแถวหรือยัง
                if (dataproduct.CurrentRow == null)
                {
                    MessageBox.Show("กรุณาเลือกสินค้าที่ต้องการแก้ไข");
                    return;
                }

                // ตรวจสอบว่าทุกช่องจะไม่ว่าง
                string name = textBoxname.Text.Trim();
                string price = textBoxprice.Text.Trim();
                string quantity = textBoxquantity.Text.Trim();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(price) || string.IsNullOrEmpty(quantity))
                {
                    MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน");
                    return;
                }
                // ตรวจสอบว่า price เป็นตัวเลข > 0
                if (!decimal.TryParse(price, out decimal priceValue) || priceValue <= 0)
                {
                    MessageBox.Show("กรุณากรอกราคาสินค้าที่ถูกต้อง (ต้องเป็นตัวเลขมากกว่า 0)");
                    return;
                }                // ตรวจสอบว่า price เป็นตัวเลข > 0
                if (!decimal.TryParse(quantity, out decimal quantityValue) || quantityValue <= 0)
                {
                    MessageBox.Show("กรุณากรอกจำนวนสินค้าที่ถูกต้อง (ต้องเป็นตัวเลขมากกว่า 0)");
                    return;
                }

                // ตรวจสอบว่ามีรูปอยู่หรือไม่
                if (imageBytes == null || imageBytes.Length == 0)
                {
                    MessageBox.Show("ไม่พบรูปภาพ กรุณาเลือกรูปใหม่ หรือคลิกสินค้าจากตารางเพื่อโหลดรูปเดิม");
                    return;
                }

                int selectedRow = dataproduct.CurrentCell.RowIndex;
                int editId = Convert.ToInt32(dataproduct.Rows[selectedRow].Cells["id"].Value);

                using (MySqlConnection conn = databaseConnection())
                {
                    string sql = "UPDATE product SET name = @name, price = @price, quantity = @quantity, picture = @picture WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@picture", imageBytes);
                    cmd.Parameters.AddWithValue("@id", editId);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rows > 0)
                    {
                        MessageBox.Show("แก้ไขข้อมูลสำเร็จ");
                        showEquipment();

                        // ล้างข้อมูล
                        textBoxname.Clear();
                        textBoxprice.Clear();
                        textBoxquantity.Clear();
                        textBoxpic.Clear();
                        imageBytes = null;

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
    }
}
