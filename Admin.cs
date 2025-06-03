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
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }


        /// เมื่อกดปุ่ม button5 จะเปิดฟอร์ม Home_pasg และซ่อนฟอร์มปัจจุบัน (Admin)
        private void button5_Click(object sender, EventArgs e)
        {
            Home_pasg Home_pasg = new Home_pasg();
            Home_pasg.Show();
            this.Hide();
        }

        ///เมื่อกดปุ่ม button2 จะเปิดฟอร์ม Sales_history และซ่อนฟอร์ม Admin
        private void button2_Click(object sender, EventArgs e)
        {
            Sales_history Sales_history = new Sales_history();
            Sales_history.Show();
            this.Hide();
        }

        //เปิดฟอร์ม Cust(ซึ่งอาจแสดงข้อมูลลูกค้า) แล้วซ่อนฟอร์ม Admin
        private void button3_Click(object sender, EventArgs e)
        {
            Cust Cust = new Cust();
            Cust.Show();
            this.Hide();
        }

        //เปิดฟอร์ม Product_management สำหรับจัดการสินค้าต่าง ๆ แล้วซ่อนฟอร์ม Admin
        private void button1_Click(object sender, EventArgs e)
        {
            Product_management Product_management = new Product_management();
            Product_management.Show();
            this.Hide();
        }

        //เปิดฟอร์ม CustomerCart(ตะกร้าสินค้า) และซ่อนฟอร์ม Admin
        private void button4_Click(object sender, EventArgs e)
        {
            CustomerCart CustomerCart = new CustomerCart();
            CustomerCart.Show();
            this.Hide();
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }
    }
}
