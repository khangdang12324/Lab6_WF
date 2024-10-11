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

namespace Lab6_2212387
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string connectString = @"Data Source=PC743;Initial Catalog=RestaurantManagement;Integrated Security=True;";
            SqlConnection sqlConnection = new SqlConnection(connectString);

            SqlCommand command = sqlConnection.CreateCommand();

            string query = "SELECT ID, Name, Type FROM Category";
            command.CommandText = query;
            sqlConnection.Open();

            SqlDataReader sqlDataReader = command.ExecuteReader();

            this.DisplayCategory(sqlDataReader);

            sqlConnection.Close();
        }

        private void DisplayCategory(SqlDataReader reader)
        {
            lvCatagory.Items.Clear();
            while (reader.Read())
            {
                ListViewItem item = new ListViewItem(reader["ID"].ToString());

                lvCatagory.Items.Add(item);

                item.SubItems.Add(reader["Name"].ToString());
                item.SubItems.Add(reader["Type"].ToString());
            }
        }

        private void btnADD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtType.Text))
            {
                MessageBox.Show("Vui lòng nhập tên và loại.");
                return;
            }

            string connectString = @"Data Source=PC743;Initial Catalog=RestaurantManagement;Integrated Security=True;";

            using (SqlConnection sqlConnection = new SqlConnection(connectString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlcommand = sqlConnection.CreateCommand())
                {
                    sqlcommand.CommandText = "INSERT INTO Category(Name, [Type]) VALUES (@Name, @Type)";
                    sqlcommand.Parameters.AddWithValue("@Name", txtName.Text);
                    sqlcommand.Parameters.AddWithValue("@Type", txtType.Text);

                    try
                    {
                        int numOfRowsEffected = sqlcommand.ExecuteNonQuery();

                        if (numOfRowsEffected == 1)
                        {
                            MessageBox.Show("Thêm nhom món thành công");
                            btnLoad.PerformClick();
                            txtName.Text = "";
                            txtType.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Da co loi xay ra. Vui long thu lai!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
            }
        }

        private void lvCatagory_Click(object sender, EventArgs e)
        {
            ListViewItem item = lvCatagory.SelectedItems[0];

            txtID.Text = item.Text;
            txtName.Text = item.SubItems[1].Text;
            txtType.Text = item.SubItems[1].Text == "0" ? "Thức uống" : "Đồ ăn";


            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string connectString = @"Data Source=PC743;Initial Catalog=RestaurantManagement;Integrated Security=True;";
            SqlConnection sqlConnection = new SqlConnection(connectString);

            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            sqlCommand.CommandText = "UPDATE Category SET Name = N'" + txtName.Text +
                "', [Type] = " + txtType.Text + "WHERE ID = " + txtID.Text;

            sqlConnection.Open();

            int numOfRowsEffected = sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();

            if ( numOfRowsEffected == 0)
            {
                ListViewItem item = lvCatagory.SelectedItems[0];

                item.SubItems[1].Text = txtName.Text;
                item.SubItems[2].Text = txtType.Text;

                txtID.Text = string.Empty; txtName.Text = string.Empty;txtType.Text = string.Empty;


                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                MessageBox.Show("Cap nhat nhom mon an thanh cong");
            }
            else
            {
                MessageBox.Show("DA co loi");
            }
        }
    }

}
