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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data.SqlTypes;

namespace LAB04_Nhom
{
    public partial class AddNV : Form
    {
        public AddNV()
        {
            InitializeComponent();
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
            File.WriteAllText(nameprivatekey +".txt", privateKeyXml);
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

        public string Decrypt(string privateKeyFile, string encryptedBase64)
        {
            // Đọc khóa private từ file
            string privateKeyXml = File.ReadAllText(privateKeyFile);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(512);
            rsa.FromXmlString(privateKeyXml);

            // Chuyển Base64 sang byte array
            byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);

            // Giải mã chuỗi sử dụng khóa private
            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, false);
            string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedText;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //Mã hóa matkhau
            string mk = matkhau.Text;
            byte[] mkBytes = Encoding.UTF8.GetBytes(mk);
            SHA1 sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(mkBytes);
            String mkh = BitConverter.ToString(hashBytes).Replace("-", "");

            //
            CreateKey(manv.Text, manv.Text+"2");
            string publicKeyFile = manv.Text + ".txt";
            string lg = Encrypt(publicKeyFile, luong.Text);
            string connectionString = "Data Source=VISHAGNA;Initial Catalog=QLSVNhom;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SP_INS_PUBLIC_ENCRYPT_NHANVIEN ", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MANV", manv.Text);
            command.Parameters.AddWithValue("@HOTEN", hoten.Text);
            command.Parameters.AddWithValue("@EMAIL", email.Text);
            command.Parameters.AddWithValue("@LUONG", lg);
            command.Parameters.AddWithValue("@TENDN", username.Text);
            command.Parameters.AddWithValue("@MK", mkh);
            command.Parameters.AddWithValue("@PUBKEY", manv.Text);

            try
            {
                if (manv.Text != null)
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm nhân viên thành công.");
                }
                else
                {
                    MessageBox.Show("Không thực hiện được.");
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thực hiện được." +  ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
