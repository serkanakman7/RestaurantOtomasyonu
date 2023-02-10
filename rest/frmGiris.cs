using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rest
{
    public partial class FrmGiris : Form
    {
        cGenel gnl = new cGenel();
        public FrmGiris()
        {
            InitializeComponent();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            cPersoneller p = new cPersoneller();

            bool result=p.personelEntryControl(txtSifre.Text, cGenel._PersonelId);

            if (result)
            {
                cPersonelHareketleri ch = new cPersonelHareketleri();
                ch.PersonelId = cGenel._PersonelId;
                ch.Islem = "Giris Yaptı";
                ch.Tarih = DateTime.Now;
                ch.PersonelActionSave(ch);


                this.Hide();                                  //formu gizler
                frmMenu menu = new frmMenu();
                menu.Show();                                  //form açar
            }
        }

        private void FrmGiris_Load(object sender, EventArgs e)
        {
            cPersoneller p = new cPersoneller();
            p.personelGetByInformetion(cbKullanici);
        }

        private void cbKullanici_SelectedIndexChanged(object sender, EventArgs e)
        {
            cPersoneller p = (cPersoneller)cbKullanici.SelectedItem;
            cGenel._PersonelId = p.PersonelId;
            cGenel._PersonelGorevId = p.PersonelGorevId;
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Çıkmak İstediğinize Emin Misiniz", "Uyarı!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
