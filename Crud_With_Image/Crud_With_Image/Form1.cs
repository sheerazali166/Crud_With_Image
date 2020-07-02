using Crud_With_Image.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crud_With_Image
{
    public partial class Form1 : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["csdb"].ConnectionString;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Image";
            ofd.Filter = "ImageFile (*.png;*.jpg;*.bmp;*.gif;|*.png;*.jpg;*.bmp;*.gif)";
            if (ofd.ShowDialog() == DialogResult.OK) {

                pictureBoxImage.Image = new Bitmap(ofd.FileName);
            }
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(cs);
            string query = "insert into Student values (@id, @name, @age, @picture)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", textBoxId.Text);
            cmd.Parameters.AddWithValue("@name", textBoxName.Text);
            cmd.Parameters.AddWithValue("@age", numericUpDownAge.Value);
            cmd.Parameters.AddWithValue("@picture", SavePhoto());

            conn.Open();

            int n = cmd.ExecuteNonQuery();

            if (n > 0)
            {

                MessageBox.Show("Insertion Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetControls();
                BindGridView();
            }
            else {

                MessageBox.Show("Insertion Failed.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            conn.Close();

        }

        private byte[] SavePhoto()
        {
            MemoryStream ms = new MemoryStream();
            pictureBoxImage.Image.Save(ms, pictureBoxImage.Image.RawFormat);

            return ms.GetBuffer();
        }

        public void BindGridView() {

            SqlConnection conn = new SqlConnection(cs);
            string query = "select * from Student";
            SqlDataAdapter sda = new SqlDataAdapter(query, conn);
            DataTable data = new DataTable();
            sda.Fill(data);
            dataGridViewShowData.DataSource = data;
        }

        private void buttonDisplayData_Click(object sender, EventArgs e)
        {
            BindGridView();
        }

        public void ResetControls() {

            textBoxId.Clear();
            textBoxName.Clear();
            numericUpDownAge.Value = 0;
            pictureBoxImage.Image = Resources.No_Image_Available;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        private void dataGridViewShowData_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBoxId.Text = dataGridViewShowData.SelectedRows[0].Cells[0].Value.ToString();
            textBoxName.Text = dataGridViewShowData.SelectedRows[0].Cells[1].Value.ToString();
            numericUpDownAge.Value = Convert.ToInt32(dataGridViewShowData.SelectedRows[0].Cells[2].Value);

            pictureBoxImage.Image = GetPhot((byte[])dataGridViewShowData.SelectedRows[0].Cells[3].Value);
        }

        private Image GetPhot(byte[] photo)
        {
            MemoryStream ms = new MemoryStream(photo);

            return Image.FromStream(ms);
            
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(cs);
            string query = "update Student set Name = @name, Age = @age, Picture = @picture where Id = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", textBoxId.Text);
            cmd.Parameters.AddWithValue("@name", textBoxName.Text);
            cmd.Parameters.AddWithValue("@age", numericUpDownAge.Value);
            cmd.Parameters.AddWithValue("@picture", SavePhoto());

            conn.Open();

            int n = cmd.ExecuteNonQuery();

            if (n > 0)
            {

                MessageBox.Show("Updation Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetControls();
                BindGridView();
            }
            else
            {

                MessageBox.Show("Updation Failed.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            conn.Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(cs);
            string query = "delete from Student where Id = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", textBoxId.Text);

            conn.Open();

            int n = cmd.ExecuteNonQuery();

            if (n > 0)
            {

                MessageBox.Show("Deletion Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetControls();
                BindGridView();
            }
            else
            {

                MessageBox.Show("Deletion Failed.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            conn.Close();
        }
    }
}
