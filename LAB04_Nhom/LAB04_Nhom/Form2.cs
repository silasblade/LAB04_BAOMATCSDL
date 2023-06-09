﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB04_Nhom
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        public void CreateKey(string namepublickey, string nameprivatekey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(512);

            // Lấy khóa công khai (public key)
            string publicKeyXml = rsa.ToXmlString(false);

            // Lưu khóa công khai vào file 1
            File.WriteAllText(namepublickey + ".txt", publicKeyXml);

            // Lưu khóa giải mã vào file 2
            string privateKeyXml = rsa.ToXmlString(true);
            File.WriteAllText(nameprivatekey + ".txt", privateKeyXml);
        }

        public string Encrypt(string publicKeyFile, string plainText)
        {
            // Đọc khóa public từ file
            string publicKeyXml = File.ReadAllText(publicKeyFile);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(512);
            rsa.FromXmlString(publicKeyXml);

            // Mã hóa chuỗi sử dụng khóa public
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = rsa.Encrypt(plainBytes, false);

            // Chuyển byte array sang Base64 để lưu trữ hoặc truyền đi
            string hexstring = BitConverter.ToString(encryptedBytes).Replace("-", "");
            return hexstring;
        }

        public string Decrypt(string privateKeyFile, string encryptedHex)
        {
            // Đọc khóa private từ file
            string privateKeyXml = File.ReadAllText(privateKeyFile);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(512);
            rsa.FromXmlString(privateKeyXml);
        // Chuyển hex sang byte array
        byte[] encryptedBytes = new byte[encryptedHex.Length / 2];
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                encryptedBytes[i] = Convert.ToByte(encryptedHex.Substring(i * 2, 2), 16);
            }

            // Giải mã chuỗi sử dụng khóa private
            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);
            string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
            return decryptedText;
        }






        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=VISHAGNA;Initial Catalog=QLSVNhom;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SP_SEL_ENCRYPT_NHANVIEN", connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            connection.Open();
            adapter.Fill(dataTable);

            dataTable.Columns["MANV"].ColumnName = "Mã Nhân Viên";
            dataTable.Columns["HOTEN"].ColumnName = "Họ Tên";
            dataTable.Columns["LUONG"].ColumnName = "Lương";

            foreach (DataRow row in dataTable.Rows)
            {
                string privateKeyFile = row["Mã Nhân Viên"] + "2.txt";
                string encryptedValue = row["Lương"].ToString();
                string decryptedValue = Decrypt(privateKeyFile, encryptedValue); // Hàm Decrypt là hàm giải mã của bạn
                row["Lương"] = decryptedValue;
            }

            connection.Close();

            dataGridView1.DataSource = dataTable;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddNV addNV = new AddNV();
            addNV.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
     
        }
    }
}
