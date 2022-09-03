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
    public partial class FormAdminDoktorvePoliklinik : Form
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

        private void updateDoktor()
        {
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM DOKTOR ORDER BY DOKTORNO";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView2.DataSource = dt.DefaultView;
            dr.Close();
        }

        private void updatePoliklinik() //POLİKLİNİK UPDATE
        {
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM POLIKLINIK ";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();
        }

        void temizleDoktor() {txtDoktorTc.Clear();  txtDoktorAd.Clear();txtDoktorSoyad.Clear();
            txtDoktorTel.Clear(); txtDoktorBolum.Clear();  txtDoktorSifre.Clear(); comboBox1.SelectedIndex = -1;
        }
        void temizle() { txtAD.Clear(); txtID.Clear(); }

        void tasarım()  //POLİKLİNİK GRİDVİEW TASARIMI
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            //dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.SeaGreen; // Çizgi Rengi
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView1.BackgroundColor = Color.FromArgb(30, 30, 30);
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//optional
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dataGridView2.BorderStyle = BorderStyle.None;
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            //dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.SeaGreen; // Çizgi Rengi
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView2.BackgroundColor = Color.FromArgb(30, 30, 30);
            dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;//optional
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            //dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 9);
            //dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9);

        }


        //POLİKLİNİK EKLEME
        private void btnEKLE_Click(object sender, EventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();

            OracleCommand polEkle = new OracleCommand("sp_polekle",con);
            polEkle.CommandType = CommandType.StoredProcedure;
            polEkle.Parameters.Add("pol_id", txtID.Text.Trim());
            polEkle.Parameters.Add("pol_ad", txtAD.Text.Trim());
           
            polEkle.ExecuteNonQuery();
            this.updatePoliklinik();
            temizle();
            con.Close();
            MessageBox.Show("YENİ POLİKLİNİK EKLENDİ");
        }

        //POLİKLİNİK SİLME
        private void btnSil_Click(object sender, EventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();

            OracleCommand polEkle = new OracleCommand("sp_polsil", con);
            polEkle.CommandType = CommandType.StoredProcedure;
            polEkle.Parameters.Add("pol_id", txtID.Text.Trim());
            polEkle.ExecuteNonQuery();
            this.updatePoliklinik();
            temizle();
            con.Close();
            MessageBox.Show(" POLİKLİNİK SİLİNDİ");
        }


        //DATAGRİDTEKİ VERİLERİ TEXTBOXLARA ATAR
        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtDoktorTc.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            txtDoktorAd.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            txtDoktorSoyad.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            this.dateTimePicker1.Text = dataGridView2.CurrentRow.Cells[4].Value.ToString();
            txtDoktorTel.Text = dataGridView2.CurrentRow.Cells[5].Value.ToString();
            txtDoktorBolum.Text = dataGridView2.CurrentRow.Cells[6].Value.ToString();
            comboBox1.SelectedItem = dataGridView2.CurrentRow.Cells[7].Value.ToString();
          txtDoktorSifre.Text = dataGridView2.CurrentRow.Cells[8].Value.ToString();
       
        }

        private void btnDoktorEkle_Click(object sender, EventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();
            OracleCommand doktorEkle = new OracleCommand("sp_doktor_ekle", con);
            doktorEkle.CommandType = CommandType.StoredProcedure;

            doktorEkle.Parameters.Add("doktor_tc", txtDoktorTc.Text);
            doktorEkle.Parameters.Add("doktor_ad", txtDoktorAd.Text);
            doktorEkle.Parameters.Add("doktor_soyad", txtDoktorSoyad.Text);
            doktorEkle.Parameters.Add(" doktor_dt", this.dateTimePicker1.Text);
            doktorEkle.Parameters.Add("doktor_tel", txtDoktorTel.Text);
            doktorEkle.Parameters.Add("doktor_bolum", txtDoktorBolum.Text);
            doktorEkle.Parameters.Add("doktor_polid", comboBox1.SelectedItem.ToString());
            doktorEkle.Parameters.Add("doktor_sifre", txtDoktorSifre.Text);
            doktorEkle.ExecuteNonQuery();
            this.updateDoktor();
            temizleDoktor();
            con.Close();
            MessageBox.Show("YENİ HASTA KAYDEDİLDİ");
        }

        private void btnDoktorGuncelle_Click(object sender, EventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();

            OracleCommand doktorGuncelle = new OracleCommand("sp_doktor_guncelleme", con);
            doktorGuncelle.CommandType = CommandType.StoredProcedure;

            doktorGuncelle.Parameters.Add("doktor_tc", txtDoktorTc.Text);
            doktorGuncelle.Parameters.Add("doktor_ad", txtDoktorAd.Text);
            doktorGuncelle.Parameters.Add("doktor_soyad", txtDoktorSoyad.Text);
            doktorGuncelle.Parameters.Add("doktor_dt", this.dateTimePicker1.Text);
            doktorGuncelle.Parameters.Add("doktor_tel", txtDoktorTel.Text);
            doktorGuncelle.Parameters.Add("doktor_bolum", txtDoktorBolum.Text);
            doktorGuncelle.Parameters.Add("doktor_polID", comboBox1.SelectedItem.ToString());
            doktorGuncelle.Parameters.Add("doktor_sifre", txtDoktorSifre.Text);

            doktorGuncelle.ExecuteNonQuery();
            this.updateDoktor();
            temizleDoktor();
            con.Close();
            MessageBox.Show("DOKTOR GÜNCELLENDİ");
        }

        private void btnDoktorSil_Click(object sender, EventArgs e)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);
            con.Open();

            OracleCommand doktorSil = new OracleCommand("sp_doktor_sil", con);
            doktorSil.CommandType = CommandType.StoredProcedure;
            doktorSil.Parameters.Add("doktor_tc", txtDoktorTc.Text);
            doktorSil.ExecuteNonQuery();
            this.updateDoktor();
            temizleDoktor();
            con.Close();
            MessageBox.Show("DOKTOR SİLİNDİ");
        }

        public FormAdminDoktorvePoliklinik()
        {
            
            this.setConnection();
            InitializeComponent();

            String connectionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
            con = new OracleConnection(connectionString);

            OracleCommand komut = new OracleCommand("SELECT * FROM POLIKLINIK", con);
            OracleDataReader dr;
            con.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr["POLIKLINIKID"]);
            }
        }

        private void FormAdminDoktorvePoliklinik_Load(object sender, EventArgs e)
        {
            tasarım();
            this.updateDoktor();
            this.updatePoliklinik();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            temizleDoktor();
        }

        private void btnPdf_Click(object sender, EventArgs e)
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
                        PdfPTable table = new PdfPTable(dataGridView2.Columns.Count);
                        for (int j = 0; j < dataGridView2.Columns.Count; j++)
                        {
                            PdfPCell cell = new PdfPCell(); //create object from the pdfpcell
                            cell.BackgroundColor = BaseColor.WHITE;//set color of cells
                            cell.AddElement(new Chunk(dataGridView2.Columns[j].HeaderText, font));
                            table.AddCell(cell);
                        }

                        //adding rows from gridview to table
                        for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        {
                            table.WidthPercentage = 115;//set width of the table
                            for (int j = 0; j < dataGridView2.Columns.Count; j++)
                            {
                                if (dataGridView2[j, i].Value != null)
                                    table.AddCell(new Phrase(dataGridView2[j, i].Value.ToString()));
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
