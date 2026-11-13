using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TRS_1._1
{
    public partial class Toplantı_Oluştur : Form
    {
        public int adminNo;

        public Toplantı_Oluştur(int adminNo)
        {
            InitializeComponent();
            this.adminNo = adminNo;
            dateTimePicker1.Value = DateTime.Today;
        }       

        public void button2_Click(object sender, EventArgs e)
        {
            Log log = new Log(adminNo);
            log.LogWrite(adminNo + " No'lu Admin Toplantı Oluşturdu");

            baglanti bgl = new baglanti();

            string sql = "SELECT COUNT(*) FROM top_olustur WHERE top_tar = @top_tar AND top_saati = @top_saati AND top_odasi = @top_odasi";
            SqlCommand command = new SqlCommand(sql, bgl.baglantim());
            command.Parameters.AddWithValue("@top_tar", Convert.ToDateTime(dateTimePicker1.Text));
            command.Parameters.AddWithValue("@top_saati", comboBox3.Text);
            command.Parameters.AddWithValue("@top_odasi", comboBox1.Text);

            // Kontrol sorgusunun sonucu (yani, aynı tarih, saat ve toplantı odasına ait kayıt sayısı) bir değişkende sakladım.
            int count = (int)command.ExecuteScalar();

            // Eğer aynı tarih, saat ve toplantı odasına ait kayıt yoksa (yani, count değişkeni 0'a eşitse),
            // yeni toplantı verisini/verilerini veritabanına ekledim.
            // Count 0 dan büyük ise bilgilerin çakıştığını mesaj olarak yazdırdım.
            if (count > 0)
            {
                MessageBox.Show("Seçtiğiniz tarih, saat ve toplantı odası doludur. Lütfen farklı bir tarih, saat veya toplantı odası seçiniz.");
            }
            else
            {
               


                string insertSql = "INSERT INTO top_olustur (top_olusturan , top_bol , top_odasi , top_tar , top_saati , top_konusu , top_katilanlar, admin_no) VALUES (@top_olusturan , @top_bol , @top_odasi , @top_tar, @top_saati , @top_konusu , @top_katilanlar, @adminNo)";
                command = new SqlCommand(insertSql, bgl.baglantim());

                command.Parameters.AddWithValue("@top_olusturan", textBox4.Text);
                command.Parameters.AddWithValue("@top_bol", comboBox2.Text);
                command.Parameters.AddWithValue("@top_odasi", comboBox1.Text);
                command.Parameters.AddWithValue("@top_tar", Convert.ToDateTime(dateTimePicker1.Text));
                command.Parameters.AddWithValue("@top_saati", comboBox3.Text);
                command.Parameters.AddWithValue("@top_konusu", textBox1.Text);


                string[] alanlar = { textBox4.Text, comboBox2.Text, comboBox1.Text, dateTimePicker1.Text, comboBox3.Text, textBox1.Text, listBox1.Text };
                string[] etiketler = { "Toplantı Oluşturan", "Toplantı Bölümü", "Toplantı Odası", "Toplantı Tarihi", "Toplantı Saati", "Toplantı Konusu", "Toplantıya Katılacaklar" };

                for (int i = 0; i < alanlar.Length; i++)
                {
                    if (string.IsNullOrEmpty(alanlar[i]))
                    {
                        MessageBox.Show($"Lütfen {etiketler[i]} alanını doldurun.");
                        return;
                    }
                }


                string selectedItems = "";
                foreach (var item in listBox1.SelectedItems)
                {
                    selectedItems += item.ToString().Trim() + ",";
                }
                selectedItems = selectedItems.TrimEnd(',');

                command.Parameters.AddWithValue("@top_katilanlar", selectedItems);                                


                command.Parameters.AddWithValue("@adminNo", this.adminNo);

                string sorgu = "SELECT users_ad FROM dbo.users";
                SqlCommand comand = new SqlCommand(sorgu, bgl.baglantim());
                SqlDataReader reader = comand.ExecuteReader();

                listBox1.Items.Clear();
                while (reader.Read())
                {                    
                    listBox1.Items.Add(reader.GetString(0));                    
                }

                reader.Close();
                bgl.baglantim().Close();
                command.ExecuteNonQuery();
                
                MessageBox.Show("Tebrikler başarıyla toplantı oluşturdunuz!");
            }               
            
        }        

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Anasayfa back = new Anasayfa(adminNo);
            back.Show();
        }

        private void Toplantı_Oluştur_Load(object sender, EventArgs e)
        {
            baglanti bgl = new baglanti(); // Veritabanı bağlantısını sağlayan nesnem  
                            

            try
            {
                textBox4.ReadOnly = false;
                string sql = "SELECT admin_ad, admin_soyad FROM dbo.admin WHERE admin_no = @admin_no";
                SqlCommand command = new SqlCommand(sql, bgl.baglantim());
                command.Parameters.AddWithValue("@admin_no", adminNo);
                using (SqlDataReader reader2 = command.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        string adminAd = reader2.GetString(0);
                        string adminSoyad = reader2.GetString(1);
                        textBox4.Text = adminAd + " " + adminSoyad;
                        bgl.baglantim().Close();
                        reader2.Close();
                    }                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bir hata oluştu: " + ex.Message);
            }

            listBox1.SelectionMode = SelectionMode.MultiSimple;
                      

            string sorgu = "SELECT users_ad FROM dbo.users";
            SqlCommand comand = new SqlCommand(sorgu)
            {
                Connection = bgl.baglantim()
            };
            SqlDataReader reader = comand.ExecuteReader();

            while (reader.Read())
            {
                
                listBox1.Items.Add(reader.GetString(0));
            }

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.Items[i] = listBox1.Items[i].ToString().Replace(" ", " ");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Log log = new Log(adminNo);
            log.LogWrite(adminNo + " No'lu Admin Kendi Toplantılarını Görüntüledi");

            baglanti bgl = new baglanti();
            bgl.baglantim();

            string set2 = "SELECT * FROM top_olustur WHERE admin_no = @adminNo";
            SqlCommand command2 = new SqlCommand(set2, bgl.baglantim());

            // @adminNo değişkenini tanımla ve değerini atadım
            SqlParameter param = new SqlParameter("@adminNo", SqlDbType.Int);
            param.Value = adminNo;
            command2.Parameters.Add(param);
            SqlDataAdapter adapter2 = new SqlDataAdapter(command2);


            // Verileri içerecek bir DataSet nesnesi oluşturdum
            DataSet veriSeti2 = new DataSet();

            // Veritabanından verileri çekin ve DataSet nesnesine yükledim
            adapter2.Fill(veriSeti2, "top_olustur");

            dataGridView2.DataSource = veriSeti2.Tables["top_olustur"];

            dataGridView2.Columns[0].HeaderText = "Toplantı Oluşturan";
            dataGridView2.Columns[1].HeaderText = "Toplantı Bölümü";
            dataGridView2.Columns[2].HeaderText = "Toplantı Odası";
            dataGridView2.Columns[3].HeaderText = "Toplantı Tarihi";
            dataGridView2.Columns[4].HeaderText = "Toplantı Saati";
            dataGridView2.Columns[5].HeaderText = "Toplantı Konusu";
            dataGridView2.Columns[6].HeaderText = "Toplantıya Katılacaklar";
            dataGridView2.Columns[7].HeaderText = "Toplantı No";

            dataGridView2.Columns[8].Visible = false;


            command2.ExecuteNonQuery();
            bgl.baglantim().Close();
        }
        //Log kaydı için kullandığım metod.
        public void logtut(string value)
        {
            string koşul = string.Format(value, adminNo);
            Log b = new Log(koşul);
        }
    }
}
