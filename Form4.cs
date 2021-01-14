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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        OracleConnection connection = new OracleConnection("User Id=SYSTEM;Password=1234;Data Source=localhost:1521/XEPDB1;");

        private void izinleri_göster()
        {
            try
            {
                connection.Open();
                string sorgu2 = "SELECT * FROM IZINLER";
                OracleDataAdapter izinleri_listele = new OracleDataAdapter
                    (sorgu2, connection);
                DataSet dshafiza = new DataSet();
                izinleri_listele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                connection.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();

            }
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = 11;
            toolTip1.SetToolTip(this.textBox1, "TC Kimlik No 11 Karakter Olmalı!");
            izinleri_göster();

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
            //Kullanıcının rakam girişi dışında herhangi bir işlem yapmasını engelleriz.
            //Rakamların ASCII karşılıklarına göre sınırlar belirlendi.
            if ((int)(e.KeyChar) >= 48 && (int)(e.KeyChar) <= 57 || (int)e.KeyChar == 8)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool kayitkontrol = false;
            connection.Open();
            OracleCommand selectsorgu = new OracleCommand("select * from IZINLER where personeltcno='" +
                textBox1.Text + "'", connection);
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
                if (textBox1.Text.Length < 11 )
                    label1.ForeColor = Color.Red;
                else
                    label1.ForeColor = Color.Black;
                //İzin gün sayısı kontrolu
                if (textBox2.Text.Length < 1)
                    label1.ForeColor = Color.Red;
                else
                    label1.ForeColor = Color.Black;

                if (textBox1.Text.Length==11 && dateTimePicker1.Text != "" && textBox2.Text != "")
                {
                    try
                    {
                        connection.Open();
                        OracleCommand eklekomutu = new OracleCommand("insert into IZINLER values('" + textBox1.Text +
                            "','" + dateTimePicker1.Text + "','" + textBox2.Text + "')", connection);
                        eklekomutu.ExecuteNonQuery();
                        connection.Close();

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
                MessageBox.Show("Girilen TC Kimlik numarasına ait izin daha önceden verilmiştir!",
                    "Personel Takip Otomasyonu", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }
    
    }
    
}
