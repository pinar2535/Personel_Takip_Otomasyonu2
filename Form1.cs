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

namespace Personel_Takip_Otomasyonu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OracleConnection connection = new OracleConnection("User Id=SYSTEM;Password=1234;Data Source=localhost:1521/XEPDB1;");

        //Formlar arası veri aktarımında kullanılacak değişkenler
        public static string tcno, adi, soyadi, yetki;

        // Yerel, yani yalnıza bu formda geçerli olacak değişkenler
        int hak = 3;
        bool durum = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (hak != 0)
            {
                string sorgu1 = "SELECT * FROM kullanicilar";
                connection.Open();
                OracleCommand selectsorgu = new OracleCommand(sorgu1, connection);
                OracleDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    if (radioButton1.Checked == true)
                    {
                        if(kayitokuma["kullaniciadi"].ToString() == textBox1.Text && kayitokuma["parola"].ToString
                            () == textBox2.Text && kayitokuma["yetki"].ToString() == "Yönetici")
                        {
                            durum = true;
                            tcno = kayitokuma.GetValue(0).ToString();
                            adi = kayitokuma.GetValue(1).ToString();
                            soyadi = kayitokuma.GetValue(2).ToString();
                            yetki = kayitokuma.GetValue(3).ToString();
                            this.Hide();
                            Form2 frm2 = new Form2();
                            frm2.Show();
                            break;
                        }
                    }
                    if (radioButton2.Checked == true)
                    {
                        if (kayitokuma["kullaniciadi"].ToString() == textBox1.Text && kayitokuma["parola"].ToString
                            () == textBox2.Text && kayitokuma["yetki"].ToString() == "Kullanıcı")
                        {
                            durum = true;
                            tcno = kayitokuma.GetValue(0).ToString();
                            adi = kayitokuma.GetValue(1).ToString();
                            soyadi = kayitokuma.GetValue(2).ToString();
                            yetki = kayitokuma.GetValue(3).ToString();
                            this.Hide();
                            Form3 frm3 = new Form3();
                            frm3.Show();
                            break;
                        }
                    }
                }
                if (durum == false)
                    hak--;
                connection.Close();
                
            }
            label5.Text = Convert.ToString(hak);
            if (hak == 0)
            {
                button1.Enabled = false;
                MessageBox.Show("Giriş hakkı kalmadı!", "Personel Takip Otomasyonu", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
            }
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text="Kullanıcı Girişi...";
            this.AcceptButton = button1;
            this.CancelButton = button2;
            label5.Text = Convert.ToString(hak);
            radioButton1.Checked = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

           
            
        }
    }
}
