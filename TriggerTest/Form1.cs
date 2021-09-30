using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TriggerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-OC5036T\MSSQLSERVER1;Initial Catalog=DbBook;Integrated Security=True");
        void list()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select ID as 'Kayıt Numarası',Ad,Yazar,Sayfa,Yayinevi as 'Yayınevi',Tur as 'Kategorisi' from TblKitaplar", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }
        void bookstok()
        {
            connection.Open();
            SqlCommand command5 = new SqlCommand("select * from TblSayac", connection);
            SqlDataReader reader = command5.ExecuteReader();
            while (reader.Read())
            {
                label7.Text = reader[0].ToString();
            }
            connection.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            list();
            bookstok();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TxtBookName.Text.Trim() != "" && TxtBookWriter.Text.Trim() != "" && TxtBookPublisher.Text.Trim() != "" && TxtBookPageNum.Text.Trim() != "" && TxtBookCategory.Text.Trim() != "")
            {
                connection.Open();
                SqlCommand command = new SqlCommand("insert into TblKitaplar (Ad,Yazar,Sayfa,Yayinevi,Tur) values (@p1,@p2,@p3,@p4,@p5)", connection);
                command.Parameters.AddWithValue("@p1", TxtBookName.Text.Trim());
                command.Parameters.AddWithValue("@p2", TxtBookWriter.Text.Trim());
                command.Parameters.AddWithValue("@p3", TxtBookPageNum.Text.Trim());
                command.Parameters.AddWithValue("@p4", TxtBookPublisher.Text.Trim());
                command.Parameters.AddWithValue("@p5", TxtBookCategory.Text.Trim());
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Kayıt Başarılı");
                list();
                bookstok();
            }
            else
            {
                MessageBox.Show("Lütfen Bilgileri Eksiksiz Doldurunuz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (TxtBookName.Text.Trim() != "" && TxtBookWriter.Text.Trim() != "" && TxtBookPublisher.Text.Trim() != "" && TxtBookPageNum.Text.Trim() != "" && TxtBookCategory.Text.Trim() != "" && lblid.Text != "0")
            {
                connection.Open();
                SqlCommand command1 = new SqlCommand("update TblKitaplar set Ad=@p1,Yazar=@p2,Sayfa=@p3,Yayinevi=@p4,Tur=@p5 where ID=@p6", connection);
                command1.Parameters.AddWithValue("@p6", lblid.Text);
                command1.Parameters.AddWithValue("@p1", TxtBookName.Text.Trim());
                command1.Parameters.AddWithValue("@p2", TxtBookWriter.Text.Trim());
                command1.Parameters.AddWithValue("@p3", TxtBookPageNum.Text.Trim());
                command1.Parameters.AddWithValue("@p4", TxtBookPublisher.Text.Trim());
                command1.Parameters.AddWithValue("@p5", TxtBookCategory.Text.Trim());
                command1.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Güncelleme Başarılı");
                list();
                bookstok();
            }
            else
            {
                MessageBox.Show("Lütfen Bilgileri Eksiksiz Doldurunuz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (lblid.Text != "0")
            {
                DialogResult result = MessageBox.Show("Kitabı Silmek İstediğinize Emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (result == DialogResult.Yes)
                {
                    connection.Open();
                    SqlCommand command2 = new SqlCommand("delete from TblKitaplar where ID=" + lblid.Text, connection);
                    command2.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Kitap Silindi");
                    list();
                    bookstok();
                }

            }
            else
            {
                MessageBox.Show("Lütfen Silmek İstediğiniz Kitabı Seçiniz");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblid.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            TxtBookName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            TxtBookWriter.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            TxtBookPageNum.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            TxtBookPublisher.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            TxtBookCategory.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
        }

        private void TxtBookName_TextChanged(object sender, EventArgs e)
        {
            SqlDataAdapter adapter1 = new SqlDataAdapter("select ID as 'Kayıt Numarası',Ad,Yazar,Sayfa,Yayinevi as 'Yayınevi',Tur as 'Kategorisi' from TblKitaplar where Ad like '%" + TxtBookName.Text+"%'", connection);
            DataTable table1 = new DataTable();
            adapter1.Fill(table1);
            dataGridView1.DataSource = table1;
        }

        private void TxtBookWriter_TextChanged(object sender, EventArgs e)
        {
            SqlDataAdapter adapter2 = new SqlDataAdapter("Select ID as 'Kayıt Numarası',Ad,Yazar,Sayfa,Yayinevi as 'Yayınevi',Tur as 'Kategorisi' from TblKitaplar where Yazar like '%" + TxtBookWriter.Text + "%'", connection);
            DataTable table2 = new DataTable();
            adapter2.Fill(table2);
            dataGridView1.DataSource = table2;
        }

        private void TxtBookCategory_TextChanged(object sender, EventArgs e)
        {
            SqlDataAdapter adapter3 = new SqlDataAdapter("Select ID as 'Kayıt Numarası',Ad,Yazar,Sayfa,Yayinevi as 'Yayınevi',Tur as 'Kategorisi' from TblKitaplar where Tur like '%" + TxtBookCategory.Text + "%'", connection);
            DataTable table3 = new DataTable();
            adapter3.Fill(table3);
            dataGridView1.DataSource = table3;
        }

        private void TxtBookPublisher_TextChanged(object sender, EventArgs e)
        {
            SqlDataAdapter adapter4 = new SqlDataAdapter("Select ID as 'Kayıt Numarası',Ad,Yazar,Sayfa,Yayinevi as 'Yayınevi',Tur as 'Kategorisi' from TblKitaplar where Yayinevi like '%" + TxtBookPublisher.Text + "%'", connection);
            DataTable table4 = new DataTable();
            adapter4.Fill(table4);
            dataGridView1.DataSource = table4;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
