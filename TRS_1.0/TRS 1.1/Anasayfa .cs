using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRS_1._1
{
    public partial class Anasayfa : Form
    {
        public int adminNo;

        public Anasayfa(int adminNo)
        {
            InitializeComponent();
            this.adminNo = adminNo;

            // Timer nesnesini oluşturdum ve Interval özelliğini belirledim
            Timer timer = new Timer();
            timer.Interval = 500;  // 500 milisaniyede bir çalışmasını belirledim

            // Timer nesnesinin Tick olayına bir olay işleyicisi ekledim
            timer.Tick += new EventHandler(timer1_Tick);

            // Timer nesnesini başlattım
            timer.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti bgl = new baglanti();
            
            Log log = new Log(adminNo);
            log.LogWrite(adminNo + " No'lu Admin Toplantı Oluşturma Sayfasına Giriş Yaptı");

            try
            {
                bgl.baglantim();
                string sql1 = "SELECT admin_ad, admin_soyad FROM dbo.admin WHERE admin_no = @adminNo";
                SqlCommand command3 = new SqlCommand(sql1, bgl.baglantim());
                command3.Parameters.AddWithValue("@admin_no", adminNo);
                using (SqlDataReader reader2 = command3.ExecuteReader())
                {
                    if (reader2.Read())
                    {
                        string adminAd = reader2.GetString(0);
                        string adminSoyad = reader2.GetString(1);

                        // Toplantı oluşturan textBox'a giriş yapılan admin_ad ve admin_soyad verilerini otomatik yazdırmak istedim.
                        MessageBox.Show("admin_ad" + adminAd + "admin_soyad" + adminSoyad);
                    }
                    else
                    {
                        Console.WriteLine("Sorgu hiçbir sonuç döndürmedi.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bir hata oluştu: " + ex.Message);
            }

            string sql = "INSERT INTO top_olustur (admin_no) VALUES (@adminNo)";
            SqlCommand command = new SqlCommand(sql, bgl.baglantim());                        
            Toplantı_Oluştur toplantiOlustur = new Toplantı_Oluştur(adminNo);            
            toplantiOlustur.Show();
            this.Hide();
        }

        private void Anasayfa_Load(object sender, EventArgs e)
        {
            Log log = new Log(adminNo);
            log.LogWrite(adminNo + " No'lu Admin Hesabına Giriş Yaptı");

            baglanti bgl = new baglanti();            
            SqlConnection connection = bgl.baglantim();
            string sorgu4 = "SELECT admin_ad , admin_soyad , cal_bol , profil_foto  FROM dbo.admin WHERE admin_no = @adminNo";
            string sorgu2 = "DELETE FROM top_olustur WHERE top_tar < GETDATE() AND top_tar != CONVERT(date, GETDATE())";

            using (connection)
            {
                SqlCommand command = new SqlCommand(sorgu2, bgl.baglantim());
                
                command.ExecuteNonQuery();
                
            }

            using (connection)
            {
                SqlCommand command = new SqlCommand(sorgu4, bgl.baglantim());
                command.Parameters.AddWithValue("@adminNo", adminNo); // adminNo değişkeninin değerinin veritabanındaki admin_no'ya eşit olmasını sağlayan parametrem.          
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    pictureBox1.ImageLocation = reader["profil_foto"].ToString();
                    label10.Text = reader["admin_ad"].ToString();
                    label11.Text = reader["admin_soyad"].ToString();
                    label12.Text = reader["cal_bol"].ToString();
                }
            }
            bgl.baglantim().Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Log log = new Log(adminNo);
            log.LogWrite(adminNo + " No'lu Admin Tüm Toplantıları Görüntüledi");

            baglanti bgl = new baglanti();
            bgl.baglantim();

            string set = "SELECT * FROM top_olustur";
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM top_olustur", bgl.baglantim());
            SqlCommand command = new SqlCommand(set, bgl.baglantim());

            // Verileri içerecek bir DataSet nesnesi oluşturdum
            DataSet veriSeti = new DataSet();

            // Veritabanından verileri çekin ve DataSet nesnesine yükledim
            adapter.Fill(veriSeti, "top_olustur");
            
            // Formunuzda oluşturduğunuz DataGridView kontrolünün DataSource özelliğini DataSet nesnesine ayarladım
            tümtopDGV.DataSource = veriSeti.Tables["top_olustur"];

            tümtopDGV.Columns[0].HeaderText = "Toplantı Oluşturan";
            tümtopDGV.Columns[1].HeaderText = "Toplantı Bölümü";
            tümtopDGV.Columns[2].HeaderText = "Toplantı Odası";
            tümtopDGV.Columns[3].HeaderText = "Toplantı Tarihi";
            tümtopDGV.Columns[4].HeaderText = "Toplantı Saati";
            tümtopDGV.Columns[5].HeaderText = "Toplantı Konusu";
            tümtopDGV.Columns[6].HeaderText = "Toplantıya Katılacaklar";
            tümtopDGV.Columns[7].HeaderText = "Toplantı No";

            tümtopDGV.Columns[8].Visible = false;
            

            command.ExecuteNonQuery();
            bgl.baglantim().Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Log log = new Log(adminNo);
            log.LogWrite(adminNo + " No'lu Admin Toplantı Düzeltme Sayfasına Giriş Yaptı");

            TopDüzenle toplantıdüzenleform = new TopDüzenle(adminNo);
            toplantıdüzenleform.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 back3 = new Form1();
            back3.Show();
            Log log = new Log(adminNo);
            log.LogWrite(adminNo + " No'lu Admin Hesabından Çıkış Yaptı ");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            label2.Text = DateTime.Now.ToString("hh:mm:ss");
        }
        //Log kaydı için kullandığım metod.
        public void logtut(string value)
        {
            string koşul = string.Format(value, adminNo);
            Log b = new Log(koşul);
        }

      
    }
}
