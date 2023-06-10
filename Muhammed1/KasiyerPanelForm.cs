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
    public partial class KasiyerPanelForm : Form
    {
        private string adSoyad;
        private SqlConnection connection;
        private string connectionString = (@"Server=DESKTOP-R9C7EV4; Initial Catalog=teknik_servis; Integrated Security=true;");

        public KasiyerPanelForm(string adSoyad)
        {
            InitializeComponent();
            this.adSoyad = adSoyad;
            InitializeDatabaseConnection();
            ListeleTeknisyenler();
            label20.Text = "Giriş Saati: " + DateTime.Now.ToString() + " - Kullanıcı Adı Soyadı: " + adSoyad;

        }

        private void ListeleTeknisyenler()
        {
            textBoxIlgiliPersonel.Items.Clear(); 

            string selectQuery = "SELECT AdSoyad FROM personel WHERE Pozisyon = 'Teknisyen'";
            SqlCommand selectCommand = new SqlCommand(selectQuery, connection);

            connection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                string adSoyad = reader["AdSoyad"].ToString();
                textBoxIlgiliPersonel.Items.Add(adSoyad); // Teknisyenleri combobox'a ekle
            }

            reader.Close();
            connection.Close();
        }

        private void InitializeDatabaseConnection()
        {
            connection = new SqlConnection(connectionString);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string adSoyad = textBoxAdSoyad.Text;
            string telefon = textBoxTelefon.Text;
            string adres = textBoxAdres.Text;
            string email = textBoxEmail.Text;

            string insertQuery = "INSERT INTO MusteriBilgileri (AdSoyad, Telefon, Adres, Email) " +
                "VALUES (@AdSoyad, @Telefon, @Adres, @Email)";
            SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@AdSoyad", adSoyad);
            insertCommand.Parameters.AddWithValue("@Telefon", telefon);
            insertCommand.Parameters.AddWithValue("@Adres", adres);
            insertCommand.Parameters.AddWithValue("@Email", email);

            connection.Open();
            insertCommand.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Müşteri kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void ClearTextBoxes()
        {
            textBoxTakipNo.Text = "";
            textBoxChaz.Text = "";
            textBoxMarka.Text = "";
            textBoxModel.Text = "";
            textBoxSeriNo.Text = "";
            textBoxGarantiDurumu.Text = "";
            textBoxRafBilgileri.Text = "";
            textBoxSorun.Text = "";
            textBoxSorunTanimi.Text = "";
            textBoxIlgiliPersonel.Text = "";
            textBoxOnarimDurumu.Text = "Başlamadı";
            textBoxYapilanOnarim.Text = "";
            textBoxUcret.Text = "";
            textBoxTelefon.Text = "";
            textBoxAdSoyad.Text = "";
            textBoxAdres.Text = "";
            textBoxEmail.Text = "";
        }


        private void button3_Click(object sender, EventArgs e)
        {
            string adSoyad = textBoxAdSoyad.Text;
            string telefon = textBoxTelefon.Text;
            string adres = textBoxAdres.Text;
            string email = textBoxEmail.Text;

            string updateQuery = "UPDATE MusteriBilgileri SET AdSoyad = @AdSoyad, Adres = @Adres, Email = @Email " +
                "WHERE Telefon = @Telefon";
            SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@AdSoyad", adSoyad);
            updateCommand.Parameters.AddWithValue("@Adres", adres);
            updateCommand.Parameters.AddWithValue("@Email", email);
            updateCommand.Parameters.AddWithValue("@Telefon", telefon);

            connection.Open();
            updateCommand.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Müşteri güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string telefon = textBoxTelefon.Text;

                // Müşteri bilgilerini getir
                string selectCustomerQuery = "SELECT * FROM MusteriBilgileri WHERE Telefon = @Telefon";
                SqlCommand selectCustomerCommand = new SqlCommand(selectCustomerQuery, connection);
                selectCustomerCommand.Parameters.AddWithValue("@Telefon", telefon);

                connection.Open();

                SqlDataReader customerReader = selectCustomerCommand.ExecuteReader();

                if (customerReader.Read())
                {
                    // Müşteri bilgilerini doldur
                    textBoxAdSoyad.Text = customerReader["AdSoyad"].ToString();
                    textBoxAdres.Text = customerReader["Adres"].ToString();
                    textBoxEmail.Text = customerReader["Email"].ToString();
                }

                customerReader.Close();

                connection.Close();
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            string takipNo = textBoxTakipNo.Text;
            string chaz = textBoxChaz.Text;
            string marka = textBoxMarka.Text;
            string model = textBoxModel.Text;
            string seriNo = textBoxSeriNo.Text;
            string garantiDurumu = textBoxGarantiDurumu.Text;
            string rafBilgileri = textBoxRafBilgileri.Text;
            string sorun = textBoxSorun.Text;
            string sorunTanimi = textBoxSorunTanimi.Text;
            string ilgiliPersonel = textBoxIlgiliPersonel.Text;
            string onarimDurumu = textBoxOnarimDurumu.Text;
            string yapilanOnarim = textBoxYapilanOnarim.Text;
            string ucret = textBoxUcret.Text;
            string telefon = textBoxTelefon.Text;



            // Cihaz bilgilerini kaydet
            string insertDeviceInfoQuery = "INSERT INTO CihazBilgileri (TakipNo, Cihaz, Marka, Model, SeriNo, GarantiDurumu, raf_bilgileri, MusteriTelefon) VALUES (@TakipNo, @Chaz, @Marka, @Model, @SeriNo, @GarantiDurumu, @RafBilgileri, @MusteriTelefon)";
            SqlCommand insertDeviceInfoCommand = new SqlCommand(insertDeviceInfoQuery, connection);
            insertDeviceInfoCommand.Parameters.AddWithValue("@TakipNo", takipNo);
            insertDeviceInfoCommand.Parameters.AddWithValue("@Chaz", chaz);
            insertDeviceInfoCommand.Parameters.AddWithValue("@Marka", marka);
            insertDeviceInfoCommand.Parameters.AddWithValue("@Model", model);
            insertDeviceInfoCommand.Parameters.AddWithValue("@SeriNo", seriNo);
            insertDeviceInfoCommand.Parameters.AddWithValue("@GarantiDurumu", garantiDurumu);
            insertDeviceInfoCommand.Parameters.AddWithValue("@RafBilgileri", rafBilgileri);
            insertDeviceInfoCommand.Parameters.AddWithValue("@MusteriTelefon", telefon);


            string insertRepairInfoQuery = "INSERT INTO ArizaTablosu (CihazTakipNo, Sorun, SorunTanimi, IlgiliPersonel, OnarimDurumu, YapilanOnarim, Ucret) VALUES (@TakipNo, @Sorun, @SorunTanimi, @IlgiliPersonel, @OnarimDurumu, @YapilanOnarim, @Ucret)";
            SqlCommand insertRepairInfoCommand = new SqlCommand(insertRepairInfoQuery, connection);
            insertRepairInfoCommand.Parameters.AddWithValue("@TakipNo", takipNo);
            insertRepairInfoCommand.Parameters.AddWithValue("@Sorun", sorun);
            insertRepairInfoCommand.Parameters.AddWithValue("@SorunTanimi", sorunTanimi);
            insertRepairInfoCommand.Parameters.AddWithValue("@IlgiliPersonel", ilgiliPersonel);
            insertRepairInfoCommand.Parameters.AddWithValue("@OnarimDurumu", onarimDurumu);
            insertRepairInfoCommand.Parameters.AddWithValue("@YapilanOnarim", yapilanOnarim);
            insertRepairInfoCommand.Parameters.AddWithValue("@Ucret", ucret);


            


            try
            {
                connection.Open();

               
                // Cihaz bilgilerini kaydet
                insertDeviceInfoCommand.ExecuteNonQuery();
                connection.Close();
                connection.Open();

                // Arıza bilgilerini kaydet
                insertRepairInfoCommand.ExecuteNonQuery();

                
                MessageBox.Show("Cihaz teslim alındı ve kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearTextBoxes();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Cihaz teslim alma işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            // Takip numarasını al
            int takipNo = Convert.ToInt32(textBoxTakipNo.Text);

            try
            {
                connection.Open();

                // Cihazın onarım durumunu güncelle (Teslim Edildi)
                string updateRepairStatusQuery = "UPDATE ArizaTablosu SET OnarimDurumu = 'Müşteriye Teslim Edildi' WHERE CihazTakipNo = @TakipNo";
                SqlCommand updateRepairStatusCommand = new SqlCommand(updateRepairStatusQuery, connection);
                updateRepairStatusCommand.Parameters.AddWithValue("@TakipNo", takipNo);
                updateRepairStatusCommand.ExecuteNonQuery();

                string query = "INSERT INTO Kasa (cihaz_no, ücret, tarih,personel_id) VALUES (@cihaz_no, @ücret, @tarih, @personel)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@cihaz_no", textBoxSeriNo.Text);
                command.Parameters.AddWithValue("@ücret", Convert.ToDecimal(textBoxUcret.Text));
                command.Parameters.AddWithValue("@tarih", DateTime.Now);
                command.Parameters.AddWithValue("@Personel", adSoyad);

                command.ExecuteNonQuery();

                string updateRepairStatusQuery2 = "UPDATE CihazBilgileri SET raf_bilgileri = '' WHERE TakipNo = @TakipNo";
                SqlCommand updateRepairStatusCommand2 = new SqlCommand(updateRepairStatusQuery2, connection);
                updateRepairStatusCommand2.Parameters.AddWithValue("@TakipNo", takipNo);
                updateRepairStatusCommand2.ExecuteNonQuery();

                textBoxTelefon.Text = string.Empty;
                textBoxAdSoyad.Text = string.Empty;
                textBoxAdres.Text = string.Empty;
                textBoxEmail.Text = string.Empty;
                textBoxTakipNo.Text = string.Empty;
                textBoxChaz.Text = string.Empty;
                textBoxMarka.Text = string.Empty;
                textBoxModel.Text = string.Empty;
                textBoxSeriNo.Text = string.Empty;
                textBoxGarantiDurumu.Text = string.Empty;
                textBoxSorun.Text = string.Empty;
                textBoxSorunTanimi.Text = string.Empty;
                textBoxIlgiliPersonel.Text = string.Empty;
                textBoxOnarimDurumu.Text = "Başlamadı";
                textBoxYapilanOnarim.Text = string.Empty;
                textBoxRafBilgileri.Text = string.Empty;
                textBoxUcret.Text = string.Empty;



                MessageBox.Show("Cihaz müşteriye teslim edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearTextBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cihaz teslim etme işlemi sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void textBoxTakipNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string takipNo = textBoxTakipNo.Text;

                string selectDeviceInfoQuery = "SELECT * FROM CihazBilgileri WHERE TakipNo = @TakipNo";
                string selectCustomerInfoQuery = "SELECT * FROM ArizaTablosu WHERE CihazTakipNo = @TakipNo";

                connection.Open();

                using (SqlCommand selectDeviceInfoCommand = new SqlCommand(selectDeviceInfoQuery, connection))
                using (SqlCommand selectCustomerInfoCommand = new SqlCommand(selectCustomerInfoQuery, connection))
                {
                    selectDeviceInfoCommand.Parameters.AddWithValue("@TakipNo", takipNo);
                    selectCustomerInfoCommand.Parameters.AddWithValue("@TakipNo", takipNo);

                    using (SqlDataReader deviceReader = selectDeviceInfoCommand.ExecuteReader())
                    {
                        if (deviceReader.Read())
                        {
                            // Cihaz bilgilerini doldur
                            textBoxChaz.Text = deviceReader["Cihaz"].ToString();
                            textBoxMarka.Text = deviceReader["Marka"].ToString();
                            textBoxModel.Text = deviceReader["Model"].ToString();
                            textBoxSeriNo.Text = deviceReader["SeriNo"].ToString();
                            textBoxGarantiDurumu.Text = deviceReader["GarantiDurumu"].ToString();
                            textBoxRafBilgileri.Text = deviceReader["raf_bilgileri"].ToString();
                        }
                        deviceReader.Close();
                    }

                    using (SqlDataReader customerReader = selectCustomerInfoCommand.ExecuteReader())
                    {
                        if (customerReader.Read())
                        {
                            // Müşteri bilgilerini doldur
                           textBoxSorun.Text = customerReader["Sorun"].ToString();
                            textBoxSorunTanimi.Text = customerReader["SorunTanimi"].ToString();
                            textBoxIlgiliPersonel.Text = customerReader["IlgiliPersonel"].ToString();
                            textBoxOnarimDurumu.Text = customerReader["OnarimDurumu"].ToString();
                            textBoxYapilanOnarim.Text = customerReader["YapilanOnarim"].ToString();
                            textBoxUcret.Text = customerReader["Ucret"].ToString();
                        }
                        customerReader.Close();
                    }
                }

                connection.Close();
            }
        
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void KasiyerPanelForm_Load(object sender, EventArgs e)
        {

        }

        

        // Diğer kodlar ve olay işleyicileri
    }
}
