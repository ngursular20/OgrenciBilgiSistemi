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

namespace OgrenciBilgiSistemi
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=SEVDE;Database=OgrenciBilgiSistemi;Integrated Security=True;";

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void Aktif_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string ogretmenKimlik = textBox16.Text;
                    SqlCommand ogretmenCmd = new SqlCommand("SELECT COUNT(*) FROM Ogretmenler WHERE OgretmenKimligi = @kimlik", conn);
                    ogretmenCmd.Parameters.AddWithValue("@kimlik", ogretmenKimlik);
                    int ogretmenCount = (int)ogretmenCmd.ExecuteScalar();

                    if (ogretmenCount > 0)
                    {
                        using (SqlCommand cmd = new SqlCommand("Usp_OgrenciEkle", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@ad", textBox1.Text);
                            cmd.Parameters.AddWithValue("@soyad", textBox2.Text);
                            cmd.Parameters.AddWithValue("@numara", textBox3.Text);
                            cmd.Parameters.AddWithValue("@sinif", textBox4.Text);

                            // Check checkBox1 and checkBox2 states to determine the status
                            string status = "";
                            if (checkBox1.Checked)
                                status = "Aktif";
                            else if (checkBox2.Checked)
                                status = "Pasif";

                            cmd.Parameters.AddWithValue("@durum", status); // Add the status parameter

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Veri başarıyla eklendi.");
                            }
                            else
                            {
                                MessageBox.Show("Veri eklenirken bir hata oluştu.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Geçersiz öğretmen kimliği!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }



        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // textBox5 ve textBox6'dan değerleri al
                int ogrenciNumarasi = Convert.ToInt32(textBox5.Text);
                string dersAdi = textBox6.Text;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Bağlantıyı aç
                    conn.Open();

                    // SqlCommand oluştur ve stored procedure'ı belirt
                    using (SqlCommand cmd = new SqlCommand("Usp_OgrenciNotuGetir", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parametreleri ekleyin
                        cmd.Parameters.AddWithValue("@ogrenciNumarasi", ogrenciNumarasi);
                        cmd.Parameters.AddWithValue("@dersAdi", dersAdi);

                        SqlParameter vizeNotOutputParameter = new SqlParameter();
                        vizeNotOutputParameter.ParameterName = "@vizeNot";
                        vizeNotOutputParameter.SqlDbType = SqlDbType.Int;
                        vizeNotOutputParameter.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(vizeNotOutputParameter);

                        SqlParameter finalNotOutputParameter = new SqlParameter();
                        finalNotOutputParameter.ParameterName = "@finalNot";
                        finalNotOutputParameter.SqlDbType = SqlDbType.Int;
                        finalNotOutputParameter.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(finalNotOutputParameter);

                        cmd.ExecuteNonQuery();

                        int vizeNot = Convert.ToInt32(cmd.Parameters["@vizeNot"].Value);
                        int finalNot = Convert.ToInt32(cmd.Parameters["@finalNot"].Value);

                        textBox7.Text = vizeNot.ToString(); // textBox7'ye vize notu yaz
                        textBox14.Text = finalNot.ToString(); // textBox14'e final notu yaz
                        textBox7.ReadOnly = true;
                        textBox14.ReadOnly = true;

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string dersAdi = textBox9.Text;
                int ogrenciNumarasi = Convert.ToInt32(textBox18.Text);
                int vizeNot = Convert.ToInt32(textBox10.Text);
                int finalNot = Convert.ToInt32(textBox13.Text);
                int ogretmenKimligi = Convert.ToInt32(textBox8.Text); // Öğretmen kimliği

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Öğretmen kimliğini kontrol etmek için sorgu oluşturuluyor
                    string ogretmenKontrolQuery = "SELECT COUNT(*) FROM Ogretmenler WHERE OgretmenKimligi = @ogretmenKimligi";
                    using (SqlCommand ogretmenKontrolCmd = new SqlCommand(ogretmenKontrolQuery, conn))
                    {
                        ogretmenKontrolCmd.Parameters.AddWithValue("@ogretmenKimligi", ogretmenKimligi);
                        int ogretmenSayisi = (int)ogretmenKontrolCmd.ExecuteScalar();

                        if (ogretmenSayisi > 0) // Öğretmen kimliği doğru ise not güncelleme işlemi yapılıyor
                        {
                            using (SqlCommand cmd = new SqlCommand("Usp_OgrenciNotGuncelle", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@dersAdi", dersAdi);
                                cmd.Parameters.AddWithValue("@ogrenciNumarasi", ogrenciNumarasi);
                                cmd.Parameters.AddWithValue("@vizeNot", vizeNot);
                                cmd.Parameters.AddWithValue("@finalNot", finalNot);

                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Öğrenci notları güncellendi.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Geçersiz öğretmen kimliği!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int ogrenciNumarasi = Convert.ToInt32(textBox15.Text);
                string ogretmenKimlik = textBox17.Text;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Öğretmen kimliğini kontrol et
                    SqlCommand ogretmenCmd = new SqlCommand("SELECT COUNT(*) FROM Ogretmenler WHERE OgretmenKimligi = @kimlik", conn);
                    ogretmenCmd.Parameters.AddWithValue("@kimlik", ogretmenKimlik);
                    int ogretmenCount = (int)ogretmenCmd.ExecuteScalar();

                    if (ogretmenCount > 0) // Öğretmen kimliği mevcutsa öğrenci silme işlemine devam et
                    {
                        using (SqlCommand cmd = new SqlCommand("Usp_OgrenciSil", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ogrenciNumarasi", ogrenciNumarasi);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Öğrenci başarıyla silindi.");
                            }
                            else
                            {
                                MessageBox.Show("Öğrenci silinirken bir hata oluştu veya öğrenci bulunamadı.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Geçersiz öğretmen kimliği!"); // Geçersiz öğretmen kimliği durumunda hata mesajı göster
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int ogrenciNumarasi = Convert.ToInt32(textBox11.Text);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Öğrenci ortalama hesapla Scalar Function'ı çağırma
                    SqlCommand cmdOrtalama = new SqlCommand("SELECT dbo.OgrenciOrtalamaHesapla(@ogrenciNumarasi)", conn);
                    cmdOrtalama.Parameters.AddWithValue("@ogrenciNumarasi", ogrenciNumarasi);

                    double ortalama = Convert.ToDouble(cmdOrtalama.ExecuteScalar());

                    textBox19.Text = ortalama.ToString("0.##"); // Ortalamayı textBox19'a yazdır
                    textBox19.ReadOnly = true;

                    // Öğrenci toplam kredi hesapla Scalar Function'ı çağırma
                    SqlCommand cmdKredi = new SqlCommand("SELECT dbo.OgrenciToplamKrediHesapla(@ogrenciNumarasi)", conn);
                    cmdKredi.Parameters.AddWithValue("@ogrenciNumarasi", ogrenciNumarasi);

                    int toplamKredi = Convert.ToInt32(cmdKredi.ExecuteScalar());

                    textBox20.Text = toplamKredi.ToString(); // Toplam krediyi textBox20'ye yazdır
                    textBox20.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }



        private void textBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                // Kullanıcı tarafından girilen öğrenci numarası
                string ogrenciNumarasi = textBox12.Text;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();


                    // SQL sorgusu - OgrenciDersNotlari view'inden öğrenci bilgilerini alma
                    string sql = "SELECT * FROM OgrenciDersNotlari WHERE OgrenciNumarasi = @OgrenciNumarasi";

                    // SqlCommand nesnesi oluşturma
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@OgrenciNumarasi", ogrenciNumarasi);

                    // Veri okuyucusu (DataReader)
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Veri varsa, DataGridView'e ekleme (Örnek olarak DataGridView kullanıldı, siz istediğiniz gibi görselleştirebilirsiniz)
                    if (reader.HasRows)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        dataGridView1.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("Öğrenci bulunamadı.");
                    }

                    // Veritabanı bağlantısını kapatma
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                int ogrenciNumarasi = Convert.ToInt32(textBox21.Text);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT dbo.GetStudentStatus(@ogrenciNumarasi)", conn);
                    cmd.Parameters.AddWithValue("@ogrenciNumarasi", ogrenciNumarasi);

                    string durum = cmd.ExecuteScalar()?.ToString(); // Öğrenci durumunu al

                    if (!string.IsNullOrEmpty(durum))
                    {
                        textBox22.Text = durum; // Durumu textBox22'ye yaz
                        textBox22.ReadOnly = true;
                    }
                    else
                    {
                        MessageBox.Show("Öğrenci durumu bulunamadı.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void textBox22_TextChanged(object sender, EventArgs e)
        {

        }
    }
}