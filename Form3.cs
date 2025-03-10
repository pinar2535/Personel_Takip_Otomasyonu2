﻿using System;
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

namespace Personel_Takip_Otomasyonu
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        OracleConnection connection = new OracleConnection("User Id=SYSTEM;Password=1234;Data Source=localhost:1521/XEPDB1;");
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
                dataGridView1.DataSource = dshafiza.Tables[0];
                connection.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();

            }
        }




        private void Form3_Load(object sender, EventArgs e)
        {
            personelleri_göster();
            this.Text = "Kullanıcı İşlemleri";
            label19.Text = Form1.adi + "" + Form1.soyadi;
            pictureBox1.Height = 150;
            pictureBox1.Width = 150;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;

            pictureBox2.Height = 150;
            pictureBox2.Width = 150;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;

            try
            {
                pictureBox2.Image = Image.FromFile(Application.StartupPath+"\\kullaniciresimler\\"+
                   Form1.tcno+".jpg");

            }
            catch 
            {
                pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\resimyokk.jpg");

            }
            maskedTextBox1.Mask = "00000000000";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;
            if (maskedTextBox1.Text.Length == 11)
            {
                connection.Open();
                OracleCommand selectsorgu = new OracleCommand("Select * from Personeller where tcno='" +
                    maskedTextBox1.Text + "'", connection);
                OracleDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    try
                    {
                        pictureBox1.Image = Image.FromFile(Application.StartupPath +
                            "\\personelresimler\\" + kayitokuma.GetValue(0) + ".jpg");

                    }
                    catch
                    {
                        pictureBox1.Image = Image.FromFile(Application.StartupPath +
                           "\\personelresimler\\resimyokk.jpg");
                    }
                    label10.Text = kayitokuma.GetValue(1).ToString();
                    label11.Text = kayitokuma.GetValue(2).ToString();

                    if (kayitokuma.GetValue(3).ToString() == "Bay")
                        label12.Text = "Bay";
                    else
                        label12.Text = "Bayan";
                    label13.Text = kayitokuma.GetValue(4).ToString();
                    label14.Text = kayitokuma.GetValue(5).ToString();
                    label15.Text = kayitokuma.GetValue(6).ToString();
                    label16.Text = kayitokuma.GetValue(7).ToString();
                    label17.Text = kayitokuma.GetValue(8).ToString();
                    break;
                }
                if (kayit_arama_durumu == false)

                    MessageBox.Show("Aranan kayıt bulunamadı!", "Personel Takip Otomasyonu",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();


            }
            else
                MessageBox.Show("11 haneli TC Kimlik No giriniz");
        }
    }
}
