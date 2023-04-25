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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=VISHAGNA;Initial Catalog=QLSVNhom;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SP_SEL_CLASS", connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            connection.Open();
            adapter.Fill(dataTable);

            dataTable.Columns["MALOP"].ColumnName = "Mã lớp";
            dataTable.Columns["TENLOP"].ColumnName = "Tên lớp";
            dataTable.Columns["MANV"].ColumnName = "Mã nhân viên";
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
            AddClass addClass = new AddClass();
            addClass.ShowDialog();
        }
    }
}
