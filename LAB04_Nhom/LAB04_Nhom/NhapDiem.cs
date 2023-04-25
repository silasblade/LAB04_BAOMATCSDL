using System;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Input;

namespace LAB04_Nhom
{
    public partial class NhapDiem : Form
    {
        public NhapDiem()
        {
            InitializeComponent();
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
            string hexString = BitConverter.ToString(encryptedBytes).Replace("-", "");
            return hexString;
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

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=VISHAGNA;Initial Catalog=QLSVNhom;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SP_SEL_DIEM", connection);
            command.Parameters.AddWithValue("@MASV", textBox1.Text);

            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            connection.Open();
            adapter.Fill(dataTable);

            dataTable.Columns["MASV"].ColumnName = "Mã sinh viên";
            dataTable.Columns["MAHP"].ColumnName = "Mã học phần";
            dataTable.Columns["DIEMTHI"].ColumnName = "Điểm thi";
            foreach (DataRow row in dataTable.Rows)
            {
                string privateKeyFile = Form1.manv + "2.txt";
                string encryptedValue = row["Điểm thi"].ToString();
                string decryptedValue = Decrypt(privateKeyFile, encryptedValue); // Hàm Decrypt là hàm giải mã của bạn
                row["Điểm thi"] = decryptedValue;
            }
            connection.Close();

            dataGridView1.DataSource = dataTable;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string publicKeyFile = Form1.manv + ".txt";
            string diem = Encrypt(publicKeyFile, textBox3.Text);
            string connectionString = "Data Source=VISHAGNA;Initial Catalog=QLSVNhom;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SP_INS_NHAPDIEM", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MASV", textBox1.Text);
            command.Parameters.AddWithValue("@MAHP", textBox2.Text);
            command.Parameters.AddWithValue("@DIEMTHI", diem);

            try
            {
                if (textBox1.Text != null && textBox2.Text != null)
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm bảng điểm thành công.");
                }
                else
                {
                    MessageBox.Show("Không thực hiện được.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thực hiện được." + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string aa = Encrypt("NV01.txt", "10");
            MessageBox.Show(aa);
            string aaa = Decrypt("NV012.txt", aa);
            MessageBox.Show(aaa);
        }
    }
    }

