using MySql.Data.MySqlClient;
using Saladpuk.PromptPay.Facades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing.Common;
using ZXing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Font = iTextSharp.text.Font;
using iTextSharp.text.pdf.draw;

namespace Projectร้านกะเพรา2
{
    public partial class QRnReceipt : Form
    {
        private string _usernamee;
        public string _username
        {
            get { return _username; }
            set { _username = value; }
        }

        public QRnReceipt(string username)
        {
            InitializeComponent();
            _usernamee = username; // เก็บค่า username ที่ส่งมา
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // สร้างอ็อบเจกต์ของ Form1 (หน้าแรก)
            Home_pasg form1 = new Home_pasg();

            // แสดงหน้าแรก (Form1)
            form1.Show();

            // ปิดฟอร์มปัจจุบัน (เช่น Form2 หรือ Form3)
            this.Close();
        }


        string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss"); // ใช้ _ แทนที่ :



        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=admin;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        //เก็บยอดรวม
        decimal totalAmount = 0;
        decimal discountAmount = 0;

        private void UpdateTotalLabel()
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    conn.Open();
                    string query = "SELECT total, discount FROM carts"; //ดึงราคามาจาก cast
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalAmount = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                            discountAmount = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                            decimal afterDiscount = totalAmount - discountAmount;

