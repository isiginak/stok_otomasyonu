using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stokTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public OleDbConnection oleDbCon = new OleDbConnection("Provider=Microsoft.Ace.Oledb.12.0;Data Source=data.accdb");
        public DataTable tablo = new DataTable();
        public OleDbDataAdapter oleDbAdptr = new OleDbDataAdapter();
        public OleDbCommand komut = new OleDbCommand();
        string DosyaYolu, DosyaAdi = "";
        int id;

     public void listele()
        {
            tablo.Clear();
            oleDbCon.Open();
            OleDbDataAdapter oleDbAdptr = new OleDbDataAdapter("select stokAdi,stokModeli,stokSeriNo,stokAdedi,stokTarih,kayitYapan From stok", oleDbCon);
            oleDbAdptr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            oleDbAdptr.Dispose();
            oleDbCon.Close();
            try
            {
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                              
                dataGridView1.Columns[0].HeaderText = "STOK ÜRÜN ADI";
               
                dataGridView1.Columns[1].HeaderText = "STOK PASTA TÜRÜ";
                dataGridView1.Columns[2].HeaderText = "STOK SERİNO";
                dataGridView1.Columns[3].HeaderText = "ÜRÜN MİKTARI";
                dataGridView1.Columns[4].HeaderText = "TARİH";
                dataGridView1.Columns[5].HeaderText = "KAYIT YAPAN";
                dataGridView1.Columns[0].Width = 120;
               
                dataGridView1.Columns[1].Width = 120;
                dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[4].Width = 100;
                dataGridView1.Columns[5].Width = 120;
            }
            catch
            {
                ;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void resimEkleBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string i in openFileDialog1.FileName.Split('\\'))
                {
                    if (i.Contains(".jpg")) { DosyaAdi = i; }
                    else if (i.Contains(".bmp")) { DosyaAdi = i; }
                    else if (i.Contains(".png")) { DosyaAdi = i; }
                    else if (i.Contains(".gif")) { DosyaAdi = i; }
                    else { DosyaYolu += i + "\\"; }
                }
                pictureBox1.ImageLocation = openFileDialog1.FileName;
                
            }
            else
            {
                MessageBox.Show("Herhangibir Resim Girilmedi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            try
            {
                komut = new OleDbCommand("select * from stok where stokSeriNo='" + dataGridView1.CurrentRow.Cells[2].Value.ToString() + "'", oleDbCon);
                oleDbCon.Open();
                OleDbDataReader oku = komut.ExecuteReader();
                oku.Read();
                if (oku.HasRows)
                {
                    pictureBox1.ImageLocation = oku[7].ToString();
                    id = Convert.ToInt32(oku[0].ToString());
                }
                oleDbCon.Close();
            }
            catch
            {
                oleDbCon.Close();
            }
        }

        private void arabtn2_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter oleDbAdptr = new OleDbDataAdapter("select * From stok", oleDbCon);
            if (textBox6.Text.Trim() == "")
            {
                tablo.Clear();
               komut.Connection = oleDbCon;
                komut.CommandText = "Select * from stok";
                oleDbAdptr.SelectCommand = komut;
                oleDbAdptr.Fill(tablo);
            }
            if (Convert.ToBoolean(oleDbCon.State) == false)
            {
                oleDbCon.Open();
            }
            if (textBox6.Text.Trim() != "")
            {
                oleDbAdptr.SelectCommand.CommandText = " Select * From stok" +
                     " where(stokAdi='" + textBox6.Text + "' )";
                tablo.Clear();
                oleDbAdptr.Fill(tablo);
                oleDbCon.Close();
            }
        }

        private void arabtn1_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter oleDbAdptr = new OleDbDataAdapter("select * From stok", oleDbCon);
            if (textBox7.Text.Trim() == "")
            {
                tablo.Clear();
                komut.Connection = oleDbCon;
                komut.CommandText = "Select * from stok";
                oleDbAdptr.SelectCommand = komut;
                oleDbAdptr.Fill(tablo);
            }
            if (Convert.ToBoolean(oleDbCon.State) == false)
            {
                oleDbCon.Open();
            }
            if (textBox7.Text.Trim() != "")
            {
                oleDbAdptr.SelectCommand.CommandText = " Select * From stok" +
                     " where(stokModeli='" + textBox7.Text + "' )";
                tablo.Clear();
                oleDbAdptr.Fill(tablo);
                oleDbCon.Close();
            }
        }

        private void resimSilBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = "";
            DosyaAdi = "";
        }

        private void urunSilBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult cevap;
                cevap = MessageBox.Show("Kaydı silmek istediğinizden eminmisiniz", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (cevap == DialogResult.Yes && dataGridView1.CurrentRow.Cells[0].Value.ToString().Trim() != "")
                {
                    oleDbCon.Open();
                    komut.Connection = oleDbCon;
                    komut.CommandText = "DELETE from stok WHERE stokSeriNo='" + dataGridView1.CurrentRow.Cells[2].Value.ToString() + "' ";
                    komut.ExecuteNonQuery();
                    komut.Dispose();
                    oleDbCon.Close();
                    listele();
                }
            }
            catch
            {
                ;
            }
        }

        private void guncelleBtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() != "" && textBox3.Text.Trim() != "" && textBox4.Text.Trim() != "" && textBox5.Text.Trim() != "")
            {

                
                string sorgu = "UPDATE stok SET stokAdi='" + textBox1.Text + "',stokModeli='" + textBox2.Text + "',stokSeriNo='" + textBox3.Text + "',stokAdedi='" + textBox4.Text + "',stokTarih='" + dateTimePicker1.Text + "',kayitYapan='" + textBox5.Text + "',dosyaAdi='" + DosyaAdi + "' WHERE id=" + id;
                OleDbCommand komut = new OleDbCommand(sorgu, oleDbCon);
                oleDbCon.Open();
                komut.ExecuteNonQuery();
                komut.Dispose();
                oleDbCon.Close();
                listele();
                if (DosyaAdi != "") File.WriteAllBytes(DosyaAdi, File.ReadAllBytes(openFileDialog1.FileName));
                MessageBox.Show("Güncelleme İşlemi Tamamlandı !", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Boş Alanları Doldurunuz !");
            }
        }

        private void cikisBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == false && e.KeyChar != (char)08 && e.KeyChar != (char)44 && e.KeyChar != (char)32)
            {
                e.Handled = true;
                MessageBox.Show("Sadece Harf Girişi Yapabilirsiniz ! ", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == false && e.KeyChar != (char)08 && e.KeyChar != (char)44 && e.KeyChar != (char)32)
            {
                e.Handled = true;
                MessageBox.Show("Sadece Harf Girişi Yapabilirsiniz ! ", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == false && e.KeyChar != (char)08 && e.KeyChar != (char)44 && e.KeyChar != (char)32)
            {
                e.Handled = true;
                MessageBox.Show("Sadece Harf Girişi Yapabilirsiniz ! ", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == false && e.KeyChar != (char)08 && e.KeyChar != (char)44 && e.KeyChar != (char)32)
            {
                e.Handled = true;
                MessageBox.Show("Sadece Harf Girişi Yapabilirsiniz ! ", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) == false && e.KeyChar != (char)08 && e.KeyChar != (char)44 && e.KeyChar != (char)32)
            {
                e.Handled = true;
                MessageBox.Show("Sadece Harf Girişi Yapabilirsiniz ! ", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08 && e.KeyChar != (char)44)
            {
                e.Handled = true;
                MessageBox.Show("Sadece Sayı Girişi Yapabilirsiniz ! ", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void urunEkleBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Trim() == "") errorProvider1.SetError(textBox1, "Boş Alanı Doldurunuz.");

                if (textBox1.Text.Trim() == "") errorProvider1.SetError(textBox1, "Boş Alanı Doldurunuz.");
                else errorProvider1.SetError(textBox1, "");
                if (textBox2.Text.Trim() == "") errorProvider1.SetError(textBox2, "Boş Alanı Doldurunuz.");
                else errorProvider1.SetError(textBox2, "");
                if (textBox3.Text.Trim() == "") errorProvider1.SetError(textBox3, "Boş Alanı Doldurunuz.");
                else errorProvider1.SetError(textBox3, "");
                if (textBox4.Text.Trim() == "") errorProvider1.SetError(textBox4, "Boş Alanı Doldurunuz.");
                else errorProvider1.SetError(textBox4, "");
                if (textBox5.Text.Trim() == "") errorProvider1.SetError(textBox5, "Boş Alanı Doldurunuz.");
                else errorProvider1.SetError(textBox5, "");
                if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() != "" && textBox3.Text.Trim() != "" && textBox4.Text.Trim() != "" && textBox5.Text.Trim() != "")
                {
                    oleDbCon.Open();
                    komut.Connection = oleDbCon;
                    komut.CommandText = "INSERT INTO stok(stokAdi,stokModeli,stokSeriNo,stokAdedi,stokTarih,kayitYapan,dosyaAdi) VALUES ('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + dateTimePicker1.Text + "','" + textBox5.Text + "','" + DosyaAdi + "') ";
                    komut.ExecuteNonQuery();
                    komut.Dispose();
                    oleDbCon.Close();
                    for (int i = 0; i < this.Controls.Count; i++)
                    {
                        if (this.Controls[i] is TextBox) this.Controls[i].Text = "";
                    }
                    listele();
                }
            }
            catch
            {
                MessageBox.Show("Kayıtlı Seri No !");
                oleDbCon.Close();
            }
        }
    }
}
