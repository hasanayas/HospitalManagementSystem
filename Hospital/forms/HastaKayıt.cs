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
using System.Windows;

//EXPORT İÇİN AKTİF EDİLMELİDİR
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Imaging;



namespace Hospital
{
    public partial class HastaKayıt : Form
    {
       
        OracleConnection con = null;

        private void setConnection()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception exp)
            {

            }
        }

            void temizle(){txtTC.Clear(); txtAD.Clear();txtSOYAD.Clear();txtTELNO.Clear();txtBOY.Clear();txtKILO.Clear();}

        void tasarım()
        {
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
            //dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 9);
            //dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9);
        }

        private void updateDataGrid()
        {
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM HASTA ORDER BY HASTANO";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();
        }
        string cinsiyet = "E";
        string kangrubu = "B+";

        private void AUD(String sql_stmt, int state)
        {
            String msg = "";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = CommandType.Text;

            //RADİO BUTON VE CHECKBOXKOŞULLARI

            if (checkBox2.Checked == true) { cinsiyet = "K"; }
            else if (checkBox1.Checked == true) { cinsiyet = "E"; }


            if (radioButton3.Checked == true) { kangrubu = "A+"; }
            else if (radioButton4.Checked == true) { kangrubu = "A-"; }
            else if (radioButton5.Checked == true) { kangrubu = "B+"; }
            else if (radioButton6.Checked == true) { kangrubu = "B-"; }
            else if (radioButton7.Checked == true) { kangrubu = "AB+"; }
            else { kangrubu = "AB-"; }

            switch (state)
            {
                case 0:
                    msg = "Başarıyla Eklendi";
                    cmd.Parameters.Add("TCNO", OracleDbType.Char, 11).Value = txtTC.Text;
                    cmd.Parameters.Add("AD", OracleDbType.Varchar2, 30).Value = txtAD.Text;
                    cmd.Parameters.Add("SOYAD", OracleDbType.Varchar2, 30).Value = txtSOYAD.Text;
                    cmd.Parameters.Add("CINSIYET", OracleDbType.Char, 1).Value = cinsiyet;
                    cmd.Parameters.Add("DOGUMTARIHI", OracleDbType.Date).Value = this.dateTimePicker1.Text;
                    cmd.Parameters.Add("TELEFONNO", OracleDbType.Char, 11).Value = txtTELNO.Text;
                    cmd.Parameters.Add("KANGRUBU", OracleDbType.Char, 4).Value = kangrubu;
                    cmd.Parameters.Add("BOY", OracleDbType.Int32, 3).Value = Int32.Parse(txtBOY.Text);
                    cmd.Parameters.Add("KILO", OracleDbType.Int32, 3).Value = Int32.Parse(txtKILO.Text);
                    break;

                case 1:
                    cmd.Parameters.Add("TCNO", OracleDbType.Char, 11).Value = txtTC.Text;
                    cmd.Parameters.Add("AD", OracleDbType.Varchar2, 30).Value = txtAD.Text;
                    cmd.Parameters.Add("SOYAD", OracleDbType.Varchar2, 30).Value = txtSOYAD.Text;
                    cmd.Parameters.Add("CINSIYET", OracleDbType.Char, 1).Value = cinsiyet;
                    cmd.Parameters.Add("DOGUMTARIHI", OracleDbType.Date).Value = this.dateTimePicker1.Text;
                    cmd.Parameters.Add("TELEFONNO", OracleDbType.Char, 11).Value = txtTELNO.Text;
                    cmd.Parameters.Add("KANGRUBU", OracleDbType.Char, 4).Value = kangrubu;
                    cmd.Parameters.Add("BOY", OracleDbType.Int32, 3).Value = Int32.Parse(txtBOY.Text);
                    cmd.Parameters.Add("KILO", OracleDbType.Int32, 3).Value = Int32.Parse(txtKILO.Text);

                    msg = "Başarıyla Güncellendi";
                    break;


                case 2:
                    cmd.Parameters.Add("TCNO", OracleDbType.Char, 11).Value = txtTC.Text;
                    msg = "Başarıyla Silindi";
                    break;

            }

            try
            {
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.updateDataGrid();
                }
            }
            catch (Exception expe) { }

        }

        public HastaKayıt()
        {
            //full screen
            WindowState = FormWindowState.Maximized;

            this.setConnection();
            InitializeComponent();
        }

        private void HastaKayıt_Load(object sender, EventArgs e)
        {
            tasarım();
            this.updateDataGrid();
        }

        //YENİ HASTA EKLER
        private void btnEkle_Click(object sender, EventArgs e)
        {
            string cinsiyet = "E";
            string kangrubu = "B+";

            if (checkBox2.Checked == true) { cinsiyet = "K"; } else if (checkBox1.Checked == true) { cinsiyet = "E"; }


            if (radioButton3.Checked == true) { kangrubu = "A+"; }
            else if (radioButton4.Checked == true) { kangrubu = "A-"; }
            else if (radioButton5.Checked == true) { kangrubu = "B+"; }
            else if (radioButton6.Checked == true) { kangrubu = "B-"; }
            else if (radioButton7.Checked == true) { kangrubu = "AB+"; }
            else { kangrubu = "AB-"; }


            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();
            OracleCommand hastaekle = new OracleCommand("INSERT INTO HASTA(TCNO, AD, SOYAD, CINSIYET, DOGUMTARIHI, TELEFONNO, KANGRUBU, BOY, KILO) " +
                "VALUES (:p1,:p2,:p3,:p4,:p5,:p6,:p7,:p8,:p9) ",con);
            hastaekle.Parameters.Add(":p1", txtTC.Text);
            hastaekle.Parameters.Add(":p2", txtAD.Text);
            hastaekle.Parameters.Add(":p3", txtSOYAD.Text);
            hastaekle.Parameters.Add(":p4",cinsiyet);
            hastaekle.Parameters.Add(":p5", this.dateTimePicker1.Text);
            hastaekle.Parameters.Add(":p6",txtTELNO.Text);
            hastaekle.Parameters.Add(":p7",kangrubu);
            hastaekle.Parameters.Add(":p8",txtBOY.Text);
            hastaekle.Parameters.Add(":p9",txtKILO.Text);
            hastaekle.ExecuteNonQuery();
            this.updateDataGrid();
            con.Close();
            MessageBox.Show("YENİ HASTA KAYDEDİLDİ");
       
        }

        //HASTAYI GÜNCELLER
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            String sql = "UPDATE HASTA SET TCNO = :TCNO," +
                "AD= :AD, SOYAD= :SOYAD, CINSIYET= :CINSIYET,DOGUMTARIHI= :DOGUMTARIHI," +
                " TELEFONNO = :TELEFONNO, KANGRUBU =: KANGRUBU, BOY =:BOY, KILO= :KILO WHERE TCNO = :TCNO ";
            this.AUD(sql, 1);
        }

        //HASTAYI SİLME
        private void btnSil_Click(object sender, EventArgs e)
        {

            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();
            OracleCommand hastasil = new OracleCommand("DELETE FROM HASTA WHERE TCNO = :p1", con);
            hastasil.Parameters.Add(":p1", txtTC.Text);

            hastasil.ExecuteNonQuery();
            this.updateDataGrid();
            con.Close();
            MessageBox.Show("HASTA SİLİNDİ");

            //String Sql = "DELETE FROM HASTA WHERE TCNO = :TCNO";
            //    this.AUD(Sql, 2);

        }

        //TABLODAKİ VERİLERİ TEXTBOXLARA AKTARIR
        private void myDataGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

            txtTC.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtAD.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtSOYAD.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            cinsiyet = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            this.dateTimePicker1.Text= dataGridView1.CurrentRow.Cells[5].Value.ToString();
            txtTELNO.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            kangrubu = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            txtBOY.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            txtKILO.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();

           //.PerformClick();

            if (kangrubu == "A+") { radioButton3.Checked = true; }
            else if (kangrubu == "A-") { radioButton4.Checked = true; }
            else if (kangrubu == "B+") { radioButton5.Checked = true; }
            else if (kangrubu == "B-") { radioButton6.Checked = true; }
            else if (kangrubu == "AB+") { radioButton7.Checked = true; }
            else if (kangrubu == "AB-") { radioButton8.Checked = true; }

            if (cinsiyet == "E") { checkBox1.Checked = true; checkBox2.Checked = false; }
            else if (cinsiyet == "K") { checkBox2.Checked = true; checkBox1.Checked = false; }
        }

        //AYARLARI RESETLER
        private void btnReset_Click(object sender, EventArgs e) { temizle(); }
    }


 }



