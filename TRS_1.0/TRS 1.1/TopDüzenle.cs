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
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TRS_1._1
{
    public partial class TopDüzenle : Form
    {

        public int adminNo;

        public TopDüzenle(int adminNo)
        {
            InitializeComponent();
            this.adminNo = adminNo;
            dateTimePicker1.Value = DateTime.Today;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            Log log = new Log(adminNo);
            log.LogWrite(adminNo + " No'lu Admin İşlem Yapmak Üzere Toplantılarını Görüntüledi");

            baglanti bgl = new baglanti();
            bgl.baglantim();

            string set2 = "SELECT * FROM top_olustur WHERE admin_no = @adminNo";
            SqlCommand command2 = new SqlCommand(set2, bgl.baglantim());

            // @adminNo değişkenini tanımladığım ve değerinini atadığım kod bloğu
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

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Anasayfa back2 = new Anasayfa(adminNo);
            back2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox4.Visible = false;
            comboBox2.Visible = false;
            comboBox1.Visible = false;
            dateTimePicker1.Visible = false;
            comboBox3.Visible = false;
            textBox1.Visible = false;
            listBox1.Visible = false;

            label6.Visible = false;
            label5.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label4.Visible = false;
            label3.Visible = false;
            label9.Visible = false;

            button5.Visible = false;

            button1.Visible = true;

            Log log = new Log(adminNo);
            log.LogWrite(adminNo + " No'lu Admin Toplantısını / Toplantılarını Sildi");

            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen toplantınızı silmek için önce\n'Toplantılarını Görüntüle' butonuna tıklayıp\nsilmek istediğiniz toplantıyı seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Seçili olan satırları bir liste içerisinde sakladım
                List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    selectedRows.Add(row);
                }

                baglanti bgl = new baglanti();
                bgl.baglantim();

                // Dizideki her bir satır için bir döngü oluşturdum
                foreach (DataGridViewRow row in selectedRows)
                {
                    // toplanti_no değerini al
                    int toplantiNo = (int)row.Cells["top_id"].Value;

                    // toplanti_no değerine göre veritabanından satırı silin
                    string sorgu = "DELETE FROM top_olustur WHERE top_id = @toplantiNo";
                    SqlCommand command = new SqlCommand(sorgu, bgl.baglantim());
                    command.Parameters.AddWithValue("@toplantiNo", toplantiNo);
                    command.ExecuteNonQuery();
                    // Veritabanından veri silindikten sonra DataGridView'den de satırı kaldırdım
                    dataGridView2.Rows.Remove(row);                    
                }
                MessageBox.Show("Tebrikler Seçtiğiniz/Seçtikleriniz Toplantılar Silindi !");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            //log tutmak için
            button1.Visible = false;

            Log log = new Log(adminNo);
            log.LogWrite(adminNo + " No'lu Admin Toplantısını Düzeltme Olayını Aktif Etti");

            if (dataGridView2.SelectedRows.Count > 0)
            {
                button5.Visible = true;
            }
                

            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen toplantınızı düzenlemek için önce\n'Toplantılarını Görüntüle' butonuna tıklayıp\ndüzenlemek istediğiniz toplantıyı seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Araçların Toplantı Düzenlendiğinde gelmesini istediğim için Load içinde gizlediğim ve butona basıldığında görünürlüklerini aktif ettiğim kodlar

            textBox4.Visible = true;
            comboBox2.Visible = true;
            comboBox1.Visible = true;
            dateTimePicker1.Visible = true;
            comboBox3.Visible = true;
            textBox1.Visible = true;
            listBox1.Visible = true;

            label6.Visible = true;
            label5.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            label4.Visible = true;
            label3.Visible = true;
            label9.Visible = true;

            // Seçili olan satırı aldım
            DataGridViewRow row = dataGridView2.SelectedRows[0];

            baglanti bgl = new baglanti();

            try
            {
            //Verileri textBox comboBox gibi araçlara yazdırmak için seçili satırın içinde yer alan verileri değişkenlere atadığım.
            //Ve boş olmamasını istediğim ve bunu "?" ile sağladığım kod

                string toplantiOlusturan = row.Cells["top_olusturan"].Value?.ToString();
                string toplantiBolumu = row.Cells["top_bol"].Value?.ToString();
                string toplantiOdasi = row.Cells["top_odasi"].Value?.ToString();
                string toplantiTarihi = row.Cells["top_tar"].Value?.ToString();
                string toplantiSaati = row.Cells["top_saati"].Value?.ToString();
                string toplantiKonusu = row.Cells["top_konusu"].Value?.ToString();
                string toplantiKatilacaklar = row.Cells["top_katilanlar"].Value?.ToString();
                
            //Daha önceden oluşturulan toplantıların veritabanında saklanan verilerini textBox comboBox'lara yazdırmak için kullandığım kod

                textBox4.Text = toplantiOlusturan;
                comboBox2.Text = toplantiBolumu;
                comboBox1.Text = toplantiOdasi;
                dateTimePicker1.Text = toplantiTarihi;
                comboBox3.Text = toplantiSaati;
                textBox1.Text = toplantiKonusu;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Lütfen boş bir satır seçmeyiniz.");
            }            



            string sorgu5 = "SELECT users_ad FROM dbo.users";
            SqlCommand comand = new SqlCommand(sorgu5, bgl.baglantim());
            SqlDataReader reader = comand.ExecuteReader();

            listBox1.Items.Clear();
            while (reader.Read())
            {
                string dizgi = reader.GetString(0);


                foreach (string parca in dizgi.Split(','))
                {
                    listBox1.Items.Add(parca);
                }
            }

            reader.Close();
            bgl.baglantim().Close();          
            

            //listbox içinde toplu seçim yapılmasını sağlayan kodum

            listBox1.SelectionMode = SelectionMode.MultiSimple;

            //Güncellenmek istenen toplantının katılımcılarının seçimini kaldıran kod bloğum

            foreach (var item in listBox1.SelectedItems)
            {
                listBox1.Items.Remove(item);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {            
            Log log = new Log(adminNo);
            log.LogWrite(adminNo + " No'lu Admin Toplantısının Verilerini Güncelledi");

            //Toplantı verilerini güncellemeden önce son bir kez emin olup olmadığını sorduğum ve içerisinde listbox'taki veriler için bir döngünün bulunduğu kod bloğu

            if (MessageBox.Show("Güncellemek İstediğinizden Emin Misiniz?", "Dikkat", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                textBox4.Visible = false;
                comboBox2.Visible = false;
                comboBox1.Visible = false;
                dateTimePicker1.Visible = false;
                comboBox3.Visible = false;
                textBox1.Visible = false;
                listBox1.Visible = false;

                label6.Visible = false;
                label5.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label4.Visible = false;
                label3.Visible = false;
                label9.Visible = false;

                button5.Visible = false;

                button1.Visible = true;

                baglanti bgl = new baglanti();
                bgl.baglantim();
                DataGridViewRow row = dataGridView2.SelectedRows[0];
                int toplantiNo = (int)row.Cells["top_id"].Value;
                string sorgu = "UPDATE top_olustur SET top_olusturan = @top_olusturan, top_bol = @top_bol, top_katilanlar = @top_katilanlar , top_odasi = @top_odasi, top_tar = @top_tar, top_saati = @top_saati, top_konusu = @top_konusu WHERE top_id = @toplantiNo";
                SqlCommand command = new SqlCommand(sorgu, bgl.baglantim());

                string secilenler = "";
                foreach (string item in listBox1.SelectedItems)
                {
                    secilenler += item + ",";
                }

                // Veritabanı sorgusunda kullanılacak parametreleri tanımla ve değerlerini atamak için kullanılan kod bloğu
                command.Parameters.AddWithValue("@toplantiNo", toplantiNo);
                command.Parameters.AddWithValue("@top_olusturan", textBox4.Text);
                command.Parameters.AddWithValue("@top_bol", comboBox2.Text);
                command.Parameters.AddWithValue("@top_odasi", comboBox1.Text);
                command.Parameters.AddWithValue("@top_tar", Convert.ToDateTime(dateTimePicker1.Text));
                command.Parameters.AddWithValue("@top_saati", comboBox3.Text);
                command.Parameters.AddWithValue("@top_konusu", textBox1.Text);
                command.Parameters.AddWithValue("@top_katilanlar", secilenler);
                MessageBox.Show("Tebrikler Toplantı Güncellendi !");

                command.ExecuteNonQuery();
            }

                if (dataGridView2.SelectedRows.Count == 0)
                {
                MessageBox.Show("Lütfen toplantınızı güncellemek için önce\n'Toplantılarını Görüntüle' butonuna tıklayıp\ntoplantınızın bilgilerini güncelledikten sonra\n'Toplantıyı Güncelle' butonunu kullanın !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
                }           
        }

        private void TopDüzenle_Load(object sender, EventArgs e)
        {
            // Toplantı Düzenleme Form Sayfası açıldığında gözükmemesi gereken araçları gizlediğim kodlar.

            textBox4.Visible = false;
            comboBox2.Visible = false;
            comboBox1.Visible = false;
            dateTimePicker1.Visible = false;
            comboBox3.Visible = false;
            textBox1.Visible = false;
            listBox1.Visible = false;

            label6.Visible = false;
            label5.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label4.Visible = false;
            label3.Visible = false;
            label9.Visible = false;

            button5.Visible = false;
        }

        
        //Log kaydı için kullandığım metod.
        public void logtut(string value)
        {
            string koşul = string.Format(value, adminNo);
            Log b = new Log(koşul);
        }
    }
}
