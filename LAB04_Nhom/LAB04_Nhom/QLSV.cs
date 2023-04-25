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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LAB04_Nhom
{
    public partial class QLSV : Form
    {
        public QLSV()
        {
            InitializeComponent();
        }


        private void QLSV_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
         //Load thông tin nhân viên và lớp
            string connectionString = "Data Source=VISHAGNA;Initial Catalog=QLSVNhom;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            string ml = null;

            try
            {
                SqlCommand command1 = new SqlCommand("SELECT HOTEN FROM NHANVIEN WHERE MANV = N'" + Form1.manv + "'", connection);
                connection.Open();
                String result1 = command1.ExecuteScalar().ToString();
                connection.Close();
                SqlCommand command2 = new SqlCommand("SELECT MALOP FROM LOP WHERE MANV = N'" + Form1.manv + "'", connection);
                connection.Open();
                String result2 = command2.ExecuteScalar().ToString();
                connection.Close();
                SqlCommand command3 = new SqlCommand("SELECT TENLOP FROM LOP WHERE MANV = N'" + Form1.manv + "'", connection);
                connection.Open();
                String result3 = command3.ExecuteScalar().ToString();
                connection.Close();


                textBox1.Text = "Mã nhân viên:" + Form1.manv;
                textBox2.Text = "Tên nhân viên:" + result1;
                textBox3.Text = "Mã lớp: " + result2;
                textBox4.Text = "Tên lớp: " + result3;
                ml = result2;
            }
            catch
            {
                MessageBox.Show("Nhân viên này không có lớp.");

            }
            //Load danh sách sinh viên
            SqlCommand command = new SqlCommand("SP_SEL_LOPSV", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MALOP", ml);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            connection.Open();
            adapter.Fill(dataTable);

            dataTable.Columns["MASV"].ColumnName = "Mã sinh viên";
            dataTable.Columns["HOTEN"].ColumnName = "Họ Tên";
            dataTable.Columns["NGAYSINH"].ColumnName = "Ngày sinh";
            dataTable.Columns["DIACHI"].ColumnName = "Địa chỉ";
            dataTable.Columns["TENDN"].ColumnName = "Tên đăng nhập";
            connection.Close();

            dataGridView1.DataSource = dataTable;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


        }

        private void button5_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddSV addSV = new AddSV();
            addSV.ShowDialog();
        }
    }
}
