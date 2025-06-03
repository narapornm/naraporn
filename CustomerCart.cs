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
    public partial class CustomerCart : Form
    {
        public CustomerCart()
        {
            InitializeComponent();
        }

        private void CustomerCart_Load(object sender, EventArgs e)
        {
            showCart();
        }

        //สำหรับแสดงประวัติการซื้อสินค้าจากฐานข้อมูล
        private void showCart()
        {
            try
            {
                string connectionString = "server=localhost;user=root;password=;database=admin"; //ฟังก์ชันเชื่อมต่อข้อมูล
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT name, qty, price, totalprice, subtotal, vat, discount, total, datetime, username, receipt_ad FROM history"; //ดึงจากฐานข้อมูลจากตาราง history
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);   //ดึงข้อมูลด้วย MySqlDataAdapter แล้วเก็บลง DataTable

                    // เตรียม DataGridView
                    dataGridViewCart.Columns.Clear();
                    dataGridViewCart.Rows.Clear();
                    dataGridViewCart.AutoGenerateColumns = false;

                    // เพิ่มคอลัมน์ (ชื่อภาษาไทย)
                    dataGridViewCart.Columns.Add("name", "ชื่อสินค้า");
                    dataGridViewCart.Columns.Add("qty", "จำนวน");
                    dataGridViewCart.Columns.Add("price", "ราคา");
                    dataGridViewCart.Columns.Add("totalprice", "รวม");

                    dataGridViewCart.Columns.Add("subtotal", "ยอดก่อน VAT");
                    dataGridViewCart.Columns.Add("vat", "VAT");
                    dataGridViewCart.Columns.Add("discount", "ส่วนลด");
                    dataGridViewCart.Columns.Add("total", "ยอดสุทธิ");

                    dataGridViewCart.Columns.Add("datetime", "วันที่");
                    dataGridViewCart.Columns.Add("username", "ผู้ใช้");
                    dataGridViewCart.Columns.Add("receipt_ad", "ใบเสร็จ");

                    // ตั้งค่า Alignment และความกว้าง
                    foreach (DataGridViewColumn column in dataGridViewCart.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }

                    dataGridViewCart.Columns["qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewCart.Columns["price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewCart.Columns["totalprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewCart.Columns["subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewCart.Columns["vat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewCart.Columns["discount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewCart.Columns["total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    // เพิ่มข้อมูลจาก DataTable เข้า DataGridView
                    foreach (DataRow row in dataTable.Rows)
                    {
                        //ดึงค่าจากแต่ละแถวของ DataTable และเพิ่มเข้าไปใน dataGridViewCart
                        dataGridViewCart.Rows.Add(
                            row["name"],
                            row["qty"],
                            row["price"],
                            row["totalprice"],
                            row["subtotal"],
                            row["vat"],
                            row["discount"],
                            row["total"],
                            Convert.ToDateTime(row["datetime"]).ToString("dd/MM/yyyy HH:mm"),
                            row["username"],
                            row["receipt_ad"]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }

        
        //button1_Click กลับไปหน้า Admin
        private void button1_Click(object sender, EventArgs e)
        {
            Admin Admin = new Admin();
            Admin.Show();
            this.Hide();
        }
    }
}
