using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
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

        private void frmSetting_Load(object sender, EventArgs e)
        {
            cPersoneller cp = new cPersoneller();
            cPersonelGorev cpg = new cPersonelGorev();

            string Gorev = cpg.PersonelGorevTanim(cGenel._PersonelGorevId);

            if (Gorev == "Mudur")
            {
                cp.personelGetByInformetion(cbPersonel);
                cpg.PersonelGorevGetir(cbGorevi);
                cp.PersonelBilgileriniGetir(lvPersoneller);

                btnYeni.Enabled = true;
                btnSil.Enabled = false;
                btnBilgiDegistir.Enabled = false;
                btnEkle.Enabled = false;
                groupBox1.Visible = true;
                groupBox2.Visible = true;
                groupBox3.Visible = false;
                groupBox4.Visible = true;
                txtSifre.ReadOnly = true;
                txtTekrarSifre.ReadOnly = true;
                lblBilgi.Text = "Mevki:Müdür / Yetki Sınırsız / Kullanıcı :" + cp.PersonelBilgiGetirIsim(cGenel._PersonelId);
            }
            else
            {
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                groupBox3.Visible = true;
                groupBox4.Visible = false;
                lblBilgi.Text = "Mevki : Müdür / Yetki Sınırlı / Kullanıcı : " + cp.PersonelBilgiGetirIsim(cGenel._PersonelId);
            }


        }

        private void btnDegistir_Click(object sender, EventArgs e)
        {
            if(txtYeniSifre.Text.Trim() != "" || txtYeniSifreTekrar.Text.Trim() != "")
            {
                if (txtYeniSifre.Text.Trim() == txtYeniSifreTekrar.Text.Trim())
                {
                    if (txtPersonelId.Text != "")
                    {
                        cPersoneller c = new cPersoneller();
                        bool result = c.PersonelSifreDegistir(cGenel._PersonelId, txtYeniSifre.Text);

                        if (result)
                        {
                            MessageBox.Show("Şifre Değiştirme İşlemi Başarıyla Gerçekleşmiştir.");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Personeli Seçiniz");
                    }
                }
                else
                {
                    MessageBox.Show("Şifreler Aynı Değil");
                }
            }
            else
            {
                MessageBox.Show("Şifre Alanını Boş Bırakmayınız");
            }
        }

        private void cbPersonel_SelectedIndexChanged(object sender, EventArgs e)
        {
            cPersoneller c = (cPersoneller)cbPersonel.SelectedItem;
            txtPersonelId.Text = c.PersonelId.ToString();
        }

        private void cbGorevi_SelectedIndexChanged(object sender, EventArgs e)
        {
            cPersonelGorev c = (cPersonelGorev)cbGorevi.SelectedItem;
            txtGorevId.Text = c.PersonelGorevId.ToString();

        }

        private void btnYeni_Click(object sender, EventArgs e)
        {
            btnYeni.Enabled = false;
            btnEkle.Enabled = true;
            btnBilgiDegistir.Enabled = true;
            btnSil.Enabled = false;
            txtSifre.ReadOnly = false;
            txtTekrarSifre.ReadOnly = false;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (lvPersoneller.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Silmek İstediğinize Emin Misiniz?","Uyarı!!!",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes) 
                { 

                cPersoneller c = new cPersoneller();
                bool result = c.PersonelSil(Convert.ToInt32(lvPersoneller.SelectedItems[0].Text));

                if (result)
                {
                    MessageBox.Show("Kayıt Silindi");
                    c.PersonelBilgileriniGetir(lvPersoneller);
                }
            }
            }
            else
            {
                MessageBox.Show("Lütfen Kayıt Seçiniz");
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if(txtAd.Text.Trim() !="" && txtSoyad.Text.Trim() !="" && txtSifre.Text.Trim() != "" && txtTekrarSifre.Text.Trim() !="" && txtGorevId.Text.Trim() != "")
            {
                if((txtSifre.Text.Trim()==txtTekrarSifre.Text.Trim()) && (txtSifre.Text.Length>5 || txtTekrarSifre.Text.Length > 5)) 
                {

                    cPersoneller c = new cPersoneller();
                    c.PersonelAd = txtAd.Text.Trim();
                    c.PersonelSoyad = txtSoyad.Text.Trim();
                    c.PersonelParola = txtSifre.Text.Trim();
                    c.PersonelGorevId = Convert.ToInt32(txtGorevId.Text.Trim());
                    bool result = c.PersonelEkle(c);
                    if (result)
                    {
                        MessageBox.Show("Kayıt Eklendi");
                        c.PersonelBilgileriniGetir(lvPersoneller);
                    }
                    else
                    {
                        MessageBox.Show("Kayıt Eklenirken Bir Hata Oluştu");
                    }
                }
                else
                {
                    MessageBox.Show("Şifreler Aynı Değil");
                }
            }
            else
            {
                MessageBox.Show("Boş Bırakılan Alanları Doldurunuz");
            }
        }

        private void btnBilgiDegistir_Click(object sender, EventArgs e)
        {
            if (lvPersoneller.SelectedItems.Count > 0)
            {

                if (txtAd.Text.Trim() != "" && txtSoyad.Text.Trim() != "" && txtSifre.Text.Trim() != "" && txtTekrarSifre.Text.Trim() != "" && txtGorevId.Text.Trim() != "")
                {
                    if ((txtSifre.Text.Trim() == txtTekrarSifre.Text.Trim()) && (txtSifre.Text.Length > 5 || txtTekrarSifre.Text.Length > 5))
                    {

                        cPersoneller c = new cPersoneller();
                        c.PersonelAd = txtAd.Text.Trim();
                        c.PersonelSoyad = txtSoyad.Text.Trim();
                        c.PersonelParola = txtSifre.Text.Trim();
                        c.PersonelGorevId = Convert.ToInt32(txtGorevId.Text.Trim());
                        bool result = c.PersonelGuncelle(c,Convert.ToInt32(txtPersonelNo));
                        if (result)
                        {
                            MessageBox.Show("Kayıt Güncellendi");
                            c.PersonelBilgileriniGetir(lvPersoneller);
                        }
                        else
                        {
                            MessageBox.Show("Kayıt Güncellenirken Bir Hata Oluştu");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Şifreler Aynı Değil");
                    }
                }
                else
                {
                    MessageBox.Show("Boş Bırakılan Alanları Doldurunuz");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox9.Text.Trim() != "" || textBox8.Text.Trim() != "")
            {
                if (textBox9.Text.Trim() == textBox8.Text.Trim())
                {
                    if (cGenel._PersonelId.ToString() != "")
                    {
                        cPersoneller c = new cPersoneller();
                        bool result = c.PersonelSifreDegistir(cGenel._PersonelId, textBox9.Text);

                        if (result)
                        {
                            MessageBox.Show("Şifre Değiştirme İşlemi Başarıyla Gerçekleşmiştir.");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Personeli Seçiniz");
                    }
                }
                else
                {
                    MessageBox.Show("Şifreler Aynı Değil");
                }
            }
            else
            {
                MessageBox.Show("Şifre Alanını Boş Bırakmayınız");
            }
        }

        private void lvPersoneller_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPersoneller.SelectedItems.Count > 0)
            {
                txtPersonelNo.Text = lvPersoneller.SelectedItems[0].SubItems[0].Text;
                cbGorevi.SelectedIndex = Convert.ToInt32(lvPersoneller.SelectedItems[0].SubItems[1].Text) - 1;
                txtAd.Text = lvPersoneller.SelectedItems[0].SubItems[3].Text;
                txtSoyad.Text = lvPersoneller.SelectedItems[0].SubItems[4].Text;

                btnSil.Enabled = true;
                btnBilgiDegistir.Enabled = true;
                
            }
            else
            {
                btnSil.Enabled = false;
                btnBilgiDegistir.Enabled = false;
            }

        }
    }
}
