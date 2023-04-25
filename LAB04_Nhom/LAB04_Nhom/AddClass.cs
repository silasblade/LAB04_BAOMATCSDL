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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LAB04_Nhom
{
    public partial class AddClass : Form
    {
        public AddClass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=VISHAGNA;Initial Catalog=QLSVNhom;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SP_INS_CLASS", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MALOP", malop.Text);
            command.Parameters.AddWithValue("@TENLOP", tenlop.Text);
            command.Parameters.AddWithValue("@MANV", manv.Text);


            try
            {
                if (manv.Text != null || malop != null)
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm lớp thành công.");
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
