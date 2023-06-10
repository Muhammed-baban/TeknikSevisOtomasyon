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
    public partial class Form1 : Form
    {
        private sqlbaglanti baglanti;
        private string adSoyad;

        public Form1()
        {
            InitializeComponent();
            baglanti = new sqlbaglanti();
            baglanti.DatabaseConnection();

        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string kullaniciAdi = txtKullaniciAdi.Text;
            string sifre = txtSifre.Text;
            string pozisyon = GirisYap(kullaniciAdi, sifre);

            if (pozisyon == "Kasiyer")
            {
                KasiyerPanelForm kasiyerPanel = new KasiyerPanelForm(adSoyad); 
                kasiyerPanel.Show();
                this.Hide();
            }
            else if (pozisyon == "Teknisyen")
            {
                TeknisyenPanelForm teknisyenPanel = new TeknisyenPanelForm(adSoyad);
                teknisyenPanel.Show();
                this.Hide();
            }
            else if (pozisyon == "Yönetici")
            {
                YoneticiPanelForm yoneticiPanel = new YoneticiPanelForm(adSoyad); 
                yoneticiPanel.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Geçersiz kullanıcı adı veya şifre.");
            }

        }

        private string GirisYap(string kullaniciAdi, string sifre)
        {
            string pozisyon = "";

            try
            {
                baglanti.OpenConnection();

                string query = $"SELECT Pozisyon, AdSoyad FROM Personel WHERE KullaniciAdi = '{kullaniciAdi}' AND Sifre = '{sifre}'";
                SqlDataReader reader = baglanti.ExecuteQuery(query);

                if (reader.HasRows)
                {
                    reader.Read();
                    pozisyon = reader.GetString(0);
                    adSoyad = reader.GetString(1); 
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message);
            }
            finally
            {
                baglanti.CloseConnection();
            }

            return pozisyon;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yöneticinize başvurun!", "Şifre Yenileme", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
    

