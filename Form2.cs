using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
// Oracle ile bağlantı yapılabilmesi için kütüphane eklendi...
using System.Text.RegularExpressions;
//Regex kütüphanesinin tanımlanması(güvenli parola oluşturulması)
using System.IO;
//Giriş-Çıkış işlemlerine ilişkin kütüphanenin tanımlanması



namespace Personel_Takip_Otomasyonu
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        OracleConnection connection = new OracleConnection("User Id=SYSTEM;Password=1234;Data Source=localhost:1521/XEPDB1;");

        private void kullanicilari_göster()
        {
            try
            {
                connection.Open();
                string sorgu2 = "SELECT * FROM kullanicilar";
                OracleDataAdapter kullanicilari_listele = new OracleDataAdapter
                    (sorgu2, connection);
                DataSet dshafiza = new DataSet();
                kullanicilari_listele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                connection.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();
                
            }
        }
        private void personelleri_göster()
        {
            try
            {
                connection.Open();
                string sorgu3 = "SELECT * FROM Personeller";
                OracleDataAdapter personelleri_listele = new OracleDataAdapter
                    (sorgu3, connection);
                DataSet dshafiza = new DataSet();
                personelleri_listele.Fill(dshafiza);
                dataGridView2.DataSource = dshafiza.Tables[0];
                connection.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // FORM 2 AYARLARI..
            pictureBox1.Height = 150;
            pictureBox1.Width = 150;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            try
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\" + Form1.tcno + ".jpg");

            }
            catch 
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\resimyokk.jpg");

            }
            //Kullanıcı işlemleri sekmesinin ayarları
            this.Text = "Yönetici İşlemleri";
            label11.ForeColor = Color.DarkRed;
            label11.Text = Form1.adi + " " + Form1.soyadi;
            textBox1.MaxLength = 11;
            textBox4.MaxLength = 8;
            toolTip1.SetToolTip(this.textBox1, "TC Kimlik No 11 Karakter Olmalı!");
            radioButton1.Checked = true;

            textBox2.CharacterCasing = CharacterCasing.Upper;
            textBox3.CharacterCasing = CharacterCasing.Upper;
            textBox5.MaxLength = 10;
            textBox6.MaxLength = 10;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;

            kullanicilari_göster();

            //Personel işlemleri sekmesinin ayarları
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Width = 100; pictureBox2.Height = 100;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            maskedTextBox1.Mask = "00000000000";//11 tane rakam girişini zorunlu yaptık.
            maskedTextBox2.Mask = "LL??????????????????????";//isim için en az iki karakterli olması zorunlu kılındı.
            maskedTextBox3.Mask = "LL??????????????????????";//soyisim için en az iki karakterli olması zorunlu kılındı.
            maskedTextBox4.Mask = "0000"; // Maaş en az dört karakterli olmalıdır.
            maskedTextBox4.Text = "0";
            maskedTextBox2.Text.ToUpper();
            maskedTextBox3.Text.ToUpper();

            comboBox1.Items.Add("İlköğretim");
            comboBox1.Items.Add("Ortaöğretim");
            comboBox1.Items.Add("Lise");
            comboBox1.Items.Add("Üniversite");

            comboBox2.Items.Add("Yönetici");
            comboBox2.Items.Add("Memur");
            comboBox2.Items.Add("Şoför");
            comboBox2.Items.Add("İşçi");

            comboBox3.Items.Add("ARGE");
            comboBox3.Items.Add("Bilgi İşlem");
            comboBox3.Items.Add("Muhasebe");
            comboBox3.Items.Add("Üretim");
            comboBox3.Items.Add("Paketleme");
            comboBox3.Items.Add("Nakliye");

            DateTime zaman = DateTime.Now;
            int yil = int.Parse(zaman.ToString("yyyy"));
            int ay = int.Parse(zaman.ToString("MM"));
            int gün = int.Parse(zaman.ToString("dd"));
            dateTimePicker1.MinDate = new DateTime(1960, 1, 1);//60 yaş üstü çalışanımız olmayacak.
            dateTimePicker1.MaxDate = new DateTime(yil - 18,ay,gün);//18 yaş altı çalışanımız olmayacak.
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            radioButton3.Checked = true;
            personelleri_göster();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 11)
                errorProvider1.SetError(textBox1, "TC Kimlik No 11 karakter olmalı!");
            else
                errorProvider1.Clear();

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Kullanıcının rakam girişi dışında herhangi bir işlem yapmasını engelleriz.
            //Rakamların ASCII karşılıklarına göre sınırlar belirlendi.
            if ((int)(e.KeyChar) >= 48 && (int)(e.KeyChar) <= 57 || (int)e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Kullanıcı burada yalnızca harf tuşuna basabilecek,boşluk bırakabilecek.
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Kullanıcı burada yalnızca harf tuşuna basabilecek,boşluk bırakabilecek.
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length != 8)
                errorProvider1.SetError(textBox4, "Kullanıcı adı 8 karakter olmalıdır!");
            else
                errorProvider1.Clear();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsDigit(e.KeyChar) == true)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {


        }

        int parola_skoru = 0;
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string parola_seviyesi = "";
            int kucuk_harf_skoru = 0;
            int buyuk_harf_skoru = 0;
            int rakam_skoru = 0;
            int sembol_skoru = 0;
            string sifre = textBox5.Text;
            //Regex kütüphanesi ingilizce karakterleri  baz aldığından Türkçe karkaterlerde sorun yaşamamak
            //için şifre string ifadesindeki Türkçe karakterleri İngilizce karakterlere dönüştürmemiz gerekir.
            string duzeltilmis_sifre = "";
            duzeltilmis_sifre = sifre;
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('İ', 'I');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ı', 'i');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ç', 'C');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ç', 'c');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ş', 'S');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ş', 's');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ğ', 'G');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ü', 'U');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ü', 'u');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ö', 'O');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ö', 'o');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ğ', 'g');

            if (sifre != duzeltilmis_sifre)
            {
                sifre = duzeltilmis_sifre;
                textBox5.Text = sifre;
                MessageBox.Show("Paroladaki Türkçe karakterler İngizlizce karakterlere dönüştürülmüştür!");
            }
            //1 küçük harf 10 puan, 2 ve üzeri 20 puan
            int az_karakter_sayisi = sifre.Length - Regex.Replace(sifre, "[a-z]", "").Length;
            kucuk_harf_skoru = Math.Min(2, az_karakter_sayisi) * 10;

            //1 büyük harf 10 puan, 2 ve üzeri 20 puan
            int AZ_karakter_sayisi = sifre.Length - Regex.Replace(sifre, "[A-Z]", "").Length;
            buyuk_harf_skoru = Math.Min(2, AZ_karakter_sayisi) * 10;

            //1 rakam 10 puan, 2 ve üzeri 20 puan
            int rakam_sayisi = sifre.Length - Regex.Replace(sifre, "[a-z]", "").Length;
            rakam_skoru = Math.Min(2, rakam_sayisi) * 10;

            //1 sembol 10 puan, 2 ve üzeri 20 puan
            int sembol_sayisi = sifre.Length - az_karakter_sayisi - AZ_karakter_sayisi - rakam_sayisi;
            sembol_skoru = Math.Min(2, sembol_sayisi) * 10;

            parola_skoru = kucuk_harf_skoru + buyuk_harf_skoru + rakam_skoru + sembol_skoru;
            if (sifre.Length == 9)
                parola_skoru += 10;
            else if (sifre.Length == 10)
                parola_skoru += 20;

            if (kucuk_harf_skoru == 0 || buyuk_harf_skoru == 0 || rakam_skoru == 0 || sembol_skoru == 0)
                label22.Text = "Büyük harf,küçük harf, rakam ve sembol mutlaka kullanmalısın!";
            if (kucuk_harf_skoru != 0 && buyuk_harf_skoru != 0 || rakam_skoru != 0 || sembol_skoru != 0)
                label22.Text = "";

            if (parola_skoru < 70)
                parola_seviyesi = "Kabul edilemez";
            else if (parola_skoru == 70 || parola_skoru == 80)
                parola_seviyesi = "Güçlü";
            else if (parola_skoru == 90 || parola_skoru == 100)
                parola_seviyesi = "Çok güçlü";

            label9.Text = "%" + Convert.ToString(parola_skoru);
            label10.Text = parola_seviyesi;
            progressBar1.Value = parola_skoru;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != textBox5.Text)
                errorProvider1.SetError(textBox6, "Parola tekrarı eşleşmiyor");
            else
                errorProvider1.Clear();
        }

        private void topPage1_temizle()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }
        private void topPage2_temizle()
        {
            pictureBox2.Image = null;
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            maskedTextBox3.Clear();
            maskedTextBox4.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string yetki = "";
            bool kayitkontrol = false;

            connection.Open();
            OracleCommand selectsorgu = new OracleCommand("select * from kullanicilar where tcno='"+textBox1.Text+"'", connection);
            OracleDataReader kayitokuma = selectsorgu.ExecuteReader();
            while (kayitokuma.Read())
            {
                kayitkontrol = true;
                break;
            }
            connection.Close();

            if (kayitkontrol == false)
            {
                //TC Kimlik No kontrolu
                if (textBox1.Text.Length < 11 || textBox1.Text == "")
                    label1.ForeColor = Color.Red;
                else
                    label1.ForeColor = Color.Black;

                //Adı kontrolu
                if (textBox2.Text.Length < 2 || textBox2.Text == "")
                    label2.ForeColor = Color.Red;
                else
                    label2.ForeColor = Color.Black;
                //Soyadı kontrolu
                if (textBox3.Text.Length < 2 || textBox3.Text == "")
                    label3.ForeColor = Color.Red;
                else
                    label3.ForeColor = Color.Black;
                //Kullancıadı kontrolu
                if (textBox4.Text.Length != 8 || textBox4.Text == "")
                    label5.ForeColor = Color.Red;
                else
                    label5.ForeColor = Color.Black;
                //Parola kontrolu
                if (parola_skoru < 70 || textBox5.Text == "")
                    label6.ForeColor = Color.Red;
                else
                    label6.ForeColor = Color.Black;
                //Parola tekrar veri kontrolu
                if (textBox5.Text!=textBox6.Text || textBox6.Text == "")
                    label7.ForeColor = Color.Red;
                else
                    label7.ForeColor = Color.Black;

                if(textBox1.Text.Length==11 && textBox1.Text!="" && textBox2.Text!="" && textBox2.Text.Length>1 &&
                    textBox3.Text != "" && textBox3.Text.Length > 1 &&
                    textBox4.Text != "" && textBox5.Text != "" &&
                    textBox6.Text != "" && textBox5.Text==textBox6.Text && parola_skoru >= 70)
                {
                    if (radioButton1.Checked == true)
                        yetki = "Yönetici";
                    else if (radioButton2.Checked == true)
                        yetki = "Kullanıcı";
                    try
                    {
                        connection.Open();
                        OracleCommand eklekomutu = new OracleCommand("insert into kullanicilar values" +
                            "('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + yetki
                            + "','" + textBox4.Text + "','" + textBox5.Text + "')", connection);
                        eklekomutu.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Yeni kullanıcı kaydı oluşturuldu", "Personel Takip" +
                            " Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        topPage1_temizle();
                        
                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message);
                        connection.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Yazı rengi kırmızı olan alanları yenidn gözden geçirin!",
                        "Personel Takip otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Girilen TC kimlik numarası daha önceden kayıtlıdır",
                    "Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool kayit_arama = false;
            if (textBox1.Text.Length == 11)
            {
                connection.Open();
                OracleCommand selectsorgu = new OracleCommand("Select * from kullanicilar where tcno='" + textBox1.Text + "'", connection);
                OracleDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama = true;
                    textBox2.Text = kayitokuma.GetValue(1).ToString();
                    textBox3.Text = kayitokuma.GetValue(2).ToString();
                    if (kayitokuma.GetValue(3).ToString() == "Yönetici")
                        radioButton1.Checked = true;
                    else
                        radioButton2.Checked = true;
                    textBox4.Text = kayitokuma.GetValue(4).ToString();
                    textBox5.Text = kayitokuma.GetValue(5).ToString();
                    textBox6.Text = kayitokuma.GetValue(5).ToString();
                    break;
                }
                if (kayit_arama = false)
                {
                    MessageBox.Show("Aranan kayıt bulunamadı!", "Personel Takip Otomasyonu",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                connection.Close();
            }
            else
            {
                MessageBox.Show("Lütfen 11 haneli TC Kimlik no giriniz!", "Personel Takip Otomasyonu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                topPage1_temizle();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string yetki = "";

            //TC Kimlik No kontrolu
            if (textBox1.Text.Length < 11 || textBox1.Text == "")
                label1.ForeColor = Color.Red;
            else
                label1.ForeColor = Color.Black;

            //Adı kontrolu
            if (textBox2.Text.Length < 2 || textBox2.Text == "")
                label2.ForeColor = Color.Red;
            else
                label2.ForeColor = Color.Black;
            //Soyadı kontrolu
            if (textBox3.Text.Length < 2 || textBox3.Text == "")
                label3.ForeColor = Color.Red;
            else
                label3.ForeColor = Color.Black;
            //Kullancıadı kontrolu
            if (textBox4.Text.Length != 8 || textBox4.Text == "")
                label5.ForeColor = Color.Red;
            else
                label5.ForeColor = Color.Black;
            //Parola kontrolu
            if (parola_skoru < 70 || textBox5.Text == "")
                label6.ForeColor = Color.Red;
            else
                label6.ForeColor = Color.Black;
            //Parola tekrar veri kontrolu
            if (textBox5.Text != textBox6.Text || textBox6.Text == "")
                label7.ForeColor = Color.Red;
            else
                label7.ForeColor = Color.Black;

            if (textBox1.Text.Length == 11 && textBox1.Text != "" && textBox2.Text != "" && textBox2.Text.Length > 1 &&
                textBox3.Text != "" && textBox3.Text.Length > 1 &&
                textBox4.Text != "" && textBox5.Text != "" &&
                textBox6.Text != "" && textBox5.Text == textBox6.Text && parola_skoru >= 70)
            {
                if (radioButton1.Checked == true)
                    yetki = "Yönetici";
                else if (radioButton2.Checked == true)
                    yetki = "Kullanıcı";
                try
                {
                    connection.Open();
                    OracleCommand güncellekomutu = new OracleCommand("update kullanicilar set ad='"+textBox2.Text+
                        "',soyad='"+textBox3.Text+"',yetki='"+yetki+
                        "',kullaniciadi='"+textBox4.Text+"',parola='"+textBox5.Text+"' where tcno='"+textBox1.Text+"'",connection);
                    güncellekomutu.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Kullanıcı bilgileri güncellendi", "Personel Takip" +
                        " Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    kullanicilari_göster();
                }
                catch (Exception hatamsj)
                {
                    MessageBox.Show(hatamsj.Message,"Personel Takip Otomasyonu",
                            MessageBoxButtons.OK,MessageBoxIcon.Error);
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Yazı rengi kırmızı olan alanları yenidn gözden geçirin!",
                    "Personel Takip otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 11)
            {
                bool kayit_arama_durumu = false;
                connection.Open();
                OracleCommand selectsorgu = new OracleCommand("Select * from kullanicilar where tcno='" +
                    textBox1.Text + "'", connection);
                OracleDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    OracleCommand deletesorgu=new OracleCommand("DELETE  from kullanicilar where tcno='"+textBox1.Text+
                        "'",connection);
                    deletesorgu.ExecuteNonQuery();
                    MessageBox.Show("Kullanıcı kaydı silindi!", "Personel Takip Otomasyonu",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    connection.Close();
                    kullanicilari_göster();
                    topPage1_temizle();
                    break;
                }
                if (kayit_arama_durumu = false)
                    MessageBox.Show("Silinecek kayıt bulunamadı!", "Personel Takip Otomasyonu",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();
                topPage1_temizle();

            }
            else
                MessageBox.Show("Lütfen 11 karakterden oluşan bir TC Kimlik no giriniz!", "Personel Takip Otomasyonu",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            topPage1_temizle();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog resimsec = new OpenFileDialog();
            resimsec.Title = " Personel resmi seçiniz.";
            resimsec.Filter = "JPG Dosyalar (*.jpg) | *.jpg";
            if (resimsec.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox2.Image = new Bitmap(resimsec.OpenFile());
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string cinsiyet = "";
            bool kayitkontrol = false;
            connection.Open();
            OracleCommand selectsorgu = new OracleCommand("select * from personeller where tcno='" +
                maskedTextBox1.Text + "'", connection);
            OracleDataReader kayitokuma = selectsorgu.ExecuteReader();
            while (kayitokuma.Read())
            {
                kayitkontrol = true;
                break;
            }
            connection.Close();
            if (kayitkontrol == false)
            {
                if (pictureBox2.Image == null)
                    button6.ForeColor = Color.Red;
                else
                    button6.ForeColor = Color.Black;
                if (maskedTextBox1.MaskCompleted == false)
                    label13.ForeColor = Color.Red;
                else
                    label13.ForeColor = Color.Black;
                if (maskedTextBox2.MaskCompleted == false)
                    label14.ForeColor = Color.Red;
                else
                    label14.ForeColor = Color.Black;
                if (maskedTextBox3.MaskCompleted == false)
                    label15.ForeColor = Color.Red;
                else
                    label15.ForeColor = Color.Black;
                if (comboBox1.Text == "")
                    label17.ForeColor = Color.Red;
                else
                    label17.ForeColor = Color.Black;
                if (comboBox2.Text == "")
                    label19.ForeColor = Color.Red;
                else
                    label19.ForeColor = Color.Black;
                if (comboBox3.Text == "")
                    label20.ForeColor = Color.Red;
                else
                    label20.ForeColor = Color.Black;
                if (maskedTextBox4.MaskCompleted == false)
                    label21.ForeColor = Color.Red;
                else
                    label21.ForeColor = Color.Black;
                if (int.Parse(maskedTextBox4.Text) < 1000)
                    label21.ForeColor = Color.Red;
                else
                    label21.ForeColor = Color.Black;

                if (pictureBox2.Image != null && maskedTextBox1.MaskCompleted != false && maskedTextBox2.MaskCompleted != false &&
                    maskedTextBox3.MaskCompleted != false && comboBox1.Text != "" && comboBox2.Text != "" &&
                    comboBox3.Text != "" && maskedTextBox4.MaskCompleted != false)
                {
                    if (radioButton3.Checked == true)
                        cinsiyet = "Bay";
                    else if (radioButton4.Checked == true)
                        cinsiyet = "Bayan";
                    try
                    {
                        connection.Open();
                        OracleCommand eklekomutu = new OracleCommand("insert into personeller values('" + maskedTextBox1.Text +
                            "','" + maskedTextBox2.Text + "','" + maskedTextBox3.Text + "','" + cinsiyet + "','" +
                            comboBox1.Text + "','" + dateTimePicker1.Text + "','" + comboBox2.Text + "','" + comboBox3.Text + "','" + maskedTextBox4.Text + "')", connection);
                        eklekomutu.ExecuteNonQuery();
                        connection.Close();
                        if (!Directory.Exists(Application.StartupPath + "\\personelresimler"))
                        {
                            Directory.CreateDirectory(Application.StartupPath + "\\personelresimler");

                            pictureBox2.Image.Save(Application.StartupPath + "\\personelresimler\\" + maskedTextBox1 + ".jpg");
                            MessageBox.Show("Yeni personel kaydı oluşturuldu", "Personel Takip Otomasyonu",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            personelleri_göster();
                            topPage2_temizle();
                        }
                        else
                        {
                            pictureBox2.Image.Save(Application.StartupPath + "\\personelresimler\\" + maskedTextBox1 + ".jpg");
                            MessageBox.Show("Yeni personel kaydı oluşturuldu", "Personel Takip Otomasyonu",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            personelleri_göster();
                            topPage2_temizle();

                        }

                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message, "Personel Takip Otomasyonu",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        connection.Close();
                    }

                }
                else
                    MessageBox.Show("Yazı rengi kırmızı olan alanları yeniden gözden geçiriniz",
                        "Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Girilen TC Kimlik numarası daha önceden kayıtlıdır!",
                    "Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;
            if (maskedTextBox1.Text.Length == 11)
            {
                connection.Open();
                OracleCommand selectsorgu = new OracleCommand("select * from personeller where tcno='" +
                    maskedTextBox1.Text + "'", connection);
                OracleDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    try
                    {
                        pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\" +
                            kayitokuma.GetValue(0).ToString() + ".jpg");
                        
                       
                    }
                    catch
                    {
                        pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\resimyokk.jpg");

                    }
                    maskedTextBox2.Text = kayitokuma.GetValue(1).ToString();
                    maskedTextBox3.Text = kayitokuma.GetValue(2).ToString();
                    if (kayitokuma.GetValue(3).ToString() == "Bay")
                        radioButton3.Checked = true;
                    else
                        radioButton4.Checked = true;
                    comboBox1.Text = kayitokuma.GetValue(4).ToString();
                    dateTimePicker1.Text = kayitokuma.GetValue(5).ToString();
                    comboBox2.Text = kayitokuma.GetValue(6).ToString();
                    comboBox3.Text = kayitokuma.GetValue(7).ToString();
                    maskedTextBox4.Text = kayitokuma.GetValue(8).ToString();
                    break;
                }
                if (kayit_arama_durumu = false)
                    MessageBox.Show("Aranan kayıt bulunamadı!", "Personel Takip Otomasyonu",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                connection.Close();
               
            }
            else
            {
                MessageBox.Show("11 haneli TC No giriniz!", "Personel Takip Otomasyonu",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            string cinsiyet = "";
            
     
                if (pictureBox2.Image == null)
                    button6.ForeColor = Color.Red;
                else
                    button6.ForeColor = Color.Black;
                if (maskedTextBox1.MaskCompleted == false)
                    label13.ForeColor = Color.Red;
                else
                    label13.ForeColor = Color.Black;
                if (maskedTextBox2.MaskCompleted == false)
                    label14.ForeColor = Color.Red;
                else
                    label14.ForeColor = Color.Black;
                if (maskedTextBox3.MaskCompleted == false)
                    label15.ForeColor = Color.Red;
                else
                    label15.ForeColor = Color.Black;
                if (comboBox1.Text == "")
                    label17.ForeColor = Color.Red;
                else
                    label17.ForeColor = Color.Black;
                if (comboBox2.Text == "")
                    label19.ForeColor = Color.Red;
                else
                    label19.ForeColor = Color.Black;
                if (comboBox3.Text == "")
                    label20.ForeColor = Color.Red;
                else
                    label20.ForeColor = Color.Black;
                if (maskedTextBox4.MaskCompleted == false)
                    label21.ForeColor = Color.Red;
                else
                    label21.ForeColor = Color.Black;
                if (int.Parse(maskedTextBox4.Text) < 1000)
                    label21.ForeColor = Color.Red;
                else
                    label21.ForeColor = Color.Black;

                if (pictureBox2.Image != null && maskedTextBox1.MaskCompleted != false && maskedTextBox2.MaskCompleted != false &&
                    maskedTextBox3.MaskCompleted != false && comboBox1.Text != "" && comboBox2.Text != "" &&
                    comboBox3.Text != "" && maskedTextBox4.MaskCompleted != false)
                {
                    if (radioButton3.Checked == true)
                        cinsiyet = "Bay";
                    else if (radioButton4.Checked == true)
                        cinsiyet = "Bayan";
                    try
                    {
                        connection.Open();
                        OracleCommand guncellekomutu = new OracleCommand("update personeller set ad='" +
                            maskedTextBox2.Text + "',soyad='" + maskedTextBox3.Text + "',cinsiyet='" + cinsiyet+ "',mezuniyet='" +
                            comboBox1.Text + "',dogumtarihi='" + dateTimePicker1.Text + "',görevi='" + comboBox2.Text +
                            "',görevyeri='" + comboBox3.Text + "',maasi='" + maskedTextBox4.Text + "' where tcno='"+
                            maskedTextBox1.Text+"'", connection);
                        guncellekomutu.ExecuteNonQuery();
                        connection.Close();
                    personelleri_göster();
                    topPage2_temizle();
                    maskedTextBox4.Text = "0";

                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message, "Personel Takip Otomasyonu",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        connection.Close();
                    }

                }
                else
                    MessageBox.Show("Yazı rengi kırmızı olan alanları yeniden gözden geçiriniz",
                        "Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
          

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.MaskCompleted == true)
            {
                bool kayit_arama_durumu = false;
                connection.Open();
                OracleCommand arama_sorgu = new OracleCommand("select *from personeler where tcno='" +
                    maskedTextBox1.Text + "'", connection);
                OracleDataReader kayitokuma = arama_sorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    OracleCommand deletesorgu = new OracleCommand("delete from personeller where tcno='" +
                        maskedTextBox1.Text + "'", connection);
                    deletesorgu.ExecuteNonQuery();
                    break;
                }
                if (kayit_arama_durumu = false)
                    MessageBox.Show("Silinecek kayit bulunamadı!", "Personel Takip Otomasyonu",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();
                personelleri_göster();
                topPage2_temizle();
                maskedTextBox4.Text = "0";

            }
            else
            {
                MessageBox.Show("Lütfen 11 karkterden oluşan bir TC Kimlik no giriniz!", "Personel Takip Otomasyonu",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                topPage2_temizle();
                maskedTextBox4.Text = "0";

            }
               
        }

        private void button11_Click(object sender, EventArgs e)
        {
            topPage2_temizle();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Form4 frm4 = new Form4();
            frm4.Show();

        }
    }
}
