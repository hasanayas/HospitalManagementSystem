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
using Oracle.ManagedDataAccess.Types;
using System.Configuration;

namespace Hospital
{
   

    public partial class MuayeneKayit : Form
    {
       public string tc2;
        public int db;
        //CHARACTERCASİNG UPPER  - AllowUserToAddRows FALSE

        OracleConnection baglanti = new OracleConnection();
        OracleDataAdapter da;


        public MuayeneKayit()
        {
           
            //full screen
            WindowState = FormWindowState.Maximized;
           

            InitializeComponent();

            //BAĞLANTI
          
            baglanti.ConnectionString = "Data Source=(DESCRIPTION =" +"(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))" +
            "(CONNECT_DATA =" + "(SERVER = DEDICATED)" +"(SERVICE_NAME = oracle.hasan.com)" + ")" + ");User Id = HASAN;password=AYAS";
           

            //POLİKLİNİKLERİ VERİTABANINDA ÇEKME


            OracleCommand komut = new OracleCommand("SELECT * FROM POLIKLINIK", baglanti);
            OracleDataReader dr;
            baglanti.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["POLIKLINIKADI"]);
            }

            //"Select * From  MüsteriTablosu Where Ad like '%" + textBox5.Text + "%' or Soyad like '%" + textBox5.Text + "%'",

         



            //HASTALARI DATAGRİDVİEW'E EKLEME
            da = new OracleDataAdapter("SELECT * FROM HASTA ORDER BY HASTANO", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            //DataGridView, DataTable tipindeki tablonun verilerini aldı.
            dataGridView1.DataSource = tablo;



            baglanti.Close();
            


            //İstenilen Şekilde saat:dakika alma
            String hourMinute = DateTime.Now.ToString("HH.mm");
            txtMuayeneSaat.Text = hourMinute;

            String muayeneTarih = DateTime.Now.ToString(" dd / MM / yyyy");
            txtMuayeneTarihi.Text = muayeneTarih;

            //Buton Çerçeve Kaldırma
            button1.TabStop = false;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;

            //DATAGRİDVİEW TASARIM KODLARI
            
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.SeaGreen; // Çizgi Rengi
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView1.BackgroundColor = Color.FromArgb(30, 30, 30);
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//optional
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 10);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 12);
      


        }

  

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

            // KULLANICI TC'YE GÖRE

            baglanti.ConnectionString = "Data Source=(DESCRIPTION =" + "(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))" +
    "(CONNECT_DATA =" + "(SERVER = DEDICATED)" + "(SERVICE_NAME = oracle.hasan.com)" + ")" + ");User Id = HASAN;password=AYAS";

            baglanti.Open();
            DataTable tbl = new DataTable();
            OracleDataAdapter ara = new OracleDataAdapter("Select * From  HASTA Where TCNO like '%" + textBox4.Text + "%' ORDER BY HASTANO ", baglanti);
            ara.Fill(tbl);
            dataGridView1.DataSource = tbl;
            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            baglanti.ConnectionString = "Data Source=(DESCRIPTION =" + "(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))" +
         "(CONNECT_DATA =" + "(SERVER = DEDICATED)" + "(SERVICE_NAME = oracle.hasan.com)" + ")" + ");User Id = HASAN;password=AYAS";
            baglanti.Open();
         
            string bolum = comboBox1.SelectedItem.ToString();
            comboBox2.Items.Add("deneme"); // Verilerin silinmesi için 1 tane değer attık.
            comboBox2.Items.Clear(); //Comboboxtaki verileri siler
            OracleCommand komut2 = new OracleCommand("SELECT * FROM DOKTOR WHERE POLIKLINIKID in (SELECT POLIKLINIKID FROM POLIKLINIK WHERE POLIKLINIKADI = '"+ bolum +"')", baglanti);
            OracleDataReader dr;
            dr = komut2.ExecuteReader();
            while (dr.Read())
            {
                comboBox2.Items.Add(dr["AD"]);
            }
            
            baglanti.Close();
        }

      

        private void button1_Click_1(object sender, EventArgs e)
        {
            baglanti.ConnectionString = "Data Source=(DESCRIPTION =" + "(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))" +
          "(CONNECT_DATA =" + "(SERVER = DEDICATED)" + "(SERVICE_NAME = oracle.hasan.com)" + ")" + ");User Id = HASAN;password=AYAS";

            baglanti.Open();

            int hastano = Int32.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            string bolum = comboBox1.SelectedItem.ToString();
            string doktorTercihi= comboBox2.SelectedItem.ToString();
            
            //SORGUDAKİ DENEN DEĞERİ ATAMA

            OracleCommand muayeneEkle = new OracleCommand("INSERT INTO MUAYENE (MUAYENETARIHI, HASTASIKAYETI, HASTANO, SEKRETERNO, MUAYENESAATI, HASTABOLUMU,DOKTORTERCIHI) " +
               "VALUES(:p1,:p2,:p3,:p4,:p5,:p6,:p7) ", baglanti);
         
            muayeneEkle.Parameters.Add(":p1", txtMuayeneTarihi.Text);
            muayeneEkle.Parameters.Add(":p2", txtHastaSikayet.Text);
            muayeneEkle.Parameters.Add(":p3", hastano);
            muayeneEkle.Parameters.Add(":p4", label8.Text);
            muayeneEkle.Parameters.Add(":p5", txtMuayeneSaat.Text);
            muayeneEkle.Parameters.Add(":p6", bolum);
            muayeneEkle.Parameters.Add(":p7", doktorTercihi);
            muayeneEkle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("YENİ HASTA KAYDEDİLDİ");
        }

        private void MuayeneKayit_Load(object sender, EventArgs e)
        {

            baglanti.ConnectionString = "Data Source=(DESCRIPTION =" + "(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))" +
          "(CONNECT_DATA =" + "(SERVER = DEDICATED)" + "(SERVICE_NAME = oracle.hasan.com)" + ")" + ");User Id = HASAN;password=AYAS";
            baglanti.Open();
            label9.Text = tc2;
            OracleCommand sorguSekreter = new OracleCommand("SELECT SEKRETERNO FROM SEKRETER WHERE TCNO= :p1", baglanti);
            sorguSekreter.Parameters.Add(":p1", label9.Text);
            OracleDataReader drSekreter = sorguSekreter.ExecuteReader();
            while (drSekreter.Read())
            {
                label8.Text = drSekreter[0] + "";
            }
            baglanti.Close();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            // KULLANICI ARAMA AD VE SOYADA GÖRE

            baglanti.ConnectionString = "Data Source=(DESCRIPTION =" + "(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))" +
      "(CONNECT_DATA =" + "(SERVER = DEDICATED)" + "(SERVICE_NAME = oracle.hasan.com)" + ")" + ");User Id = HASAN;password=AYAS";

            baglanti.Open();
            DataTable tbl = new DataTable();
            OracleDataAdapter ara = new OracleDataAdapter("Select * From  HASTA Where AD like '%" + textBox1.Text + "%' or SOYAD like '%" + textBox1.Text + "%' ", baglanti);
            ara.Fill(tbl);
            dataGridView1.DataSource = tbl;
            baglanti.Close();
        }
    }
}
