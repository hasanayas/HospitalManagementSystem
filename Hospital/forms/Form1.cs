﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

namespace Hospital
{
    public partial class Form1 : Form
    {
  
        public Form1()
        {
           //full screen
            WindowState = FormWindowState.Maximized;

            InitializeComponent();
            button1.TabStop = false;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
        }

        public void textBoxTemizle()
        {
           textBox1.Clear();
           textBox2.Clear();
        }

      

        private void button1_Click(object sender, EventArgs e)
        {

            if(String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text))

            {
                MessageBox.Show("Alanları Doldurunuz");
            }

            

            //BAĞLANTI 

            OracleConnection con = new OracleConnection();
            con.ConnectionString = "Data Source=(DESCRIPTION =" +
        "(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))" +
        "(CONNECT_DATA =" +
         "(SERVER = DEDICATED)" +
         "(SERVICE_NAME = oracle.hasan.com)" +
           ")" +
         ");User Id = HASAN;password=AYAS";


            con.Open();

         

            //SEKRETER GİRİŞ KISMI

            string sorgu = "SELECT SEKRETERNO FROM SEKRETER where TCNO ='"+textBox1.Text.Trim()+"'and SİFRE  = '"+textBox2.Text.Trim()+"'";
            
            OracleDataAdapter table1 = new OracleDataAdapter(sorgu, con);
            DataSet dset = new DataSet();
            table1.Fill(dset);

            if(dset.Tables[0].Rows.Count > 0)
            {
             

   
                SekreterGiris sayfa = new SekreterGiris();
                sayfa.tc = textBox1.Text;
                sayfa.ShowDialog();
                textBoxTemizle();
            }


            // BOLUM EKLENECEK , İLGİLİ DOKTOR BÖLÜM SAYFASI AÇILDI
            //DATAGRIDVİEW => where Muayne Tarihi == BUGÜN ve MUAYENE SAATİ (Artan Sıralamada Gözüksün)


            //DAHİLİYE DOKTORU GİRİŞ KISMI 

           string sorgu2 = "SELECT * FROM DOKTOR where TCNO ='" + textBox1.Text.Trim() + "'  and SİFRE  = '" + textBox2.Text.Trim() + "' and BOLUMU = 'DAHİLİYE'";
            OracleDataAdapter table2 = new OracleDataAdapter(sorgu2, con);
            DataSet dset2 = new DataSet();
           table2.Fill(dset2);

            if (dset2.Tables[0].Rows.Count > 0)
            {
                textBoxTemizle();
                DAHILIYE sayfa = new DAHILIYE();
                sayfa.ShowDialog();
            }


            //GÖZ DOKTORU GİRİŞ KISMI 

            string sorgu3 = "SELECT * FROM DOKTOR where TCNO ='" + textBox1.Text.Trim() + "'  and SİFRE  = '" + textBox2.Text.Trim() + "' and BOLUMU = 'GÖZ'";
            OracleDataAdapter table3 = new OracleDataAdapter(sorgu3, con);
            DataSet dset3 = new DataSet();
            table3.Fill(dset3);

            if (dset3.Tables[0].Rows.Count > 0)
            {
                textBoxTemizle();
                GOZ sayfa = new GOZ();
                sayfa.ShowDialog();
            }

            //CİLDİYE DOKTORU GİRİŞ KISMI 

            string sorgu4 = "SELECT * FROM DOKTOR where TCNO ='" + textBox1.Text.Trim() + "'  and SİFRE  = '" + textBox2.Text.Trim() + "' and BOLUMU = 'CİLDİYE'";
            OracleDataAdapter table4 = new OracleDataAdapter(sorgu4, con);
            DataSet dset4 = new DataSet();
            table4.Fill(dset4);

            if (dset4.Tables[0].Rows.Count > 0)
            {
                textBoxTemizle();
                CILDIYE sayfa = new CILDIYE();
                sayfa.ShowDialog();
            }










            //if (dtbl.Tables[0].Rows.Count > 0)
            //{
            //    OracleCommand command = new OracleCommand(sorgu2, con);



            //    MessageBox.Show("DOKTOR Girişi");
            //    command.ExecuteNonQuery();

            //}






        }

        private void button2_Click(object sender, EventArgs e)
        {
            SekreterGiris sayfa = new SekreterGiris();
            sayfa.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminForm sayfa = new AdminForm();
            sayfa.ShowDialog();
        }
    }
}
