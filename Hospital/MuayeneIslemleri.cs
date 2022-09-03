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

namespace Hospital
{
    public partial class MuayeneIslemleri : Form
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
            dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 9);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9);
        }

        void temizle() { txtMuayeneNo.Clear(); txtHastaNo.Clear(); txtHastaSikayet.Clear(); txtMuayeneSaati.Clear(); txtSekreterNo.Clear(); }

        private void updateDataGrid()
        {
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT MUAYENENO,HASTANO,SEKRETERNO,MUAYENETARIHI,MUAYENESAATI,HASTASIKAYETI FROM MUAYENE ORDER BY MUAYENENO";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();
        }

        public MuayeneIslemleri()
        {
            WindowState = FormWindowState.Maximized;

            this.setConnection();
            InitializeComponent();
        }

        private void MuayeneIslemleri_Load(object sender, EventArgs e)
        {
            tasarım();
            this.updateDataGrid();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
           
            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();
            OracleCommand cmd = new OracleCommand("UPDATE MUAYENE SET MUAYENETARIHI = :MUAYENETARIHI," +
                "MUAYENESAATI= :MUAYENESAATI, HASTASIKAYETI= :HASTASIKAYETI WHERE MUAYENENO = :MUAYENENO ", con);

            cmd.Parameters.Add("MUAYENETARIHI", OracleDbType.Date).Value = this.dateTimePicker1.Text;
            cmd.Parameters.Add("MUAYENESAATI", OracleDbType.Char, 5).Value = txtMuayeneSaati.Text;
            cmd.Parameters.Add("HASTASIKAYETI", OracleDbType.Varchar2, 500).Value = txtHastaSikayet.Text;
            cmd.Parameters.Add("MUAYENENO", OracleDbType.Int32).Value = Int32.Parse(txtMuayeneNo.Text);

            cmd.ExecuteNonQuery();
            this.updateDataGrid();
            con.Close();
            MessageBox.Show("MUAYENE GÜNCELLENDİ");


            // cmd.Parameters.Add(":p1", this.dateTimePicker1.Text);
            //cmd.Parameters.Add(":p2", txtMuayeneSaati.Text);
            // cmd.Parameters.Add(":p3", txtHastaSikayet.Text);
            // cmd.Parameters.Add(":p4", Int32.Parse(txtMuayeneNo.Text));
        }


        private void btnSil_Click(object sender, EventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();
            OracleCommand hastasil = new OracleCommand("DELETE FROM MUAYENE WHERE MUAYENENO = :p1", con);
            hastasil.Parameters.Add(":p1", Int32.Parse(txtMuayeneNo.Text));

            hastasil.ExecuteNonQuery();
            this.updateDataGrid();
            con.Close();
            MessageBox.Show("MUAYENE SİLİNDİ");
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
          txtMuayeneNo.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
           txtHastaNo.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
          txtSekreterNo.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            this.dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
          txtMuayeneSaati.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
           txtHastaSikayet.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
       

        }

     
    }
}




//private void AUD(String sql_stmt, int state)
//{
//    String msg = "";
//    OracleCommand cmd = con.CreateCommand();
//    cmd.CommandText = sql_stmt;
//    cmd.CommandType = CommandType.Text;

//    //RADİO BUTON VE CHECKBOXKOŞULLARI


//    switch (state)
//    {
//        case 0:
//            MessageBox.Show("saas");
//            break;

//        case 1:
//            cmd.Parameters.Add(":p1", Int32.Parse(txtMuayeneNo.Text));
//            cmd.Parameters.Add("MUAYENETARIHI", OracleDbType.Date).Value = this.dateTimePicker1.Text;
//            cmd.Parameters.Add("MUAYENESAATI", OracleDbType.Char, 5).Value = txtMuayeneSaati.Text;
//            cmd.Parameters.Add("HASTASIKAYETI", OracleDbType.Char, 500).Value = txtHastaSikayet.Text;

//            msg = "Muayene Başarıyla Güncellendi";
//            break;
//    }

//    try
//    {
//        int n = cmd.ExecuteNonQuery();
//        if (n > 0)
//        {
//            MessageBox.Show(msg);
//            this.updateDataGrid();
//        }
//    }
//    catch (Exception exp ) { }
//}