                            labelTotalAmount.Text = afterDiscount.ToString("N2"); // แสดงผลในรูปแบบ 2 ตำแหน่งทศนิยม
                        }
                        else
                        {
                            MessageBox.Show("ไม่พบข้อมูลใน carts table.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void buttonreceipt_Click(object sender, EventArgs e)
        {
            DialogResult re = MessageBox.Show("ยืนยันที่จะออกใบเสร็จหรือไม่", "ยืนยัน", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                try
                {
                    using (MySqlConnection conn = databaseConnection())
                    {
                        conn.Open();

                        // ประกาศตำแหน่งไฟล์ PDF ที่จะสร้าง สร้างเอกสาร PDF โดยใช้ iTextSharp
                        string pdfFilePath = @"C:\Users\User\Desktop\c#\bill" + currentDateTime + ".pdf";
                        

                        Document document = new Document(PageSize.A4, 30, 30, 40, 40);

                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(@"C:\Users\User\Desktop\c#\bill" + currentDateTime + ".pdf", FileMode.Create));
                        document.Open();
                        // ระบุตำแหน่งของฟอนต์ภาษาไทยในระบบของคุณ
                        string thaiFontPath = @"C:\Windows\Fonts\TAHOMA.TTF";

                        // โหลดฟอนต์ภาษาไทยและสร้าง BaseFont
                        BaseFont baseFont = BaseFont.CreateFont(thaiFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                        // ดึงข้อมูลจาก carts เพื่อเตรียมลดจำนวนสต้อก
                        string selectQuery = "SELECT id, qty FROM carts";
                        MySqlCommand selectCommand = new MySqlCommand(selectQuery, conn);
                        MySqlDataReader reader = selectCommand.ExecuteReader();

                        Dictionary<int, int> cartItems = new Dictionary<int, int>();

                        while (reader.Read())
                        {
                            int productId = reader.GetInt32("id");
                            int quantity = reader.GetInt32("qty");
                            cartItems[productId] = quantity;
                        }

                        reader.Close();

                        // ลดจำนวนสินค้าในตาราง product ตามที่อยู่ใน carts
                        foreach (var item in cartItems)
                        {
                            string updateQuery = "UPDATE product SET quantity = quantity - @quantity WHERE id = @id";
                            MySqlCommand updateCommand = new MySqlCommand(updateQuery, conn);
                            updateCommand.Parameters.AddWithValue("@quantity", item.Value);
                            updateCommand.Parameters.AddWithValue("@id", item.Key);
                            updateCommand.ExecuteNonQuery();
                        }

                        // สร้างออบเจ็กต์ Font จาก BaseFont
                        Font thaiFont = new Font(baseFont, 16);
                        Paragraph paragraph = new Paragraph("123 หมู่ที่ 16 ถ.มิตรภาพ ตำบลในเมือง\n อำเภอเมืองขอนแก่น ขอนแก่น 40002\n ", thaiFont);
                        paragraph.Alignment = Element.ALIGN_CENTER;
                        paragraph.SpacingAfter = 5;
                        paragraph.SpacingBefore = 5;
                        document.Add(paragraph);

                        // แสดงชื่อผู้ใช้ด้านล่างที่อยู่
                        Paragraph userParagraph = new Paragraph("ลูกค้า : " + _usernamee, thaiFont);
                        userParagraph.Alignment = Element.ALIGN_CENTER;
                        userParagraph.SpacingAfter = 10;
                        document.Add(userParagraph);


                        // ดึงข้อมูลจาก carts เพื่อแสดงใน PDF
                        string selectCartsQuery = "SELECT name, qty, price, totalprice FROM carts";
                        MySqlCommand selectCartsCommand = new MySqlCommand(selectCartsQuery, conn);
                        MySqlDataReader cartsReader = selectCartsCommand.ExecuteReader();

                        // สร้างตารางสำหรับข้อมูลสินค้า
                        PdfPTable table = new PdfPTable(4);
                        table.WidthPercentage = 100;
                        table.SetWidths(new float[] { 20, 15, 15, 15 });

                        // เพิ่มหัวตาราง
                        PdfPCell cell;
                        cell = new PdfPCell(new Phrase("สินค้า", thaiFont));
                        cell.BorderWidthTop = 1f; // ความหนาของเส้นขอบด้านบน
                        cell.BorderWidthBottom = 1f; // ความหนาของเส้นขอบด้านล่าง
                        cell.BorderWidthLeft = 0f; // ไม่มีเส้นขอบด้านซ้าย
                        cell.BorderWidthRight = 0f; // ไม่มีเส้นขอบด้านขวา
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("จำนวน", thaiFont));
                        cell.BorderWidthTop = 1f;
                        cell.BorderWidthBottom = 1f;
                        cell.BorderWidthLeft = 0f;
                        cell.BorderWidthRight = 0f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("ราคา", thaiFont));
                        cell.BorderWidthTop = 1f;
                        cell.BorderWidthBottom = 1f;
                        cell.BorderWidthLeft = 0f;
                        cell.BorderWidthRight = 0f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("ราคารวม", thaiFont));
                        cell.BorderWidthTop = 1f;
                        cell.BorderWidthBottom = 1f;
                        cell.BorderWidthLeft = 0f;
                        cell.BorderWidthRight = 0f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);


                        while (cartsReader.Read()) // ดึงข้อมูลจาก carts
                        {
                            cell = new PdfPCell(new Phrase(cartsReader["name"].ToString(), thaiFont));
                            cell.Border = PdfPCell.NO_BORDER;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(cartsReader["qty"].ToString(), thaiFont));
                            cell.Border = PdfPCell.NO_BORDER;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(cartsReader["price"].ToString(), thaiFont));
                            cell.Border = PdfPCell.NO_BORDER;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);

