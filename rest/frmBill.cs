using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    public partial class frmBill : Form
    {
        public frmBill()
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
            if(MessageBox.Show("Çıkmak İstediğinize Emin Misiniz?", "Uyarı!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes){
                Application.Exit();
            }
        }

        cSiparis cs = new cSiparis(); int odemeTur = 0;
        private void frmBill_Load(object sender, EventArgs e)
        {
            gbIndirim.Visible = false; 
            if (cGenel._ServisTurNo == 1)
            {
                lblAdisyonId.Text = cGenel._AdisyonId;
                txtIndirimtutari.TextChanged += new EventHandler(txtIndirimtutari_TextChanged);
                cs.GetByOrder(lvUrunler, Convert.ToInt32(lblAdisyonId.Text));

                if (lvUrunler.Items.Count > 0)
                {
                    decimal toplam = 0;
                    for (int i = 0; i < lvUrunler.Items.Count; i++)
                    {
                        toplam += (Convert.ToDecimal(lvUrunler.Items[i].SubItems[1].Text) * Convert.ToDecimal(lvUrunler.Items[i].SubItems[3].Text));
                    }
                    lblToplamTutar.Text = string.Format("{0:0.000}", toplam);
                    lblOdenecek.Text = string.Format("{0:0.000}", toplam);
                    decimal kdv = Convert.ToDecimal(lblOdenecek.Text) * 18 / 100;
                    lblKdv.Text = string.Format("{0:0.000}", kdv);
                }
              // gbIndirim.Visible = true;
                txtIndirimtutari.Clear();
            }
            else if (cGenel._ServisTurNo == 2)
            {
                lblAdisyonId.Text = cGenel._AdisyonId;
                cPaketler pc = new cPaketler();
                odemeTur = pc.OdemeTurIdGetir(Convert.ToInt32(lblAdisyonId.Text));

                txtIndirimtutari.TextChanged += new EventHandler(txtIndirimtutari_TextChanged);
                cs.GetByOrder(lvUrunler, Convert.ToInt32(lblAdisyonId.Text));


                if (odemeTur == 1)
                {
                    rbNakit.Checked = true;
                }
                else if (odemeTur == 2)
                {
                    rbKrediKarti.Checked = true;
                }
                else if (odemeTur == 3)
                {
                    rbTicket.Checked = true;
                }


                if (lvUrunler.Items.Count > 0)
                {
                    decimal toplam = 0;
                    for (int i = 0; i < lvUrunler.Items.Count; i++)
                    {
                        toplam += (Convert.ToDecimal(lvUrunler.Items[i].SubItems[1].Text) * Convert.ToDecimal(lvUrunler.Items[i].SubItems[3].Text)); ;
                    }
                    lblToplamTutar.Text = string.Format("{0:0.000}", toplam);
                    lblOdenecek.Text = string.Format("{0:0.000}", toplam);
                    decimal kdv = Convert.ToDecimal(lblOdenecek.Text)*18/100;
                    lblKdv.Text = string.Format("{0:0.000}", kdv);
                }
                gbIndirim.Visible = false;
                txtIndirimtutari.Clear();
            }
        }

        private void txtIndirimtutari_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDecimal(lblIndirim.Text) < Convert.ToDecimal(lblToplamTutar.Text))
                {
                    try
                    {
                        lblIndirim.Text = string.Format("{0:0.000}", Convert.ToDecimal(txtIndirimtutari.Text));
                    }
                    catch (Exception)
                    {
                        lblIndirim.Text = string.Format("{0:0.000}", 0);
                    }
                }
                else
                {
                    MessageBox.Show("İndirim Tutarı Toplamı Tutardan Fazla Olamaz!!!");
                }
            }
            catch (Exception)
            {
                lblIndirim.Text = string.Format("{0:0.000}", 0);
            }
        }

        private void chkIndirim_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIndirim.Checked)
            {
                gbIndirim.Visible = true;
                txtIndirimtutari.Clear();
            }
            else
            {
                gbIndirim.Visible = false;
                txtIndirimtutari.Clear();
            }
        }

        private void lblIndirim_TextChanged(object sender, EventArgs e)
        {
            lblIndirim.Text = txtIndirimtutari.Text;
            if (Convert.ToDecimal(lblIndirim.Text) > 0)
            {
                decimal odenecek = 0;
                lblOdenecek.Text = lblToplamTutar.Text;
                odenecek = Convert.ToDecimal(lblOdenecek.Text) - Convert.ToDecimal(lblIndirim.Text);

                lblOdenecek.Text = string.Format("{0:0.000}", odenecek);
            }
            decimal kdv = Convert.ToDecimal(lblOdenecek.Text) * 18 / 100;
            lblKdv.Text = string.Format("{0:0.000}", kdv);
        }

                    cMasalar masalar = new cMasalar();
            cRezervasyon rezerve = new cRezervasyon();
        private void btnHesabiKapat_Click(object sender, EventArgs e)
        {

            if(cGenel._ServisTurNo == 1)
            {
                int masaId = masalar.TableGetByNumber(cGenel._ButtonName);
                int musteriId = 0;

                if (masalar.TableGetByState(masaId, 4))
                {
                    musteriId = rezerve.GetByClientIdFromRezervasyon(masaId);
                }
                else
                {
                    musteriId = 1;
                }

                int odemeTurId = 0;

                if (rbNakit.Checked)
                {
                    odemeTurId = 1;
                }
                else if (rbKrediKarti.Checked)
                {
                    odemeTurId = 2;
                }
                else if (rbTicket.Checked)
                {
                    odemeTurId = 3;
                }

                cOdeme odeme = new cOdeme();

                odeme.AdisyonId = Convert.ToInt32(lblAdisyonId.Text);
                odeme.OdemeTurId = odemeTurId;
                odeme.MusteriId = musteriId;
                odeme.AraToplam = Convert.ToDecimal(lblOdenecek.Text);
                odeme.KDVTutari = Convert.ToDecimal(lblKdv.Text);
                odeme.GenelToplam = Convert.ToDecimal(lblToplamTutar.Text);
                odeme.Indirim = Convert.ToDecimal(lblIndirim.Text);

                bool result = odeme.BillClose(odeme);

                if (result)
                {
                    MessageBox.Show("Hesap Kapatılmıştır");
                    masalar.SetChangeTableState(cGenel._ButtonName, 1);

                    cRezervasyon c = new cRezervasyon();
                    c.RezervationClose(Convert.ToInt32(lblAdisyonId.Text));

                    cAdisyon a = new cAdisyon();
                    a.AdditionClose(Convert.ToInt32(lblAdisyonId.Text), 0);

                    this.Close();
                    frmMasalar frm = new frmMasalar();
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Hesap Kapatılırken Beklenmedik Bir Hata Oluştu.Lütfen yetkililere Bildiriniz");
                }


            }
            else if (cGenel._ServisTurNo == 2)
            {
                cOdeme odeme = new cOdeme();

                odeme.AdisyonId = Convert.ToInt32(lblAdisyonId.Text);
                odeme.OdemeTurId = odemeTur;
                odeme.MusteriId = 1;
                odeme.AraToplam = Convert.ToDecimal(lblOdenecek.Text);
                odeme.KDVTutari = Convert.ToDecimal(lblKdv.Text);
                odeme.GenelToplam = Convert.ToDecimal(lblToplamTutar.Text);
                odeme.Indirim = Convert.ToDecimal(lblIndirim.Text);

                bool result = odeme.BillClose(odeme);

                if (!result)
                {
                    MessageBox.Show("Hesap Kapatılmıştır");

                    cAdisyon a = new cAdisyon();
                    a.AdditionClose(Convert.ToInt32(lblAdisyonId.Text), 1);

                    cPaketler p = new cPaketler();
                    p.OrderServiceClose(Convert.ToInt32(lblAdisyonId.Text));

                    this.Close();
                    frmMasalar frm = new frmMasalar();
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Hesap Kapatılırken Beklenmedik Bir Hata Oluştu.Lütfen yetkililere Bildiriniz");
                }
            }
        }

        private void btnHesapOdeme_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        Font Baslik = new Font("Verdana", 15, FontStyle.Bold);
        Font AltBaslik = new Font("Verdana", 12, FontStyle.Regular);
        Font Icerik = new Font("Verdana", 10);
        SolidBrush sb = new SolidBrush(Color.Black);

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            StringFormat st = new StringFormat();
            st.Alignment = StringAlignment.Near;
            e.Graphics.DrawString("Mevlana RESTUARANT", Baslik, sb, 270, 100, st);

            e.Graphics.DrawString("---------------------------------------", AltBaslik, sb, 300, 120, st);
            e.Graphics.DrawString("Ürün Adı          Adet                Fiyat", AltBaslik, sb, 150, 250, st);
            e.Graphics.DrawString("-------------------------------------------------", AltBaslik, sb, 150, 280, st);

            for(int i = 0; i < lvUrunler.Items.Count; i++)
            {
                e.Graphics.DrawString(lvUrunler.Items[i].SubItems[0].Text,Icerik, sb, 150, 300+i*30, st);
                e.Graphics.DrawString(lvUrunler.Items[i].SubItems[1].Text, Icerik, sb, 300, 300 + i * 30, st);
                e.Graphics.DrawString(lvUrunler.Items[i].SubItems[3].Text, Icerik, sb, 450, 300 + i * 30, st);
            }
            e.Graphics.DrawString("-------------------------------------------------", AltBaslik, sb, 150, 300 + 30 * lvUrunler.Items.Count, st);
            e.Graphics.DrawString("İndirim Tutarı:------------"+lblIndirim.Text+"TL", AltBaslik, sb, 150, 300 + 30 * (lvUrunler.Items.Count+1), st);
            e.Graphics.DrawString("KDV Tutarı:---------------"+lblKdv.Text+"TL", AltBaslik, sb, 150, 300 + 30 * (lvUrunler.Items.Count+2), st);
            e.Graphics.DrawString("ToplamTutar:-------------"+lblToplamTutar.Text+"TL", AltBaslik, sb, 150, 300 + 30 * (lvUrunler.Items.Count+3), st);
            e.Graphics.DrawString("Ödeyeceğiniz Tutar:-----"+lblOdenecek.Text+"TL", AltBaslik, sb, 150, 300 + 30 * (lvUrunler.Items.Count+4), st);
        }

    }
}
