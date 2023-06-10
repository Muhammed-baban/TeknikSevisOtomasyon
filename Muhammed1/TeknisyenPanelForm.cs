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

namespace Muhammed1
{
    public partial class TeknisyenPanelForm : Form
    {
        private string adSoyad;
        private SqlConnection connection;
        private string connectionString = (@"Server=DESKTOP-R9C7EV4; Initial Catalog=teknik_servis; Integrated Security=true;");

        public TeknisyenPanelForm(string adSoyad)
        {
            InitializeComponent();
            this.adSoyad = adSoyad;
            connection = new SqlConnection(connectionString);
            FillDataGridOnarimDurumuBaslamadi();
            FillDataGridYapilmasiGerekenIsler();
            label1.Text = "Giriş Saati: " + DateTime.Now.ToString() + " - Kullanıcı Adı Soyadı: " + adSoyad;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Seçili satırı kontrol et
            if (dataGridViewOnarimDurumuBaslamadi.SelectedRows.Count > 0)
            {
                // Seçili satırın Takip No değerini al
                int takipNo = Convert.ToInt32(dataGridViewOnarimDurumuBaslamadi.SelectedRows[0].Cells["CihazTakipNo"].Value);
                //string takipNo = dataGridViewOnarimDurumuBaslamadi.SelectedRows[0].Cells["CihazTakipNo"].Value.ToString();
                try
                {
                    connection.Open();

                    // Onarım durumunu güncelle (Devam Ediyor)
                    string updateRepairStatusQuery = "UPDATE ArizaTablosu SET OnarimDurumu = 'Devam Ediyor',IlgiliPersonel= @IlgiliPersonel WHERE CihazTakipNo = @TakipNo";
                    SqlCommand updateRepairStatusCommand = new SqlCommand(updateRepairStatusQuery, connection);
                    updateRepairStatusCommand.Parameters.AddWithValue("@TakipNo", Convert.ToInt32(takipNo));
                    updateRepairStatusCommand.Parameters.AddWithValue("@IlgiliPersonel", adSoyad);
                    updateRepairStatusCommand.ExecuteNonQuery();

                    MessageBox.Show("Onarım durumu güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Veri tablosunu yenile
                    FillDataGridOnarimDurumuBaslamadi();
                    FillDataGridYapilmasiGerekenIsler();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Onarım durumu güncelleme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir kayıt seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void FillDataGridOnarimDurumuBaslamadi()
        {
            string selectRepairInfoQuery = "SELECT AB.CihazTakipNo,CB.Cihaz,CB.Marka,cb.Model,CB.raf_bilgileri,CB.GarantiDurumu,AB.Sorun,AB.SorunTanimi FROM ArizaTablosu AB INNER JOIN CihazBilgileri CB ON AB.CihazTakipNo = CB.TakipNo WHERE AB.OnarimDurumu = 'Başlamadı'";
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(selectRepairInfoQuery, connection))
                {
                    connection.Open(); 

                    dataAdapter.Fill(dataTable);

                    connection.Close(); 
                }
            }

            dataGridViewOnarimDurumuBaslamadi.DataSource = dataTable;
        }

        private void FillDataGridYapilmasiGerekenIsler()
        {
            string query = "SELECT AB.CihazTakipNo,CB.Cihaz, CB.Marka, CB.Model, CB.raf_bilgileri, CB.GarantiDurumu, AB.Sorun, AB.SorunTanimi " +
                           "FROM ArizaTablosu AB " +
                           "INNER JOIN CihazBilgileri CB ON AB.CihazTakipNo = CB.TakipNo " +
                           "WHERE AB.IlgiliPersonel = @IlgiliPersonel and OnarimDurumu='Devam Ediyor'";

            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IlgiliPersonel", adSoyad);
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        connection.Open();
                        dataAdapter.Fill(dataTable);
                        connection.Close();
                    }
                }
            }

