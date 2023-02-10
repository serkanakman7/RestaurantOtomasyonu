using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    public partial class frmSiparisKontrol : Form
    {
        public frmSiparisKontrol()
        {
            InitializeComponent();
        }

        
        protected void dinamikMethod(object sender , EventArgs e)
        {
            cAdisyon c = new cAdisyon();

            Button dinamikButon = (sender as Button);
            frmBill frm = new frmBill();
            cGenel._ServisTurNo = 2;
            cGenel._AdisyonId = c.MusterininSonAdisyonId(Convert.ToInt32(dinamikButon.Name)).ToString();
            frm.Show();
        }

        protected void dinamikMethod2(object sender, EventArgs e)
        {
            cAdisyon c = new cAdisyon();

            Button dinamikButon = (sender as Button);
            c.MusteriDetaylar(lvMusteriDetaylari, Convert.ToInt32(dinamikButon.Name));
            SonSiparisTarihi();
            lvSatisDetaylari.Items.Clear();
            //Toplam();
            cSiparis s = new cSiparis();
            cGenel._ServisTurNo = 2;
            cGenel._AdisyonId = c.MusterininSonAdisyonId(Convert.ToInt32(dinamikButon.Name)).ToString();
            lblGenelToplam.Text = s.GenelToplamBul(Convert.ToInt32(dinamikButon.Name)).ToString()+ "TL";
        }

        void SonSiparisTarihi()
        {
            if (lvMusteriDetaylari.Items.Count > 0)
            {
                int s = lvMusteriDetaylari.Items.Count;
                lblSonSiparisTarihi.Text = lvMusteriDetaylari.Items[s-1].SubItems[3].Text;
                txtToplamTutar.Text = s + "Adet";
            }
        }

        void Toplam()
        {
            int kayitSayisi = lvSatisDetaylari.Items.Count;
            decimal toplam = 0;
            for(int i = 0; i < kayitSayisi; i++)
            {
                toplam += (Convert.ToDecimal(lvSatisDetaylari.Items[i].SubItems[2].Text) * Convert.ToDecimal(lvSatisDetaylari.Items[i].SubItems[3].Text));
            }

            lblToplamSiparis.Text = toplam.ToString() + "TL";


        }

        private void frmSiparisKontrol_Load(object sender, EventArgs e)
        {
            cAdisyon c = new cAdisyon();
            int butonSayisi = c.PaketAdisyonIdBulAdedi();
            c.AcikPaketAdisyonlar(lvMusteriler);

            int sol = 1;
            int alt = 50;
            int bol = Convert.ToInt32(Math.Ceiling(Math.Sqrt(butonSayisi)));

            for (int i = 1; i <= butonSayisi; i++)
            {
                Button btn = new Button();
                btn.AutoSize = false;
                btn.Size = new Size(179, 50);
                btn.FlatStyle = FlatStyle.Flat;
                btn.Name = lvMusteriler.Items[i - 1].SubItems[0].Text;
                btn.Text = lvMusteriler.Items[i - 1].SubItems[1].Text;
                btn.Font = new Font(btn.Font.FontFamily.Name, 18);
                btn.Location = new Point(sol, alt);
                this.Controls.Add(btn);

                sol += btn.Width + 5;
                if (i % 2 == 0)
                {
                    sol = 1;
                    alt += 50;
                }

                btn.Click += new EventHandler(dinamikMethod);
                btn.MouseEnter += new EventHandler(dinamikMethod2);
            }

        }

        private void lvMusteriDetaylari_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMusteriDetaylari.SelectedItems.Count > 0)
            {
                cSiparis c = new cSiparis();
                c.AdisyonPaketSiparisDetaylari(lvSatisDetaylari, Convert.ToInt32(lvMusteriDetaylari.SelectedItems[0].SubItems[4].Text));
                Toplam();
                lblGenelToplam.Text = c.GenelToplamBul(Convert.ToInt32(lvMusteriDetaylari.SelectedItems[0].SubItems[0].Text)) + "TL";
            }
        }

        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            frmMenu frm = new frmMenu();
            this.Close();
            frm.Show();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Çıkmak İstediğinize Emin Misiniz?", "Uyarı!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
