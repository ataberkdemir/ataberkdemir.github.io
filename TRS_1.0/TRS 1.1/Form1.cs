using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TRS_1._1
{
    public partial class Form1 : Form
    {
        public int adminNo;
        public static int adminNo2;

        public Form1()
        {
            InitializeComponent();            
        }        

        public void button1_Click(object sender, EventArgs e)
        {
            baglanti bgl = new baglanti();

            string sorgu = "SELECT admin_no FROM dbo.admin WHERE admin_id=@admin_id AND admin_pswrd=@admin_pswrd";
            SqlCommand comand = new SqlCommand(sorgu)
            {
                Connection = bgl.baglantim()
            };

            comand.Parameters.AddWithValue("@admin_id", textBox1.Text);
            comand.Parameters.AddWithValue("@admin_pswrd", textBox2.Text);

            SqlDataReader reader = comand.ExecuteReader();

            if (reader.Read())
            {
                // Burada, kullanıcının admin_no değerini "adminNo" değişkeninde sakladım.
                // Daha sonra, bu değişkeni diğer formlarımda kullanarak profil bilgisi oluşturabilir ve "top_olustur" tablosundaki verileri filtreleyebilirim.

                int adminNo = reader.GetInt32(0);
                
                Log log = new Log(adminNo);
                log.LogWrite(adminNo + " No'lu Admin Toplantı Randevu Sistemine Giriş Yaptı");

                Log form4 = new Log(adminNo);
                form4.adminNo = adminNo; // Burada, "form4" nesnesinin "adminNo" özelliğine "adminNo" değişkenini atadım

                Toplantı_Oluştur form3 = new Toplantı_Oluştur(adminNo);
                form3.adminNo = adminNo; // Burada, "form3" nesnesinin "adminNo" özelliğine "adminNo" değişkenini atadım

                Anasayfa anasayfaForm = new Anasayfa(adminNo);
                anasayfaForm.adminNo = adminNo; // Burada, "anasayfaForm" nesnesinin "adminNo" özelliğine "adminNo" değişkenini atadım

                TopDüzenle form2 = new TopDüzenle(adminNo);
                form2.adminNo = adminNo; // Burada, "form2" nesnesinin "adminNo" özelliğine "adminNo" değişkenini atadım

                anasayfaForm.Show();
                MessageBox.Show("Admin olarak giriş yaptınız. Admin sayfasını görüntülemektesiniz.");
                this.Hide();
                bgl.baglantim().Close();
            }

            else
            {
                reader.Close();
                string sorgu2 = "SELECT users_no , users_ad , users_soyad FROM dbo.users WHERE users_id=@users_id AND users_pswrd=@users_pswrd";
                SqlCommand comand2 = new SqlCommand(sorgu2)
                {
                    Connection = bgl.baglantim()
                };
                comand2.Parameters.AddWithValue("@users_id", textBox1.Text);
                comand2.Parameters.AddWithValue("@users_pswrd", textBox2.Text);
                SqlDataReader reader2 = comand2.ExecuteReader();

                if (reader2.Read())
                {
                    
                    int usersNo = reader2.GetInt32(0);
                    string usersAd = reader2.GetString(1).Trim();
                    string usersSoyad = reader2.GetString(2).Trim();
                    KAnasayfa Kanasayfa1 = new KAnasayfa(usersNo, usersAd);
                    Kanasayfa1.usersNo = usersNo;
                    Kanasayfa1.usersAd = usersAd;

                    KLog form4 = new KLog(usersNo);
                    form4.usersNo = usersNo;

                    Kanasayfa1.Show();
                    MessageBox.Show("Kullanıcı olarak giriş yaptınız. Kullanıcı sayfasını görüntülemektesiniz.");
                    this.Hide();
                    bgl.baglantim().Close();
                    reader2.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı Adınız Veya Şifreniz Hatalı");
                }
            }            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            textBox2.PasswordChar = '*';
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Checkbox'ın "Checked" özelliğini kontrol ediyorum
            if (checkBox1.Checked)
            {
                // Butona basıldığında şifre alanını "görünür" hale getiriyorum
                textBox2.PasswordChar = '\0';
            }
            else
            {
                // Checkbox tekrar inaktif hale geldiğin (ilk konumuna) şifre alanını "gizli" hale getiriyorum
                textBox2.PasswordChar = '*';
            }
        }
        //Log kaydı için kullandığım metod.
        public void logtut(string value)
        {
            string koşul = string.Format(value, adminNo);
            Log a = new Log(koşul);
        }
    }
}
