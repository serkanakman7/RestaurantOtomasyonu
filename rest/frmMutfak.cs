using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    public partial class frmMutfak : Form
    {
        public frmMutfak()
        {
            InitializeComponent();
        }

        private void frmMutfak_Load(object sender, EventArgs e)
        {
            cUrunCesitleri AnaKategori = new cUrunCesitleri();

            AnaKategori.UrunCesitleriniGetir(cbKategoriler);
            cbKategoriler.Items.Insert(0, "Tüm Kategoriler");
            cbKategoriler.SelectedIndex = 0;

            label6.Visible = false;
            txtArama.Visible = false;
            lvGidaListesi.Visible = false;

            cUrunler c = new cUrunler();
            c.UrunleriListele(lvGidaListesi);
        }

        private void Temizle()
        {
            txtGidaAdi.Clear();
            txtGidaFiyati.Clear();
            txtGidaFiyati.Text = string.Format("{0:##0.00}", 0);
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (rbAltKategori.Checked)
            {
                if(txtGidaAdi.Text.Trim()=="" || txtGidaFiyati.Text.Trim()=="" || cbKategoriler.SelectedItem=="Tüm Kategoriler")
                {
                    MessageBox.Show("Lütfen Boş Alanları Doldurunuz", "Dikkat Bilgiler Eksik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    cUrunler c = new cUrunler();
                    c.UrunAd = txtGidaAdi.Text;
                    c.Fiyat = Convert.ToDecimal(txtGidaFiyati.Text);
                    c.Aciklama = "Ürün Eklenmiştir";
                    c.UrunTurNo = UrunTurNo;
                    int result = c.UrunEkle(c);

                    if (result !=0)
                    {
                        MessageBox.Show("Ürün Eklendi");
                        Yenile();
                        Temizle();
                    }

                }
            }
            else
            {
                if (txtKategoriAd.Text.Trim() == "")
                {
                    MessageBox.Show("Lütfen Bir Kategori İsmi Giriniz", "Dikkat Bilgiler Eksik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    cUrunCesitleri gida = new cUrunCesitleri();
                    gida.Aciklama = txtAciklama.Text;
                    gida.KategoriAd = txtKategoriAd.Text;
                    int result = gida.UrunKategoriEkle(gida);

                    if(result != 0)
                    {
                        MessageBox.Show("Kategoriler Eklendi");
                        Yenile();
                        Temizle();
                    }
                }
            }
        }

        int UrunTurNo = 0;
        private void cbKategoriler_SelectedIndexChanged(object sender, EventArgs e)
        {
            cUrunler c = new cUrunler();

            if(cbKategoriler.SelectedItem.ToString()=="Tüm Kategoriler")
            {
                c.UrunleriListele(lvGidaListesi);
            }
            else
            {
                cUrunCesitleri cesit =(cUrunCesitleri)cbKategoriler.SelectedItem;
                UrunTurNo = cesit.UrunturNo;
                 c.UrunleriListeleByUrunId(lvGidaListesi, UrunTurNo);
                //c.UrunleriListele(lvGidaListesi);
            }
        }

        private void btnDegistir_Click(object sender, EventArgs e)
        {
            if (rbAltKategori.Checked)
            {
                if (txtGidaAdi.Text.Trim() == "" || txtGidaFiyati.Text.Trim() == "" || cbKategoriler.SelectedItem == "Tüm Kategoriler")
                {
                    MessageBox.Show("Lütfen Boş Alanları Doldurunuz", "Dikkat Bilgiler Eksik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    cUrunler c = new cUrunler();
                    c.UrunAd = txtGidaAdi.Text;
                    c.Fiyat = Convert.ToDecimal(txtGidaFiyati.Text);
                    c.UrunId = Convert.ToInt32(txtUrunId.Text);
                    c.Aciklama = "Ürün Güncellenmiştir";
                    c.UrunTurNo = UrunTurNo;
                    int result = c.UrunGuncelle(c);

                    if (result != 0)
                    {
                        MessageBox.Show("Ürün Güncellendi");
                        cbKategoriler_SelectedIndexChanged(sender, e);
                        Temizle();
                    }

                }
            }
            else
            {
                if (txtKategoriAd.Text.Trim() == "")
                {
                    MessageBox.Show("Lütfen Bir Kategori İsmi Giriniz", "Dikkat Bilgiler Eksik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    cUrunCesitleri gida = new cUrunCesitleri();
                    gida.Aciklama = txtAciklama.Text;
                    gida.KategoriAd = txtKategoriAd.Text;
                    gida.UrunturNo = Convert.ToInt32(txtKategoriId.Text);
                    int result = gida.UrunKategoriGuncelle(gida);

                    if (result != 0)
                    {
                        MessageBox.Show("Kategoriler Güncellendi");
                        gida.UrunCesitleriniGetir(lvKategoriler);
                        Temizle();
                    }
                }
            }
        }

        private void lvGidaListesi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvGidaListesi.SelectedItems.Count > 0)
            {
                txtGidaAdi.Text = lvGidaListesi.SelectedItems[0].SubItems[3].Text;
                txtGidaFiyati.Text = lvGidaListesi.SelectedItems[0].SubItems[4].Text;
                txtUrunId.Text = lvGidaListesi.SelectedItems[0].SubItems[0].Text;
                //cbKategoriler.SelectedIndex = Convert.ToInt32(txtUrunId.Text);
            }
        }

        private void lvKategoriler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvKategoriler.SelectedItems.Count > 0)
            {
                txtKategoriId.Text = lvKategoriler.SelectedItems[0].SubItems[0].Text;
                txtKategoriAd.Text = lvKategoriler.SelectedItems[0].SubItems[1].Text;
                txtAciklama.Text = lvKategoriler.SelectedItems[0].SubItems[2].Text;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (rbAltKategori.Checked)
            {
                if (lvGidaListesi.SelectedItems.Count > 0)
                {
                    if(MessageBox.Show("Ürün Silienecek , Emin Misiniz","Dikkat, Bilgiler Silinecek", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        cUrunler c = new cUrunler();
                        int result = c.UrunSil(Convert.ToInt32(txtUrunId.Text));

                        if (result != 0)
                        {
                            MessageBox.Show("Ürün Silinmiştir.");
                            cbKategoriler_SelectedIndexChanged(sender, e);
                            Yenile();
                            Temizle();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen Silinecek Ürünü Seçiniz.");
                }
            }
            else
            {
                if (lvKategoriler.SelectedItems.Count > 0)
                {
                    if(MessageBox.Show("Ürün Silienecek , Emin Misiniz", "Dikkat, Bilgiler Silinecek", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        cUrunCesitleri uc = new cUrunCesitleri();
                        int result = uc.UrunKategoriSil(Convert.ToInt32(txtKategoriId.Text));

                        if(result != 0)
                        {
                            MessageBox.Show("Ürün Silinmiştir.");
                            cbKategoriler_SelectedIndexChanged(sender, e);
                            Yenile();
                            Temizle();
                        }
                    }
                }
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Çıkmak İstediğinize Emin Misiniz?", "Uyarı!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            frmMenu frm = new frmMenu();
            this.Close();
            frm.Show();
        }

        private void btnBul_Click(object sender, EventArgs e)
        {
            label6.Visible = true;
            txtArama.Visible = true;
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            if (rbAltKategori.Checked)
            {
                cUrunler u = new cUrunler();
                u.UrunleriListeleByUrunAdi(lvGidaListesi, txtArama.Text);
            }
            else
            {
                cUrunCesitleri uc = new cUrunCesitleri();
                uc.UrunCesitleriniGetir(lvKategoriler, txtArama.Text);
            }
        }

        private void rbAltKategori_CheckedChanged(object sender, EventArgs e)
        {
            panelUrun.Visible = true;
            panelAnaKategori.Visible = false;
            lvKategoriler.Visible = false;
            lvGidaListesi.Visible = true;
        }

        private void rbAnaKategori_CheckedChanged(object sender, EventArgs e)
        {
            panelUrun.Visible = false;
            panelAnaKategori.Visible = true;
            lvKategoriler.Visible = true;
            lvGidaListesi.Visible = false;
            cUrunCesitleri uc = new cUrunCesitleri();
            uc.UrunCesitleriniGetir(lvKategoriler);
        }

        private void Yenile()
        {
            cUrunCesitleri uc = new cUrunCesitleri();
            uc.UrunCesitleriniGetir(cbKategoriler);
            uc.UrunCesitleriniGetir(lvKategoriler);
            cUrunler c = new cUrunler();
            c.UrunleriListele(lvGidaListesi);
        }
    }
}
