using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    public partial class frmMusteriAra : Form
    {
        public frmMusteriAra()
        {
            InitializeComponent();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Çıkmak İstediğinize Emin Misiniz?", "Uyarı!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes){
                Application.Exit();
            }
        }

        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            frmMenu frm = new frmMenu();
            this.Close();
            frm.Show();
        }

        private void btnYeniMusteri_Click(object sender, EventArgs e)
        {
            frmMusteriEkleme frm = new frmMusteriEkleme();
            cGenel._MusteriEkleme = 1;
            frm.btnGuncelle.Visible = false;
            frm.btnEkle.Visible = true;
            frm.Show();
        }

        private void frmMusteriAra_Load(object sender, EventArgs e)
        {
            cMusteriler c = new cMusteriler();
            c.MusteriGetir(lvMusteriler);
        }

        private void btnMusteriGuncelle_Click(object sender, EventArgs e)
        {
            if (lvMusteriler.SelectedItems.Count > 0)
            {
                frmMusteriEkleme frm = new frmMusteriEkleme();

                cGenel._MusteriEkleme = 1;
                cGenel._MusteriId = Convert.ToInt32(lvMusteriler.SelectedItems[0].SubItems[0].Text);
                frm.btnEkle.Visible = false;
                frm.btnGuncelle.Visible = true;

                this.Close();
                frm.Show();
            }
        }

        private void txtAd_TextChanged(object sender, EventArgs e)
        {
            cMusteriler c = new cMusteriler();
            c.MusteriGetirAd(lvMusteriler, txtAd.Text);
        }

        private void btnAdisyonBul_Click(object sender, EventArgs e)
        {
            if (txtAdisyonId.Text != "")
            {
                cGenel._AdisyonId = txtAdisyonId.Text;

                cPaketler c = new cPaketler();
                bool result = c.GetCheckOpenAdditionId(Convert.ToInt32(txtAdisyonId.Text));

                if (result)
                {
                    frmBill frm = new frmBill();
                    cGenel._ServisTurNo = 2;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show(txtAdisyonId.Text + "Nolu Adisyon Bulunamadı.");
                }

            }
            else
            {
                MessageBox.Show("Aramak İstediğiniz Adisyonu Yazınız");
            }
        }

        private void txtSoyad_TextChanged(object sender, EventArgs e)
        {
            cMusteriler c = new cMusteriler();
            c.MusteriGetirSoyad(lvMusteriler, txtSoyad.Text);
        }

        private void txtTelefon_TextChanged(object sender, EventArgs e)
        {
            cMusteriler c = new cMusteriler();
            c.MusteriGetirTlf(lvMusteriler, txtTelefon.Text);
        }

        private void btnSiparisler_Click(object sender, EventArgs e)
        {
            frmSiparisKontrol frm = new frmSiparisKontrol();
            this.Hide();
            frm.Show();
        }
    }
}
