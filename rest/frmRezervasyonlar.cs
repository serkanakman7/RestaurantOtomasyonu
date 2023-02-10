using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    public partial class frmRezervasyonlar : Form
    {
        public frmRezervasyonlar()
        {
            InitializeComponent();
        }

        private void frmRezervasyonlar_Load(object sender, EventArgs e)
        {
            cMusteriler m = new cMusteriler();
            m.MusteriGetir(lvMusteriler);

            cMasalar masa = new cMasalar();
            masa.MasaKapasitesiveDurumuGetir(cbMasa);

            dtTarih.MinDate = DateTime.Today;
            dtTarih.Format = DateTimePickerFormat.Time;
        }

        private void txtMusteriAd_TextChanged(object sender, EventArgs e)
        {
            cMusteriler c = new cMusteriler();
            c.MusteriGetirAd(lvMusteriler, txtMusteriAd.Text);
        }

        private void txtTelefon_TextChanged(object sender, EventArgs e)
        {
            cMusteriler c = new cMusteriler();
            c.MusteriGetirTlf(lvMusteriler, txtTelefon.Text);
        }

        private void txtAdres_TextChanged(object sender, EventArgs e)
        {
            cMusteriler c = new cMusteriler();
            c.MusteriGetirAd(lvMusteriler, txtAdres.Text);                               //
        }

        void Temizle()
        {
            txtAdres.Clear();
            txtKisiSayisi.Clear();
            txtMasa.Clear();
            txtTarih.Clear();
        }

        private void btnRezervasyonAc_Click(object sender, EventArgs e)
        {
            cRezervasyon r = new cRezervasyon();

            if (lvMusteriler.SelectedItems.Count > 0)
            {
                bool result = r.RezervasyonAcikMiKontrol(Convert.ToInt32(lvMusteriler.SelectedItems[0].SubItems[0].Text));

                if (!result)
                {
                    if (txtTarih.Text != "")
                    {
                        if (txtKisiSayisi.Text != "")
                        {
                            cMasalar masa = new cMasalar();
                            if (masa.TableGetByState(Convert.ToInt32(txtMasaNo.Text), 1))
                            {
                                cAdisyon a = new cAdisyon();
                                a.Tarih = Convert.ToDateTime(txtTarih.Text);
                                a.ServisTurNo = 1;
                                a.MasaId = Convert.ToInt32(txtMasaNo.Text);
                                a.PersonelId = cGenel._PersonelId;

                                r.ClientId = Convert.ToInt32(lvMusteriler.SelectedItems[0].SubItems[0].Text);
                                r.TableId = Convert.ToInt32(txtMasaNo.Text);
                                r.Date = Convert.ToDateTime(txtTarih.Text);
                                r.ClientCount = Convert.ToInt32(txtKisiSayisi.Text);
                                r.Description = txtAciklama.Text;

                                r.AdditionId = a.RezervasyonAdisyonAc(a);
                                result = r.RezervasyonAc(r);

                                if (result)
                                {
                                    MessageBox.Show("Rezervasyon Başarıyla Açılmıştır.");
                                    masa.SetChangeTableState(txtMasaNo.Text, 3);
                                }
                                else
                                {
                                    MessageBox.Show("Rezervasyon Kayıtı Gerçekleşmemiştir.Yetkili Birine Bildiriniz!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Rezervasyon Yapılan Masa Şuanda Dolu");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Lütfen Kisi Sayisini Seçiniz");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lütfen Bir Tarih Giriniz");
                    }
                }
                else
                {
                    MessageBox.Show("Bu Müşteri Üzerinde Açık Bir Rezervasyon Bulunmaktadır.");
                }
            }
        }

        private void dtTarih_MouseEnter(object sender, EventArgs e)
        {
            dtTarih.Width = 200;
        }

        private void dtTarih_Enter(object sender, EventArgs e)
        {
            dtTarih.Width = 200;
        }

        private void dtTarih_MouseLeave(object sender, EventArgs e)
        {
            dtTarih.Width = 23;
        }

        private void dtTarih_ValueChanged(object sender, EventArgs e)
        {
            txtTarih.Text = dtTarih.Value.ToString();
        }

        private void cbMasa_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbKisiSayisi.Enabled = true;
            txtMasa.Text = cbMasa.SelectedItem.ToString();

            cMasalar Kapasitesi = (cMasalar)cbMasa.SelectedItem;
            int kapasite = Kapasitesi.Kapasite;
            txtMasaNo.Text = Kapasitesi.Id.ToString();

            cbKisiSayisi.Items.Clear();
            for(int i = 0; i < kapasite; i++)
            {
                cbKisiSayisi.Items.Add(i + 1);
            }
        }

        private void cbKisiSayisi_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtKisiSayisi.Text = cbKisiSayisi.SelectedItem.ToString();
        }

        private void cbMasa_MouseEnter(object sender, EventArgs e)
        {
            cbMasa.Width = 305;
        }

        private void cbMasa_MouseLeave(object sender, EventArgs e)
        {
            cbMasa.Width = 23;
        }

        private void cbKisiSayisi_MouseEnter(object sender, EventArgs e)
        {
            cbKisiSayisi.Width = 100;
        }

        private void cbKisiSayisi_MouseLeave(object sender, EventArgs e)
        {
            cbKisiSayisi.Width = 23;
        }

        private void bnSiparisKontrol_Click(object sender, EventArgs e)
        {
            frmSiparisKontrol frm = new frmSiparisKontrol();
            this.Close();
            frm.Show();
        }

        private void btnYeniMusteri_Click(object sender, EventArgs e)
        {
            frmMusteriEkleme frm = new frmMusteriEkleme();
            cGenel._MusteriEkleme = 0;
            btnMusteriGuncelle.Visible = false;
            btnYeniMusteri.Visible = true;
            this.Close();
            frm.Show();
        }

        private void btnMusteriGuncelle_Click(object sender, EventArgs e)
        {
            frmMusteriEkleme frm = new frmMusteriEkleme();
            cGenel._MusteriEkleme = 0;
            cGenel._MusteriId = Convert.ToInt32(lvMusteriler.SelectedItems[0].SubItems[0].Text);
            btnMusteriGuncelle.Visible = true;
            btnYeniMusteri.Visible = false;
            this.Close();
            frm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmMenu frm = new frmMenu();
            this.Close();
            frm.Show();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Çıkmak İstediğinizden Emin Misiniz ?", "Uyarı!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
