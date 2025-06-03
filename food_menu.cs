using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using ZXing;
using ZXing.QrCode;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Projectร้านกะเพรา2
{
    public partial class food_menu : Form
    {
        private string _username;
        public string username1
        {
            get { return _username; } //get คืนค่าจาก username
            set { _username = value; textBox1user.Text = value; } //set รับค่าจากภายนอกมาเก็บใน username และแสดงใน textBox1user
        }

        private MySqlConnection con; // ประกาศตัวแปร con ในคลาส Form2
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=admin ;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult re = MessageBox.Show("ต้องการชำระเงินหรือไม่", "ยืนยัน", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                _username = textBox1user.Text; // ดึงค่าจาก textBox1userเพื่อเก็บตัวเเปรชื่อผู้ที่ล็อกอิน
                QRnReceipt QRnReceipt = new QRnReceipt(_username); // ส่งค่า username ไป
                this.Hide();
                QRnReceipt.Show();
            }
        }

        public food_menu()
        {
            InitializeComponent();
            con = databaseConnection(); // กำหนดค่าให้กับตัวแปร con โดยเรียกใช้ฟังก์ชัน databaseConnection
        }
        //กำหนดขนาดของปุ่ม
        private const int BUTTON_WIDTH = 170;
        private const int BUTTON_HEIGHT = 170;
        private const int BUTTON_PADDING = 40;

        private void food_menu_Load(object sender, EventArgs e)
        {
            //LoadProductButtons();
            //dataGridViewCart.Refresh();
            //textBox1user.Text = Username;
            //showCart();

            try
            {
                panel1.Controls.Clear(); // ล้างคอนโทรลก่อนโหลดใหม่
                LoadProductButtons();

                if (dataGridViewCart != null)
                {
                    dataGridViewCart.Refresh(); // รีเฟรชตารางเพื่อให้ข้อมูลที่แสดงล่าสุด
                }
                else
                {
                    MessageBox.Show("dataGridViewCart is not initialized.");
                }

                if (!string.IsNullOrEmpty(_username)) //ตรวจสอบว่า _username มีค่าหรือไม่
                {
                    textBox1user.Text = _username;
                }
                else
                {
                    textBox1user.Text = "";
                }

                showCart(); // แสดงรายการสินค้าในตะกร้า
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during form load: " + ex.Message);
            }
        }

        private void LoadProductButtons(string query = null) // ดึงข้อมูลจาก product มาแสดงให้เลือกซื้อ
        {
            if (panel1 == null)
            {
                MessageBox.Show("panel1 is not initialized.");
                return;
            }

            panel1.Controls.Clear(); // ล้างคอนโทรลก่อนโหลดใหม่
            panel1.AutoScroll = true; // เปิด AutoScroll ก่อนที่จะโหลดข้อมูล

            string connectionString = "server=localhost;user=root;password=;database=admin";
            if (query == null)
            {
                query = "SELECT id, name, price, quantity, picture FROM product";
            }

            int buttonCount = 0;
            int currentRow = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString)) //อ่านข้อมูลจากฐานข้อมูลด้วย MySqlDataReader
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int productId = reader.GetInt32(0);
                        string productName = reader.GetString(1);
                        decimal productPrice = reader.GetDecimal(2);
                        decimal productQuantity = reader.GetDecimal(3);
                        byte[] productImageBytes = (byte[])reader["picture"];

                        // สร้าง PictureBox
                        PictureBox pictureBox = new PictureBox
                        {
                            Size = new Size(BUTTON_WIDTH, BUTTON_WIDTH),
                            SizeMode = PictureBoxSizeMode.Zoom, // เปลี่ยนเป็น Zoom
                            BackColor = Color.Black,
                            Location = new Point((BUTTON_WIDTH + BUTTON_PADDING) * (buttonCount % 3) + 40,
                                                 (BUTTON_WIDTH + BUTTON_PADDING + 25) * currentRow + 30)
                        };

                        // ตรวจสอบว่า productImageBytes มีข้อมูลก่อน
                        if (productImageBytes != null && productImageBytes.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(productImageBytes))
                            {
                                try
                                {
                                    pictureBox.Image = Image.FromStream(ms);
                                }
                                catch (ArgumentException ex)  
                                {
                                    // ถ้ามีปัญหาการแปลงภาพ
                                    MessageBox.Show("Error loading image: " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            pictureBox.Image = null; // กรณีไม่มีภาพ
                        }

                        panel1.Controls.Add(pictureBox);

                        // สร้าง Label สำหรับชื่อสินค้า
                        Label productNameLabel = new Label
                        {
                            Text = productName,
                            Size = new Size(120, 25),
                            BackColor = Color.White,
                            Location = new Point(pictureBox.Location.X, pictureBox.Location.Y + BUTTON_WIDTH + 10)
                        };
                        panel1.Controls.Add(productNameLabel);

                        // สร้าง TextBox สำหรับจำนวน
                        TextBox amountTextBox = new TextBox
                        {
                            Size = new Size(50, 25),
                            Text = "1",
                            Tag = productId,
                            Location = new Point(pictureBox.Location.X, pictureBox.Location.Y + BUTTON_WIDTH + 35)
                        };
                        panel1.Controls.Add(amountTextBox);

                        // สร้าง Button สำหรับเพิ่มสินค้า
                        Button productButton = new Button
                        {
                            Text = "เพิ่ม",
                            Size = new Size(100, 25),
                            ForeColor = Color.Red,
                            BackColor = Color.White,
                            Tag = new ProductInfo { Id = productId, Name = productName, Price = productPrice, Quantity = productQuantity },
                            Location = new Point(pictureBox.Location.X + 70, pictureBox.Location.Y + BUTTON_WIDTH + 35)
                        };
                        productButton.Click += button1_Click;
                        panel1.Controls.Add(productButton);

                        buttonCount++;
                        if (buttonCount % 3 == 0)
                        {
                            currentRow++;
                        }
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        //การประกาศคลาส ProductInfo แบบย่อย
        private class ProductInfo
        {
            public int Id { get; set; } //// ใช้เก็บข้อมูลของสินค้า
            public string Name { get; set; }
            public decimal Price { get; set; }
            public decimal Quantity { get; set; }
        }

   
        //คลิกปุ่มซื้อสินค้า
        private void button1_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                ProductInfo product = clickedButton.Tag as ProductInfo;
                if (product != null)
                {
                    // หาค่า TextBox ที่อยู่ถัดจากปุ่ม
                    TextBox amountTextBox = panel1.Controls
                        .OfType<TextBox>()
                        .FirstOrDefault(t => (int)t.Tag == product.Id);

                    int amount = 1; // ค่าเริ่มต้น

                    if (amountTextBox != null && int.TryParse(amountTextBox.Text, out int parsedAmount)) //ถ้ามี TextBox และสามารถแปลงข้อความในนั้นเป็นตัวเลขได้ก็ใช้ค่านั้น
                    {
                        amount = parsedAmount;
                    }

                    if (amount <= 0)
                    {
                        MessageBox.Show("กรุณาใส่จำนวนสินค้าที่ถูกต้อง");
                        return;
                    }

                    AddProductToCart(product.Id, product.Name, amount, product.Price);
                }
                else
                {
                    MessageBox.Show("Error: ไม่พบข้อมูลสินค้า");
                }
            }
        }


        //เพิ่มสินค้าไปยังตะกร้าสินค้า
        private void AddProductToCart(int productId, string productName, int amount, decimal productPrice)
        {
            try
            {
                string connectionString = "server=localhost;user=root;password=;database=admin";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // ตรวจสอบว่าสินค้าคงเหลือพอหรือไม่
                    MySqlCommand checkStockCommand = new MySqlCommand("SELECT quantity FROM product WHERE id = @productId", connection);
                    checkStockCommand.Parameters.AddWithValue("@productId", productId);
                    int stock = Convert.ToInt32(checkStockCommand.ExecuteScalar());

                    if (stock < amount)
                    {
                        MessageBox.Show("สินค้าหมด ไม่สามารถเพิ่มสินค้าในตะกร้าได้", "แจ้งเตือน", MessageBoxButtons.OK);
                        return;
                    }

                    // ตรวจสอบว่าสินค้ามีอยู่ในตะกร้าหรือไม่
                    MySqlCommand checkCommand = new MySqlCommand("SELECT qty FROM carts WHERE product_id = @productId", connection);
                    checkCommand.Parameters.AddWithValue("@productId", productId);
                    object result = checkCommand.ExecuteScalar();

                    if (result != null) // ถ้ามีสินค้าอยู่แล้ว
                    {
                        int existingQty = Convert.ToInt32(result);
                        MySqlCommand updateCommand = new MySqlCommand("UPDATE carts SET qty = @newQty, totalprice = price * @newQty WHERE product_id = @productId", connection);
                        updateCommand.Parameters.AddWithValue("@productId", productId);
                        updateCommand.Parameters.AddWithValue("@newQty", existingQty + amount);
                        updateCommand.ExecuteNonQuery();
                    }
                    else // ถ้ายังไม่มีสินค้าในตะกร้า
                    {
                        //เพิ่มค่าพารามิเตอร์ให้กับ SQL
                        MySqlCommand insertCommand = new MySqlCommand("INSERT INTO carts (product_id, name, qty, price, totalprice) VALUES (@productId, @productName, @amount, @productPrice, @totalprice)", connection);
                        insertCommand.Parameters.AddWithValue("@productId", productId);
                        insertCommand.Parameters.AddWithValue("@productName", productName);
                        insertCommand.Parameters.AddWithValue("@amount", amount);
                        insertCommand.Parameters.AddWithValue("@productPrice", productPrice);
                        insertCommand.Parameters.AddWithValue("@totalprice", productPrice * amount);
                        insertCommand.ExecuteNonQuery();
                    }

                    // คำนวณค่าใหม่ คำนวณยอดรวมราคาสินค้าในตะกร้า
                    MySqlCommand sumCommand = new MySqlCommand("SELECT SUM(totalprice) FROM carts", connection);
                    decimal subtotal = Convert.ToDecimal(sumCommand.ExecuteScalar());

                   
                    
                    //คำนวณยอดหลังหักส่วนลด
                    
                    //คำนวณ VAT (7% จากยอดก่อนลด)
                    decimal vat = subtotal * 0.07m;

                    // อัปเดตค่ารวมในตาราง carts (ถ้าเก็บค่ารวมไว้ในตารางนี้)
                    MySqlCommand updateSumCommand = new MySqlCommand("UPDATE carts SET subtotal = @subtotal, vat = @vat, total = @total ", connection);
                    updateSumCommand.Parameters.AddWithValue("@subtotal", subtotal);
                    
                    updateSumCommand.Parameters.AddWithValue("@vat", vat);
                    updateSumCommand.Parameters.AddWithValue("@total", subtotal);
                    updateSumCommand.ExecuteNonQuery();
                }

                showCart(); // อัปเดต DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        //แสดงรายการสินค้าในตะกร้าบน DataGridView
        public void showCart()
        {
            try
            {
                string connectionString = "server=localhost;user=root;password=;database=admin";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("SELECT name, qty, price, totalprice FROM carts", connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Clear เพื่อเตรียมแสดงข้อมูลใหม่
                    dataGridViewCart.Columns.Clear();
                    dataGridViewCart.Rows.Clear();

                    // Add columns 
                    dataGridViewCart.Columns.Add("ProductName", "สินค้า");
                    dataGridViewCart.Columns.Add("ProductQuantity", "จำนวน");
                    dataGridViewCart.Columns.Add("ProductPrice", "ราคา");
                    dataGridViewCart.Columns.Add("ProductTotal", "รวม");

                    // กำหนดความกว้าง
                    dataGridViewCart.Columns["ProductName"].Width = 144;
                    dataGridViewCart.Columns["ProductQuantity"].Width = 45;
                    dataGridViewCart.Columns["ProductPrice"].Width = 45;
                    dataGridViewCart.Columns["ProductTotal"].Width = 45;
                    //set เซนเตอร์
                    dataGridViewCart.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // คอลัมน์ที่ 2
                    dataGridViewCart.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // คอลัมน์ที่ 3
                    dataGridViewCart.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // คอลัมน์ที่ 4

                    decimal subtotal = 0; // ยอดรวมราคาสินค้าก่อนคิดภาษี
                    foreach (DataRow row in dataTable.Rows) //ดึงข้อมูลแต่ละแถว
                    {
                        string productName = row["name"].ToString();
                        int productQuantity = Convert.ToInt32(row["qty"]);
                        decimal productPrice = Convert.ToDecimal(row["price"]);
                        decimal productTotal = Convert.ToDecimal(row["totalprice"]); // ราคารวมสินค้า

                        // เพิ่มรายการสินค้าลงใน DataGridView
                        dataGridViewCart.Rows.Add(productName, productQuantity, productPrice, productTotal);

                        // บวก productTotal ของแต่ละรายการเข้าไปใน subtotal
                        subtotal += productTotal;
                    }

                    //คำนวณ ภาษีมูลค่าเพิ่ม (VAT) 7% จากยอดรวมก่อนภาษี (subtotal)
                    decimal vat = subtotal * 0.07m;

                    // คำนวณยอดรวมทั้งสิ้น
                    decimal totalAfterDiscountAndVat = subtotal - vat;

                    // แสดงยอดรวมและภาษีมูลค่าเพิ่ม (VAT) ใน TextBox
                    textBoxSubtotal.Text = totalAfterDiscountAndVat.ToString("N2");
                    textBoxVAT.Text = vat.ToString("N2");
                    textBoxTotal.Text = subtotal.ToString("N2");
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);

            }

        }

        //ฟังก์ชันที่ใช้เพื่อวาดข้อความลงกระดาษ
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("===================================================", new Font("Arial", 18, FontStyle.Regular), Brushes.Black, new Point(40, 80));
        }

        //ลบสินค้าเฉพาะรายการจากตะกร้า
        private void button2_Click(object sender, EventArgs e)
        {
            int selectedRow = dataGridViewCart.CurrentCell.RowIndex; //ดึงแถวที่ถูกเลือก
            string deleteName = dataGridViewCart.Rows[selectedRow].Cells[0].Value.ToString(); // อ้างอิงชื่อคอลัมน์โดยตำแหน่ง (index) ของคอลัมน์แรก
            MySqlConnection conn = databaseConnection();
            String sql = "DELETE FROM carts WHERE name = @deleteName";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@deleteName", deleteName);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
            if (rows > 0)
            {
                MessageBox.Show("ลบข้อมูลสำเร็จ");
                // อัพเดท dataGridViewCartให้แสดงข้อมูลล่าสุด
                showCart();
            }
        }
        //ลบสินค้าทั้งหมดในตะกร้า
        private void ClearCart()
        {
            try
            {
                // ลบข้อมูลในตาราง carts
                string connectionString = "server=localhost;user=root;password=;database=admin";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("DELETE FROM carts", connection);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {                      
                        showCart();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการลบรายการสินค้าในตะกร้า: " + ex.Message);
            }
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            ClearCart();
        }

        private void button4_Clic(object sender, EventArgs e)
        {
            Home_pasg Home_pasg = new Home_pasg();
            Home_pasg.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
