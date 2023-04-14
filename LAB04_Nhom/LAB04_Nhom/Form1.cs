using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB04_Nhom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string manv;
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 Form2 = new Form2();
            string tb1 = textBox1.Text;
            string tb2 = textBox2.Text;
            string connectionString = "Data Source=VISHAGNA;Initial Catalog=QLSVNhom;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(" Select * From dbo.NHANVIEN WHERE MATKHAU = HASHBYTES('SHA1', '" + tb2 + "') AND MANV = '" + tb1 + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                connection.Close();
                manv = tb1;
                Form2.Show();
                this.Hide();
            }
            else
                {
                    connection.Close();
                    MessageBox.Show("Không tồn tại");
                }
            }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    }

