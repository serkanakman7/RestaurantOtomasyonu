using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    public partial class frmMusteriEkleme : Form
    {
        public frmMusteriEkleme()
        {
            InitializeComponent();
        }

        private void btnYeniMusteri_Click(object sender, EventArgs e)
        {
            if (txtTelefon.Text.Length > 7)
            {
                if(txtMusteriAd.Text=="" || txtMusteriSoyad.Text == "")
                {
                    MessageBox.Show("Lütfen Müşterinin Ad ve Soyad Kısmını Doldurunuz.");
                }
                else
                {
                    cMusteriler c = new cMusteriler();
                    bool result =c.MusteriVarMi(txtTelefon.Text);

                    if (!result)
                    {
                        c.MusteriAd = txtMusteriAd.Text;
                        c.MusteriSoyad = txtMusteriSoyad.Text;
                        c.Telefon = txtTelefon.Text;
                        c.Adres = txtAdres.Text;
                        c.Email = txtEmail.Text;
                        txtMusteriNo.Text=c.MusteriEkle(c).ToString();

                        if (txtMusteriNo.Text != "")
                        {
                            MessageBox.Show("Müşteri Eklendi");
                        }
                        else
                        {
                            MessageBox.Show("Müşteri Eklenmedi");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bu Kayıtta Biri Var Zaten!!!");
                    }

                }
            }
            else
            {
                MessageBox.Show("Lütfen en az 7 haneli bir telefon giriniz.");
            }
        }

        private void btnMusteriSec_Click(object sender, EventArgs e)
        {
            if (cGenel._MusteriEkleme == 0)
            {
                frmRezervasyonlar frm = new frmRezervasyonlar();
                cGenel._MusteriEkleme = 1;
                this.Close();
                frm.Show();
            }
            else if (cGenel._MusteriEkleme == 1)
            {
                frmPaketSiparis frm = new frmPaketSiparis();
                cGenel._MusteriEkleme = 0;
                this.Close();
                frm.Show();
            }
        }

        private void btnMusteriGuncelle_Click(object sender, EventArgs e)
        {
            if (txtTelefon.Text.Length > 7)
            {
                if (txtMusteriAd.Text == "" || txtMusteriSoyad.Text == "")
                {
                    MessageBox.Show("Lütfen Müşterinin Ad ve Soyad Kısmını Doldurunuz.");
                }
                else
                {
                    cMusteriler c = new cMusteriler();
                    c.MusteriAd = txtMusteriAd.Text;
                    c.MusteriSoyad = txtMusteriSoyad.Text;
                    c.Telefon = txtTelefon.Text;
                    c.Adres = txtAdres.Text;
                    c.Email = txtEmail.Text;
                    c.MusteriId = Convert.ToInt32(txtMusteriNo.Text);

                    bool result = c.MusteriBilgileriniGuncelle(c);


                    if (!result)
                    {

                        if (txtMusteriNo.Text != "")
                        {
                            MessageBox.Show("Müşteri Güncellendi");
                        }
                        else
                        {
                            MessageBox.Show("Müşteri Güncellenmedi");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bu Kayıtta Biri Var Zaten!!!");
                    }

                }
            }
            else
            {
                MessageBox.Show("Lütfen en az 7 haneli bir telefon giriniz.");
            }
        }

        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            frmMusteriAra frm = new frmMusteriAra();
            this.Close();
            frm.Show();
        }

        private void frmMusteriEkleme_Load(object sender, EventArgs e)
        {
            if (cGenel._MusteriId > 0)
            {
                cMusteriler c = new cMusteriler();
                txtMusteriNo.Text = cGenel._MusteriId.ToString();
                c.MusterileriGetirId(Convert.ToInt32(txtMusteriNo.Text), txtMusteriAd, txtMusteriSoyad, txtTelefon, txtAdres, txtEmail);
            }
        }
    }
}
