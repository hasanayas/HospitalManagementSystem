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
    public partial class SekreterGiris : Form
    {
        public string tc;

        public SekreterGiris()
        {
            //full screen
            WindowState = FormWindowState.Maximized;

            InitializeComponent();
        }

     

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            HastaKayıt sayfa = new HastaKayıt();
            sayfa.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
              MuayeneKayit sayfa = new MuayeneKayit();
            sayfa.tc2 = tc;
            sayfa.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MuayeneIslemleri sayfa = new MuayeneIslemleri();
            sayfa.ShowDialog();
        }
    }
    }