            dataGridViewYapilmasiGerekenIsler.DataSource = dataTable;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string YapilanOnarim = textBoxYapilanOnarim.Text;
            string Ucret = textBoxUcret.Text;
            DateTime tarih = DateTime.Now;

            // Seçili satırı kontrol et
            if (dataGridViewYapilmasiGerekenIsler.SelectedRows.Count > 0)
            {
                // Seçili satırın Takip No değerini al
                int takipNo = Convert.ToInt32(dataGridViewYapilmasiGerekenIsler.SelectedRows[0].Cells["CihazTakipNo"].Value);
                //string takipNo = dataGridViewOnarimDurumuBaslamadi.SelectedRows[0].Cells["CihazTakipNo"].Value.ToString();
                try
                {
                    connection.Open();

                    // Onarım durumunu güncelle (Devam Ediyor)
                    string updateRepairStatusQuery = "UPDATE ArizaTablosu SET OnarimDurumu = 'Onarım Tamamlandı',YapilanOnarim=@YapilanOnarim, Ucret=@Ucret WHERE CihazTakipNo = @TakipNo";
                    SqlCommand updateRepairStatusCommand = new SqlCommand(updateRepairStatusQuery, connection);
                    updateRepairStatusCommand.Parameters.AddWithValue("@TakipNo", Convert.ToInt32(takipNo));
                    updateRepairStatusCommand.Parameters.AddWithValue("@YapilanOnarim", YapilanOnarim);
                    updateRepairStatusCommand.Parameters.AddWithValue("@Ucret", Ucret);
                    updateRepairStatusCommand.ExecuteNonQuery();

                    string insertQuery = "INSERT INTO Kasa (cihaz_no, ücret, tarih, personel_id) " +
                         "VALUES (@CihazNo, @Ucret, @Tarih, @PersonelId)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@CihazNo", Convert.ToInt32(takipNo));
                    insertCommand.Parameters.AddWithValue("@Ucret", Ucret);
                    insertCommand.Parameters.AddWithValue("@Tarih", tarih);
                    insertCommand.Parameters.AddWithValue("@PersonelId", adSoyad);

                    insertCommand.ExecuteNonQuery();


                    MessageBox.Show("Onarım durumu güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Veri tablosunu yenile
                    FillDataGridOnarimDurumuBaslamadi();
                    FillDataGridYapilmasiGerekenIsler();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Onarım durumu güncelleme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir kayıt seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string YapilanOnarim = textBoxYapilanOnarim.Text;

            // Seçili satırı kontrol et
            if (dataGridViewYapilmasiGerekenIsler.SelectedRows.Count > 0)
            {
                // Seçili satırın Takip No değerini al
                int takipNo = Convert.ToInt32(dataGridViewYapilmasiGerekenIsler.SelectedRows[0].Cells["CihazTakipNo"].Value);
                //string takipNo = dataGridViewOnarimDurumuBaslamadi.SelectedRows[0].Cells["CihazTakipNo"].Value.ToString();
                try
                {
                    connection.Open();

                    // Onarım durumunu güncelle (Devam Ediyor)
                    string updateRepairStatusQuery = "UPDATE ArizaTablosu SET OnarimDurumu = 'Onarım Tamamlanamadı',YapilanOnarim=@YapilanOnarim, Ucret='0' WHERE CihazTakipNo = @TakipNo";
                    SqlCommand updateRepairStatusCommand = new SqlCommand(updateRepairStatusQuery, connection);
                    updateRepairStatusCommand.Parameters.AddWithValue("@TakipNo", Convert.ToInt32(takipNo));
                    updateRepairStatusCommand.Parameters.AddWithValue("@YapilanOnarim", YapilanOnarim);
                    updateRepairStatusCommand.ExecuteNonQuery();

                    MessageBox.Show("Onarım durumu güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Veri tablosunu yenile
                    FillDataGridOnarimDurumuBaslamadi();
                    FillDataGridYapilmasiGerekenIsler();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Onarım durumu güncelleme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir kayıt seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