                            cell = new PdfPCell(new Phrase(cartsReader["totalprice"].ToString(), thaiFont));
                            cell.Border = PdfPCell.NO_BORDER;
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            table.AddCell(cell);
                        }
                        cartsReader.Close();

                        // เพิ่มตารางลงในเอกสาร PDF
                        document.Add(table);

                        // ดึงข้อมูลยอดรวม ภาษี และยอดรวมทั้งสิ้นจาก carts
                        string selectTotalsQuery = "SELECT subtotal, vat, total FROM carts LIMIT 1";
                        MySqlCommand selectTotalsCommand = new MySqlCommand(selectTotalsQuery, conn);
                        MySqlDataReader totalsReader = selectTotalsCommand.ExecuteReader();

                        
                        decimal subtotal = 0;
                        decimal vat = 0;
                        decimal grandTotal = 0;

                        if (totalsReader.Read())
                        {
                            subtotal = decimal.Parse(totalsReader["subtotal"].ToString());

                            // คำนวณ VAT เอง โดยไม่ปัดขึ้น
                            vat = Math.Floor(subtotal * 0.07m * 100) / 100m;

                            // คำนวณยอดรวมสุทธิ
                            grandTotal = subtotal - vat;
                        }
                        totalsReader.Close();
                        decimal subtotal1 = grandTotal + vat;
                        
                        float rightIndent = 60f; // กำหนดระยะขอบด้านขวา
                                                 // เพิ่มเส้นขีดก่อนแถวของยอดรวม
                        LineSeparator line = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -2);
                        document.Add(new Chunk(line));
                        // เพิ่มยอดรวม  ภาษี  และยอดรวมทั้งสิ้น โดยไม่ทำเป็นตาราง
                        Paragraph subtotalParagraph = new Paragraph("ยอดรวม(ไม่รวม vat):    " + grandTotal.ToString("N2"), thaiFont);
                        subtotalParagraph.Alignment = Element.ALIGN_RIGHT;
                        subtotalParagraph.IndentationRight = rightIndent;
                        document.Add(subtotalParagraph);

                        Paragraph vatParagraph = new Paragraph("ภาษี:    " + vat.ToString("N2"), thaiFont);
                        vatParagraph.Alignment = Element.ALIGN_RIGHT;
                        vatParagraph.IndentationRight = rightIndent;
                        document.Add(vatParagraph);

                        Paragraph grandTotalParagraph = new Paragraph("ยอดรวมทั้งสิ้น:    " + subtotal1.ToString("N2"), thaiFont);
                        grandTotalParagraph.Alignment = Element.ALIGN_RIGHT;
                        grandTotalParagraph.IndentationRight = rightIndent;
                        document.Add(grandTotalParagraph);
                        // ปิดเอกสาร PDF
                        document.Close();

                        System.Diagnostics.Process.Start(pdfFilePath);

                     
                        // คัดลอกข้อมูลจาก carts ไปยัง history พร้อมเพิ่มวันที่และเวลาในรูปแบบ DD - MM - YYYY HH:MM:SS
                        string copyQuery = "INSERT INTO history (id, name, qty, price, totalprice, subtotal, vat, total, datetime, username, receipt_ad)" +
                                            "SELECT id, name, qty, price, totalprice, subtotal, vat, total, NOW(), @username, @receipt_ad FROM carts";
                        MySqlCommand copyCommand = new MySqlCommand(copyQuery, conn);
                        copyCommand.Parameters.AddWithValue("@datetime", currentDateTime); // ส่งค่าของ datetime
                        copyCommand.Parameters.AddWithValue("@username", _usernamee);
                        copyCommand.Parameters.AddWithValue("@receipt_ad", pdfFilePath);

                        copyCommand.ExecuteNonQuery();

                        // ลบข้อมูลจากตาราง carts หลังจากคัดลอกไปยัง history
                        string deleteQuery = "DELETE FROM carts";
                        MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, conn);
                        deleteCommand.ExecuteNonQuery();


                        Home_pasg Home_pasg = new Home_pasg();
                        Home_pasg.Show();
                        this.Close();

                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void QRnReceipt_Load(object sender, EventArgs e)
        {
            UpdateTotalLabel(); // เมื่อฟอร์มถูกโหลดขึ้นมา ให้ดึงยอดรวมและแสดงผลใน Label
            //ดึงยอด สร้าง QR 
            double total = (double)totalAmount;
            string qr = PPay.DynamicQR.MobileNumber("0650071439").Amount(total).CreateCreditTransferQrCode();
            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = 326,
                    Height = 316,
                    PureBarcode = true
                }
            };
            Bitmap barCodeBitmap = barcodeWriter.Write(qr);
            pictureBox1.Image = barCodeBitmap;
        }

    }
}
