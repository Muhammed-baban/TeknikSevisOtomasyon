using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace Muhammed1
{
    public partial class YoneticiPanelForm : Form
    {

        private string adSoyad;
        private SqlConnection connection;
        private string connectionString=(@"Server=DESKTOP-R9C7EV4; Initial Catalog=teknik_servis; Integrated Security=true;");


        public YoneticiPanelForm(string adSoyad)
            {
                InitializeComponent();
                this.adSoyad = adSoyad;
                InitializeDatabaseConnection();
                LoadStokData();
                LoadPersonelData();
                LoadPozisyonData();
                label1.Text = "Giriş Saati: " + DateTime.Now.ToString() + " - Kullanıcı Adı Soyadı: " + adSoyad;
            FillDataGridMusteri();
            FillDataGridAriza();
        }

        private void InitializeDatabaseConnection()
        {
            connection = new SqlConnection(connectionString);
        }

        private void LoadPersonelData()
        {
            string query = "SELECT * FROM personel";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dataGridViewPersonel.DataSource = dataTable;
        }

        private void LoadPozisyonData()
        {
            
        }

        private void LoadStokData()
        {
            string query = "SELECT * FROM Stok";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dataGridViewStok.DataSource = dataTable;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string barkod = textBoxBarkod.Text;
            string parcaAdi = textBoxParcaAdi.Text;
            int stokAdedi = Convert.ToInt32(textBoxStokAdedi.Text);
            string tedarikci = textBoxTedarikci.Text;
            decimal alisFiyati = Convert.ToDecimal(textBoxAlisFiyati.Text);
            decimal satisFiyati = Convert.ToDecimal(textBoxSatisFiyati.Text);

            if (alisFiyati > satisFiyati)
            {
                MessageBox.Show("Alış fiyatı satış fiyatından fazla olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string insertQuery = "INSERT INTO stok (Barkod, ParcaAdi, StokAdedi, Tedarikci, AlisFiyati, SatisFiyati) " +
                "VALUES (@Barkod, @ParcaAdi, @StokAdedi, @Tedarikci, @AlisFiyati, @SatisFiyati)";
            SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@Barkod", barkod);
            insertCommand.Parameters.AddWithValue("@ParcaAdi", parcaAdi);
            insertCommand.Parameters.AddWithValue("@StokAdedi", stokAdedi);
            insertCommand.Parameters.AddWithValue("@Tedarikci", tedarikci);
            insertCommand.Parameters.AddWithValue("@AlisFiyati", alisFiyati);
            insertCommand.Parameters.AddWithValue("@SatisFiyati", satisFiyati);

            connection.Open();
            insertCommand.ExecuteNonQuery();
            connection.Close();

            LoadStokData();
            ClearTextBoxes();
        }

        private void dataGridViewStok_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridViewStok.Rows[e.RowIndex];
                textBoxBarkod.Text = row.Cells["BarkodNo"].Value.ToString();
                textBoxParcaAdi.Text = row.Cells["ParcaAdi"].Value.ToString();
                textBoxStokAdedi.Text = row.Cells["StokAdedi"].Value.ToString();
                textBoxTedarikci.Text = row.Cells["Tedarikci"].Value.ToString();
                textBoxAlisFiyati.Text = row.Cells["AlisFiyati"].Value.ToString();
                textBoxSatisFiyati.Text = row.Cells["SatisFiyati"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string barkod = textBoxBarkod.Text;
            string parcaAdi = textBoxParcaAdi.Text;
            int stokAdedi = Convert.ToInt32(textBoxStokAdedi.Text);
            string tedarikci = textBoxTedarikci.Text;
            decimal alisFiyati = Convert.ToDecimal(textBoxAlisFiyati.Text);
            decimal satisFiyati = Convert.ToDecimal(textBoxSatisFiyati.Text);

            if (alisFiyati > satisFiyati)
            {
                MessageBox.Show("Alış fiyatı satış fiyatından fazla olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string updateQuery = "UPDATE stok SET ParcaAdi = @ParcaAdi, StokAdedi = @StokAdedi, Tedarikci = @Tedarikci, AlisFiyati = @AlisFiyati, SatisFiyati = @SatisFiyati " +
                "WHERE Barkod = @Barkod";
            SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@ParcaAdi", parcaAdi);
            updateCommand.Parameters.AddWithValue("@StokAdedi", stokAdedi);
            updateCommand.Parameters.AddWithValue("@Tedarikci", tedarikci);
            updateCommand.Parameters.AddWithValue("@AlisFiyati", alisFiyati);
            updateCommand.Parameters.AddWithValue("@SatisFiyati", satisFiyati);
            updateCommand.Parameters.AddWithValue("@Barkod", barkod);

            connection.Open();
            updateCommand.ExecuteNonQuery();
            connection.Close();

            LoadStokData();
            ClearTextBoxes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string barkod = textBoxBarkod.Text;

            DialogResult result = MessageBox.Show("Seçili stok kaydını silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string deleteQuery = "DELETE FROM stok WHERE Barkod = @Barkod";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@Barkod", barkod);

                connection.Open();
                deleteCommand.ExecuteNonQuery();
                connection.Close();

                LoadStokData();
                ClearTextBoxes();
            }
        }

        private void ClearTextBoxes()
        {
            textBoxBarkod.Text = "";
            textBoxParcaAdi.Text = "";
            textBoxStokAdedi.Text = "";
            textBoxTedarikci.Text = "";
            textBoxAlisFiyati.Text = "";
            textBoxSatisFiyati.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string adSoyad = textBoxAdSoyad.Text;
            int pozisyonID = Convert.ToInt32(comboBoxPozisyon.SelectedValue);
            string telefon = textBoxTelefon.Text;
            string mail = textBoxMail.Text;
            string adres = textBoxAdres.Text;

            string insertQuery = "INSERT INTO personel (AdSoyad, PozisyonID, Telefon, Mail, Adres) " +
                "VALUES (@AdSoyad, @PozisyonID, @Telefon, @Mail, @Adres)";
            SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@AdSoyad", adSoyad);
            insertCommand.Parameters.AddWithValue("@PozisyonID", pozisyonID);
            insertCommand.Parameters.AddWithValue("@Telefon", telefon);
            insertCommand.Parameters.AddWithValue("@Mail", mail);
            insertCommand.Parameters.AddWithValue("@Adres", adres);

            connection.Open();
            insertCommand.ExecuteNonQuery();
            connection.Close();

            LoadPersonelData();
            ClearTextBox();
        }

        private void dataGridViewPersonel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridViewPersonel.Rows[e.RowIndex];
                textBoxAdSoyad.Text = row.Cells["AdSoyad"].Value.ToString();
                comboBoxPozisyon.SelectedValue = row.Cells["Pozisyon"].Value;
                textBoxTelefon.Text = row.Cells["Telefon"].Value.ToString();
                textBoxMail.Text = row.Cells["Mail"].Value.ToString();
                textBoxAdres.Text = row.Cells["Adres"].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string adSoyad = textBoxAdSoyad.Text;
            int pozisyonID = Convert.ToInt32(comboBoxPozisyon.SelectedValue);
            string telefon = textBoxTelefon.Text;
            string mail = textBoxMail.Text;
            string adres = textBoxAdres.Text;

            string updateQuery = "UPDATE personel SET AdSoyad = @AdSoyad, PozisyonID = @PozisyonID, Telefon = @Telefon, Mail = @Mail, Adres = @Adres " +
                "WHERE PersonelID = @PersonelID";
            SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@AdSoyad", adSoyad);
            updateCommand.Parameters.AddWithValue("@PozisyonID", pozisyonID);
            updateCommand.Parameters.AddWithValue("@Telefon", telefon);
            updateCommand.Parameters.AddWithValue("@Mail", mail);
            updateCommand.Parameters.AddWithValue("@Adres", adres);
            updateCommand.Parameters.AddWithValue("@PersonelID", dataGridViewPersonel.CurrentRow.Cells["PersonelID"].Value);

            connection.Open();
            updateCommand.ExecuteNonQuery();
            connection.Close();

            LoadPersonelData();
            ClearTextBox();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Seçili personel kaydını silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string deleteQuery = "DELETE FROM personel WHERE PersonelID = @PersonelID";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@PersonelID", dataGridViewPersonel.CurrentRow.Cells["PersonelID"].Value);

                connection.Open();
                deleteCommand.ExecuteNonQuery();
                connection.Close();

                LoadPersonelData();
                ClearTextBox();
            }
        }

        private void ClearTextBox()
        {
            textBoxAdSoyad.Text = "";
            textBoxTelefon.Text = "";
            textBoxMail.Text = "";
            textBoxAdres.Text = "";
        }

        private void YoneticiPanelForm_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

            DateTime Start = dateTimePicker1.Value.Date;
            DateTime End = dateTimePicker2.Value.Date;
            string query = "SELECT * FROM kasa WHERE Tarih BETWEEN @StartDate AND @EndDate";

                DataTable dataTable = new DataTable();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                    command.Parameters.AddWithValue("@StartDate", Start);
                    command.Parameters.AddWithValue("@EndDate", End);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                        {
                            connection.Open();
                            dataAdapter.Fill(dataTable);
                            connection.Close();
                        }
                    }
                }

                dataGridViewKasa.DataSource = dataTable;
            
        }

        private void FillDataGridMusteri()
        {
            string query = "Select * from MusteriBilgileri";

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        connection.Open();
                        dataAdapter.Fill(dataTable);
                        connection.Close();
                    }
                }
            }

            dataGridViewMusteri.DataSource = dataTable;
        }


        private void FillDataGridAriza()
        {
            string query = "Select * from ArizaTablosu";

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        connection.Open();
                        dataAdapter.Fill(dataTable);
                        connection.Close();
                    }
                }
            }

            dataGridViewAriza.DataSource = dataTable;
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            string query = "SELECT * FROM kasa";

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
            
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        connection.Open();
                        dataAdapter.Fill(dataTable);
                        connection.Close();
                    }
                }
            }

            dataGridViewKasa.DataSource = dataTable;
        }
    }
}
