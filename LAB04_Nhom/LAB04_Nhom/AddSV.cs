using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LAB04_Nhom
{
    public partial class AddSV : Form
    {
        public AddSV()
        {
            InitializeComponent();
            AddSV_Load(null, EventArgs.Empty);
            ngaysinh_Leave(null, EventArgs.Empty );
        }

        private void AddSV_Load(object sender, EventArgs e)
        {
            ngaysinh.Text = "mm/dd/yyyy";
            // Thiết lập các giá trị mặc định cho các TextBox khác
        }
        private void ngaysinh_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ngaysinh.Text))
            {
                ngaysinh.Text = "mm/dd/yyyy";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Mã hóa matkhau
            string mk = matkhau.Text;
            byte[] mkBytes = Encoding.UTF8.GetBytes(mk);
            MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(mkBytes);
            String mkh = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            //
            string connectionString = "Data Source=VISHAGNA;Initial Catalog=QLSVNhom;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SP_INS_ENCRYPT_SINHVIEN", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MASV", masv.Text);
            command.Parameters.AddWithValue("@HOTEN", hoten.Text);
            command.Parameters.AddWithValue("@NGAYSINH", ngaysinh.Text);
            command.Parameters.AddWithValue("@DIACHI", diachi.Text);
            command.Parameters.AddWithValue("@MALOP", malop.Text);
            command.Parameters.AddWithValue("@TENDN", username.Text);
            command.Parameters.AddWithValue("@MATKHAU", mkh);

            try
            {
                if (masv.Text != null)
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm sinh viên thành công.");
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

        

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
