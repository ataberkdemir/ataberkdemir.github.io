using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TRS_1._1
{
    public partial class KAnasayfa : Form
    {
        public int usersNo;
        public string usersAd;
        public string usersSoyad;


        public KAnasayfa(int usersNo, string usersAd)
        {
            InitializeComponent();
            this.usersNo = usersNo;
            this.usersAd = usersAd;
            



            // Timer nesnesini oluşturdum ve Interval özelliğini belirledim
            Timer timer = new Timer();
            timer.Interval = 500;  // 500 milisaniyede bir çalışmasını belirledim

            // Timer nesnesinin Tick olayına bir olay işleyicisi ekledim
            timer.Tick += new EventHandler(timer1_Tick);

            // Timer nesnesini başlattım
            timer.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {            
            label1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            label2.Text = DateTime.Now.ToString("hh:mm:ss");
        }              

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KLog log = new KLog(usersNo);
            log.LogWrite(usersNo + " No'lu Kullanıcı Tüm Toplantıları Görüntüledi");

            button1.Visible = false;

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
            dataGridView1.DataSource = veriSeti.Tables["top_olustur"];

            dataGridView1.Columns[0].HeaderText = "Toplantı Oluşturan";
            dataGridView1.Columns[1].HeaderText = "Toplantı Bölümü";
            dataGridView1.Columns[2].HeaderText = "Toplantı Odası";
            dataGridView1.Columns[3].HeaderText = "Toplantı Tarihi";
            dataGridView1.Columns[4].HeaderText = "Toplantı Saati";
            dataGridView1.Columns[5].HeaderText = "Toplantı Konusu";
            dataGridView1.Columns[6].HeaderText = "Toplantıya Katılacaklar";
            dataGridView1.Columns[7].HeaderText = "Toplantı No";

            dataGridView1.Columns[8].Visible = false;


            command.ExecuteNonQuery();
            bgl.baglantim().Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 back4 = new Form1();
            back4.Show();
            KLog log = new KLog(usersNo);
            log.LogWrite(usersNo + " No'lu Kullanıcı Hesabından Çıkış Yaptı");
        }        

        private void KAnasayfa_Load(object sender, EventArgs e)
        {
            KLog log = new KLog(usersNo);
            log.LogWrite(usersNo + " No'lu Kullanıcı Hesabına Giriş Yaptı");

            textBox1.Text = "Toplantı No / Adınızı Giriniz";

            baglanti bgl = new baglanti();
            SqlConnection connection = bgl.baglantim();
            string sorgu4 = "SELECT users_ad , users_soyad , users_cal_bol , users_profil_foto  FROM dbo.users WHERE users_no = @usersNo";
            string sorgu2 = "DELETE FROM top_olustur WHERE top_tar < GETDATE() AND top_tar != CONVERT(date, GETDATE())";
            string sorgu = "SELECT top_id FROM top_olustur WHERE ',' + top_katilanlar + ',' LIKE '%,' + @users_ad + ',%'";

            using (connection)
            {
                SqlCommand command = new SqlCommand(sorgu2, bgl.baglantim());

                command.ExecuteNonQuery();

            }

            using (connection)
            {
                SqlCommand command = new SqlCommand(sorgu4, bgl.baglantim());
                command.Parameters.AddWithValue("@usersNo", usersNo); // usersNo değişkeninin değeri veritabanındaki users_no'ya eşit olmasını sağlayan parametrem                
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    pictureBox1.ImageLocation = reader["users_profil_foto"].ToString();
                    label10.Text = reader["users_ad"].ToString();
                    label11.Text = reader["users_soyad"].ToString();
                    label12.Text = reader["users_cal_bol"].ToString();
                }
            }

            using (connection)
            {
                int[] toplantiNumaralari = new int[10]; // Bu dizi en fazla 10 tane toplantı numarasını tutabilecek şekilde oluşturuldum.
                int i = 0; // Bu oluşturduğum değişken, dizi içinde kaçıncı indise veri ekleneceğini tutacak

                
                SqlCommand command = new SqlCommand(sorgu, bgl.baglantim());
                command.Parameters.AddWithValue("@users_ad", usersAd);                
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int top_id = reader.GetInt32(0);
                    toplantiNumaralari[i] = top_id;
                    i++;

                }

                string toplantiNumaralariListesi = "";
                for (int j = 0; j < i; j++)
                {
                    toplantiNumaralariListesi += toplantiNumaralari[j] + ", ";
                }
                // Veritabanına bazen hatalı kayıtların girilmesinden doğacak sorunu engellemek için son iki karakterde (virgül ve boşluk) sildim
                if (toplantiNumaralariListesi.Length > 2)
                {
                    toplantiNumaralariListesi = toplantiNumaralariListesi.Substring(0, toplantiNumaralariListesi.Length - 2);
                    label14.Text = toplantiNumaralariListesi + "\nNo'lu toplatı / toplantılara katılımcısınız"; ;
                }
                else
                {
                    label14.Text = "Şu anda bir toplantıya katılımcı değilsiniz";
                }
            }

            bgl.baglantim().Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            baglanti bgl = new baglanti();
            string search = "SELECT top_olusturan , top_bol , top_odasi , top_tar , top_saati , top_konusu , top_katilanlar , top_id FROM dbo.top_olustur WHERE CONCAT(top_id,top_katilanlar) LIKE '%" + textBox1.Text + "%'";
            SqlDataAdapter da = new SqlDataAdapter(search, bgl.baglantim());
            DataTable tb = new DataTable();
            da.Fill(tb);
            dataGridView1.DataSource = tb;
            //textbox'a tıklanma metodunu kullanabilmek için kullandığım kod
            textBox1.Click += textBox1_Click;
        }
        //Arama textBox'ının içinde yazı yazmasını ve tıklandığında silinmesi için kullandığım kod
        private void textBox1_Click(object sender, EventArgs e)
        {
            KLog log = new KLog(usersNo);
            log.LogWrite(usersNo + " No'lu Kullanıcı Toplantılar Arasında Arama Yaptı");

            if (textBox1.Text == "Toplantı No / Adınızı Giriniz")
            {
                textBox1.Clear();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
