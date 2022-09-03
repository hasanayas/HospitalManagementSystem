using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            WindowState = FormWindowState.Maximized; // Full Screen
            InitializeComponent();
        }

        //Formları Yükleyen Fonksiyon
       public void formYukle(object Form)
        {
            if (this.mainpanel.Controls.Count > 0)
                this.mainpanel.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.mainpanel.Controls.Add(f);
            this.mainpanel.Tag = f;
            f.Show();
        }
       
        
private void btnHasta_Click(object sender, EventArgs e)
        {
            formYukle(new FormAdminHasta());
        }

        private void btnSekreter_Click(object sender, EventArgs e)
        {
            formYukle(new FormAdminSekreter());
        }

        private void btnPoliklinik_Click(object sender, EventArgs e)
        {
            formYukle(new FormAdminDoktorvePoliklinik ());
        }

       
    }
}
