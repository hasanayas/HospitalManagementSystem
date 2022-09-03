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
    public partial class FormAdminHasta : Form
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

        void temizle() { txtTC.Clear(); txtAD.Clear(); txtSOYAD.Clear(); txtTELNO.Clear(); txtBOY.Clear(); txtKILO.Clear(); }

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
            chart1.ChartAreas["ChartArea1"].BackColor = Color.Transparent;
          
            //dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 9);
            //dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9);
        }


        //VERİTABANINDAKİ DATALARI C# TABLOSUNA AKTARIR
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


        //TABLODAKİ VERİLERİ TEXTBOXLARA AKTARIR
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

            txtTC.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtAD.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtSOYAD.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            cinsiyet = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            this.dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
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

        //YENİ HASTA EKLER
        private void btnEkle_Click_1(object sender, EventArgs e)
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
            OracleCommand hastaEkle = new OracleCommand("sp_hasta_ekle", con);
           hastaEkle.CommandType = CommandType.StoredProcedure;

           hastaEkle.Parameters.Add("tc_no", txtTC.Text);
           hastaEkle.Parameters.Add("hasta_adi", txtAD.Text);
           hastaEkle.Parameters.Add("hasta_soyadi", txtSOYAD.Text);
           hastaEkle.Parameters.Add("hasta_cinsiyet", cinsiyet);
           hastaEkle.Parameters.Add("hasta_dt", this.dateTimePicker1.Text);
           hastaEkle.Parameters.Add("hasta_tel", txtTELNO.Text);
           hastaEkle.Parameters.Add("hasta_kangrubu", kangrubu);
           hastaEkle.Parameters.Add("hasta_boy", txtBOY.Text);
           hastaEkle.Parameters.Add("hasta_kilo", txtKILO.Text);

           hastaEkle.ExecuteNonQuery();
            this.updateDataGrid();
            temizle();
            con.Close();
            MessageBox.Show("YENİ HASTA KAYDEDİLDİ");

        }

        //HASTA SİLER

        private void btnSil_Click_1(object sender, EventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();

            OracleCommand hastaSil = new OracleCommand("sp_hasta_sil", con);
            hastaSil.CommandType = CommandType.StoredProcedure;
            hastaSil.Parameters.Add("hasta_tc", txtTC.Text);
            hastaSil.ExecuteNonQuery();
            this.updateDataGrid();
            temizle();
            con.Close();
            MessageBox.Show("HASTA SİLİNDİ");
        }


        //HASTA GÜNCELLEME

        private void btnGuncelle_Click(object sender, EventArgs e)
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

            OracleCommand hastaGuncelle = new OracleCommand("sp_hasta_guncelleme", con);
            hastaGuncelle.CommandType = CommandType.StoredProcedure;

            hastaGuncelle.Parameters.Add("hasta_tc", txtTC.Text);
            hastaGuncelle.Parameters.Add("hasta_ad", txtAD.Text);
            hastaGuncelle.Parameters.Add("hasta_soyad", txtSOYAD.Text);
            hastaGuncelle.Parameters.Add("hasta_cinsiyet",cinsiyet );
            hastaGuncelle.Parameters.Add("hasta_dt", this.dateTimePicker1.Text);
            hastaGuncelle.Parameters.Add("hasta_tel", txtTELNO.Text);
            hastaGuncelle.Parameters.Add("hasta_kangrubu",kangrubu );
            hastaGuncelle.Parameters.Add("hasta_boy", txtBOY.Text);
            hastaGuncelle.Parameters.Add("hasta_kilo", txtKILO.Text);

            hastaGuncelle.ExecuteNonQuery();
            this.updateDataGrid();
            temizle();
            con.Close();
            MessageBox.Show("DOKTOR GÜNCELLENDİ");
        }

        //PDF AKTARMA
        private void button1_Click_1(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PDF files|*.pdf" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Document doc = new Document(iTextSharp.text.PageSize.A4, 50, 50, 50, 50);
                        PdfWriter pdfWriter = PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                        doc.Open();
                        PdfContentByte pdfContent = pdfWriter.DirectContent;
                        iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(doc.PageSize);
                        //customized border sizes
                        rectangle.Left += doc.LeftMargin - 5;
                        rectangle.Right -= doc.RightMargin - 5;
                        rectangle.Top -= doc.TopMargin - 5;
                        rectangle.Bottom += doc.BottomMargin - 5;
                        pdfContent.SetColorStroke(BaseColor.WHITE);//setting the color of the border to white
                        pdfContent.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
                        pdfContent.Stroke();
                        //setting font type, font size and font color
                        Paragraph p = new Paragraph();
                        p.Alignment = Element.ALIGN_CENTER;//adjust the alignment of the heading
                        doc.Add(p);//adding component to the document
                        iTextSharp.text.Font font = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);

                        //creating pdf table
                        PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            PdfPCell cell = new PdfPCell(); //create object from the pdfpcell
                            cell.BackgroundColor = BaseColor.WHITE;//set color of cells
                            cell.AddElement(new Chunk(dataGridView1.Columns[j].HeaderText, font));
                            table.AddCell(cell);
                        }

                        //adding rows from gridview to table
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            table.WidthPercentage = 115;//set width of the table
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                if (dataGridView1[j, i].Value != null)
                                    table.AddCell(new Phrase(dataGridView1[j, i].Value.ToString()));
                            }
                        }
                        //adding table to document
                        doc.Add(table);
                        doc.Close();
                        MessageBox.Show("Başarıyla Kaydedildi", "Mesaj", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //AYARLARI RESETLER
        private void button2_Click(object sender, EventArgs e)
        {
            temizle();
        }

        public FormAdminHasta()
        {

            this.setConnection();
            InitializeComponent();

        }

        private void FormAdminHasta_Load(object sender, EventArgs e)
        {
            tasarım();
            this.updateDataGrid();
            chart1.Series["s1"].Points.AddXY("ERKEK", "98");
            chart1.Series["s1"].Points.AddXY("KADIN", "60");
        



        }

     
    }


}
