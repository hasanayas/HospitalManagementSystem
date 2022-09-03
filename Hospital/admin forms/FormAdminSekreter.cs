
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
using System.Drawing.Drawing2D;

//EXPORT İÇİN AKTİF EDİLMELİDİR
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Imaging;

namespace Hospital
{
    public partial class FormAdminSekreter : Form
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

        void temizle() { txtTC.Clear(); txtAD.Clear(); txtSOYAD.Clear(); txtTELNO.Clear(); txtSifre.Clear();  }

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
            cmd.CommandText = "SELECT * FROM SEKRETER ORDER BY SEKRETERNO";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();
            
        }

        //TABLODAKİ VERİLERİ TEXTBOXLARA AKTARIR
        private void dataGridView1_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            txtTC.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtAD.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtSOYAD.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            this.dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            txtTELNO.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            txtSifre.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        }


        private void btnEkle_Click(object sender, EventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();
            OracleCommand Ekle = new OracleCommand("sp_sekreter_ekle", con);
            Ekle.CommandType = CommandType.StoredProcedure;

            Ekle.Parameters.Add("sekreter_tc", txtTC.Text);
            Ekle.Parameters.Add("sekreter_ad", txtAD.Text);
            Ekle.Parameters.Add("sekreter_soyad", txtSOYAD.Text);
            Ekle.Parameters.Add("sekreter_dt", this.dateTimePicker1.Text);
            Ekle.Parameters.Add("sekreter_tel", txtTELNO.Text);
            Ekle.Parameters.Add("sekreter_sifre", txtSifre.Text);

            Ekle.ExecuteNonQuery();
            this.updateDataGrid();
            temizle();
            con.Close();
            MessageBox.Show("YENİ HASTA KAYDEDİLDİ");
        }

        private void btnGuncelle_Click_1(object sender, EventArgs e)
        {

            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();

            OracleCommand hastaGuncelle = new OracleCommand("sp_sekreter_guncelleme", con);
            hastaGuncelle.CommandType = CommandType.StoredProcedure;

            hastaGuncelle.Parameters.Add("sekreter_tc", txtTC.Text);
            hastaGuncelle.Parameters.Add("sekreter_ad", txtAD.Text);
            hastaGuncelle.Parameters.Add("sekreter_soyad", txtSOYAD.Text);
            hastaGuncelle.Parameters.Add("sekreter_dt", this.dateTimePicker1.Text);
            hastaGuncelle.Parameters.Add("sekreter_tel", txtTELNO.Text);
            hastaGuncelle.Parameters.Add("sekreter_sifre", txtSifre.Text);

            hastaGuncelle.ExecuteNonQuery();
            this.updateDataGrid();
            temizle();
            con.Close();
            MessageBox.Show("DOKTOR GÜNCELLENDİ");
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();

            OracleCommand Sil = new OracleCommand("sp_sekreter_sil", con);
            Sil.CommandType = CommandType.StoredProcedure;
            Sil.Parameters.Add("sekreter_tc", txtTC.Text);
            Sil.ExecuteNonQuery();
            this.updateDataGrid();
            temizle();
            con.Close();
            MessageBox.Show("SEKRETER SİLİNDİ");
        }


        //AYARLARI RESETLER
        private void button2_Click(object sender, EventArgs e)
        {
            temizle();
        }

        public FormAdminSekreter()
        {

            this.setConnection();
            InitializeComponent();
        }

        private void FormAdminSekreter_Load_1(object sender, EventArgs e)
        {
            tasarım();
            this.updateDataGrid();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            temizle();
        }

        //EXPORT PDF
        private void button1_Click(object sender, EventArgs e)
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
    }


}
