using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Projectร้านกะเพรา2
{
    public partial class Sales_history : Form
    {
        public Sales_history()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Admin Admin = new Admin();
            Admin.Show();
            this.Hide();
        }

        private void Sales_history_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            comboBox1.Items.AddRange(new string[] { "รายวัน", "รายเดือน", "รายปี" });
            comboBox1.SelectedIndex = 0;
            dateTimePicker1.Format = DateTimePickerFormat.Short;

            ////ดึงข้อมูลมาแสดง

            //MySqlConnection conn = databaseConnection();
            //DataSet ds = new DataSet();
            //conn.Open();

            //MySqlCommand cmd;

            //cmd = conn.CreateCommand();
            //cmd.CommandText = "SELECT * FROM history";

            //MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            //adapter.Fill(ds);

            //conn.Close();
            //dataGridView1.DataSource = ds.Tables[0].DefaultView;

            //ดึงข้อมูลมาแสดง


            //สร้างการเชื่อมต่อกับฐานข้อมูล โดยใช้ฟังก์ชัน databaseConnection()
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();  //สร้าง DataSet เพื่อใช้เก็บข้อมูลที่ดึงมาจากฐานข้อมูล
            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM history";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();

            //// ตั้งค่ารูปแบบวันที่ใน DataGridView ก่อนที่จะแสดง
            //foreach (DataColumn column in ds.Tables[0].Columns)
            //{
            //    if (column.DataType == typeof(DateTime))
            //    {
            //        // ตั้งค่าให้แสดงวันที่ในรูปแบบที่ต้องการ
            //        dataGridView1.Columns[column.ColumnName].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            //    }
            //}

            
            // กำหนด DataSource ให้กับ DataGridView
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            decimal grandTotal = 0;

            // รวมยอดจากทุกแถวใน DataTable
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (decimal.TryParse(row["total"].ToString(), out decimal total))
                {
                    grandTotal += total;
                }
            }

            // แสดงผลลัพธ์รวม ในLabel 
            label1.Text = grandTotal.ToString("N2") + " บาท";
            
        }

        //ฟังก์ชันสำหรับสร้างการเชื่อมต่อกับฐานข้อมูล
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=admin;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
       
 


      
        //เปิดไฟล์ pdf
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 11) //ตรวจสอบว่าคอลัมน์ที่ถูกคลิกคือคอลัมน์ที่ 11มั้ย
            {
                DataGridViewCell cell = null;
                foreach (DataGridViewCell selectedCell in dataGridView1.SelectedCells)
                {
                    cell = selectedCell;
                    break;
                }
                if (cell != null)
                {
                    DataGridViewRow row = cell.OwningRow;
                    string selectedId = row.Cells["receipt_ad"].Value.ToString(); //ดึงค่าจากคอลัมน์ชื่อ "receipt_ad" มาใส่ใน selectedId
                    MessageBox.Show(selectedId);


                    string file = selectedId;
                    Process.Start(file);


                }
            }
        }


       

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
    
                if (comboBox1.SelectedItem.ToString() == "รายวัน")
                {
                    dateTimePicker1.Format = DateTimePickerFormat.Short;
                    dateTimePicker1.CustomFormat = "dd/M/yyyy";
                dateTimePicker1.ShowUpDown = false;  //ให้เลือกจากปฏิทิน

            }
            else if (comboBox1.SelectedItem.ToString() == "รายเดือน")
                {
                    dateTimePicker1.Format = DateTimePickerFormat.Custom;
                    dateTimePicker1.CustomFormat = "M/yyyy";
                dateTimePicker1.ShowUpDown = true; //เลือกจากลูกศรขึ้นลง

            }
            else if (comboBox1.SelectedItem.ToString() == "รายปี")
                {
                    dateTimePicker1.Format = DateTimePickerFormat.Custom;
                    dateTimePicker1.CustomFormat = "yyyy";
                    dateTimePicker1.ShowUpDown = true;  // เพื่อเลือกปีได้สะดวก
            }
            
        }

        //ฟังก์ชันนี้ใช้รัน SQL query ที่รับมาเเล้วเอาผลลัพธ์ไปใส่ใน dataGridView1
        private void LoadHistoryWithQuery(string query)
        {
            try
            {
                MySqlConnection conn = databaseConnection();
                DataTable dt = new DataTable();
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();

                dataGridView1.DataSource = dt;

                // รวมยอดจากคอลัมน์ total
                decimal grandTotal = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (decimal.TryParse(row["total"].ToString(), out decimal value))
                    {
                        grandTotal += value;
                    }
                }

                label1.Text = grandTotal.ToString("N2") + " บาท";  //รวมยอดเงินจากทุกแถวในคอลัมน์ total
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
        }
        
        //ฟังก์ชันนี้จะทำงานเมื่อมีการเปลี่ยนวันที่ใน dateTimePicker1
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
 
                string rangeType = comboBox1.SelectedItem.ToString();
                DateTime selectedDate = dateTimePicker1.Value;
                string query = "";

                if (rangeType == "รายวัน")
                    {
                        string day = selectedDate.ToString("yyyy-M-dd");
                        query = $"SELECT * FROM history WHERE DATE(datetime) = '{day}'"; //กรองข้อมูลที่ datetime ตรงกับวันที่ที่เลือก

            }
                else if (rangeType == "รายเดือน")
                    {
                int month = selectedDate.Month;
                string year = selectedDate.ToString("yyyy"); //ดึงข้อมูลตามเดือนและปีที่เลือก

                query = $"SELECT * FROM history WHERE YEAR(datetime) = '{year}' AND MONTH(datetime) = '{month}'";

                }
                else if (rangeType == "รายปี")
                    {
                        string year = selectedDate.ToString("yyyy");
                        query = $"SELECT * FROM history WHERE YEAR(datetime) = {year}"; //แค่กรองข้อมูลเฉพาะปี
            }

                LoadHistoryWithQuery(query);
            
        }
    }
}
