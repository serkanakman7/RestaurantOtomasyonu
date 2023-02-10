using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    public class cPersoneller
    {

        cGenel gnl = new cGenel();

        #region Fields                 
        private int _PersonelId;
        private int _PersonelGorevId;
        private string _PersonelAd;                                                  
        private string _PersonelSoyad;                                              //Alan
        private string _PersonelParola;
        private string _PersonelKullaniciAdi;
        private bool _PersonelDurum;
        #endregion                                                                                                    
        #region Properties                                                  
        public int PersonelId                                                  //özellikler
        {
            get { return _PersonelId; }
            set { _PersonelId = value; }
        }
        public int PersonelGorevId
        {
            get { return _PersonelGorevId; }
            set { _PersonelGorevId = value; }
        }
        public string PersonelAd
        {
            get { return _PersonelAd; }
            set { _PersonelAd = value; }
        }
        public string PersonelSoyad
        {
            get { return _PersonelSoyad; }
            set { _PersonelSoyad = value; }
        }
        public string PersonelParola
        {
            get { return _PersonelParola; }
            set { _PersonelParola = value; }
        }
        public string PersonelKullaniciAdi
        {
            get { return _PersonelKullaniciAdi; }
            set { _PersonelKullaniciAdi = value; }
        }
        public bool PersonelDurum
        {
            get { return _PersonelDurum; }
            set { _PersonelDurum = value; }
        }
        #endregion


        public bool personelEntryControl(string password, int userId)
        {

            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * from Personeller where Parola=@password and Id=@userId", con);
            cmd.Parameters.Add("@userId", SqlDbType.VarChar).Value = userId;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                result = Convert.ToBoolean(cmd.ExecuteScalar());
            }
            catch(SqlException ex)
            {
                string hata=ex.Message;
                throw;

            }

            return result;
        } 

        public void personelGetByInformetion(ComboBox cb)
        {

            cb.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * from Personeller where Durum = 0", con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                cPersoneller p = new cPersoneller();
                p._PersonelId = Convert.ToInt32(dr["Id"]);
                p._PersonelGorevId = Convert.ToInt32(dr["GorevId"]);
                p._PersonelAd = Convert.ToString(dr["Ad"]);
                p._PersonelSoyad = Convert.ToString(dr["Soyad"]);
                p._PersonelParola = Convert.ToString(dr["Parola"]);
                p._PersonelKullaniciAdi = Convert.ToString(dr["KullaniciAdi"]);
                p._PersonelDurum = Convert.ToBoolean(dr["Durum"]);
                cb.Items.Add(p);
              }
            dr.Close();
            con.Close();
        }

        public override string ToString() {

            return PersonelAd + " " + PersonelSoyad;
        }

        public void PersonelBilgileriniGetir(ListView lv)
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Personeller.*,PersonelGorevleri.Gorev From Personeller Inner Join PersonelGorevleri on Personeller.GorevId=PersonelGorevleri.Id Where Personeller.Durum = 0", con);

            SqlDataReader dr = null;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                dr = cmd.ExecuteReader();

                int i = 0;

                while (dr.Read())
                {

                    lv.Items.Add(dr["Id"].ToString());
                    lv.Items[i].SubItems.Add(dr["GorevId"].ToString());
                    lv.Items[i].SubItems.Add(dr["Gorev"].ToString());
                    lv.Items[i].SubItems.Add(dr["Ad"].ToString());
                    lv.Items[i].SubItems.Add(dr["Soyad"].ToString());
                    i++;
                }
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                dr.Close();
                con.Dispose();
                con.Close();
            }

        }
        public void PersonelBilgileriniGetirLv(ListView lv,int Id)
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Personeller.*,PersonelGorevleri.Gorev From Personeller Inner Join PersonelGorevleri on Personeller.GorevId=PersonelGorevleri.Id Where Personeller.Durum = 0 and Personeller.Id=@id", con);

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;

            SqlDataReader dr = null;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                dr = cmd.ExecuteReader();

                int i = 0;

                while (dr.Read())
                {

                    lv.Items.Add(dr["Id"].ToString());
                    lv.Items[i].SubItems.Add(dr["GorevId"].ToString());
                    lv.Items[i].SubItems.Add(dr["Gorev"].ToString());
                    lv.Items[i].SubItems.Add(dr["Ad"].ToString());
                    lv.Items[i].SubItems.Add(dr["Soyad"].ToString());
                    i++;
                }
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                dr.Close();
                con.Dispose();
                con.Close();
            }

        }

        public string PersonelBilgiGetirIsim(int Id)
        {
            string result = "";

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Ad + ' ' + Soyad From Personeller Where Id=@id", con);

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id; try
            {
                if (con.State == ConnectionState.Closed) 
                {
                    con.Open();
                }
                result = cmd.ExecuteScalar().ToString();
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
            return result;
        }

        public bool PersonelSifreDegistir(int personelId,string pass)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update Personeller set Parola = @pass Where Id=@Id", con);

            cmd.Parameters.Add("@pass", SqlDbType.VarChar).Value = pass;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = personelId;

            try{
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                result = Convert.ToBoolean(cmd.ExecuteNonQuery());
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
            return result;
        }

        public bool PersonelEkle(cPersoneller cp)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Insert Into Personeller (Ad,Soyad,Parola,GorevId) Values (@Ad,@Soyad,@Parola,@GorevId)", con);

            cmd.Parameters.Add("@Ad", SqlDbType.VarChar).Value = cp._PersonelAd;
            cmd.Parameters.Add("@Soyad", SqlDbType.VarChar).Value = cp._PersonelSoyad;
            cmd.Parameters.Add("@Parola", SqlDbType.VarChar).Value = cp._PersonelParola;
            cmd.Parameters.Add("@GorevId", SqlDbType.Int).Value = cp._PersonelGorevId;

            try{
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                result = Convert.ToBoolean(cmd.ExecuteNonQuery());
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }

            return result;
        }

        public bool PersonelGuncelle(cPersoneller cp,int perId)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update Personeller Set Ad=@Ad , Soyad=@Soyad , Parola=@Parola , GorevId=@GorevId where Id=@perId ", con);

            cmd.Parameters.Add("@perId", SqlDbType.Int).Value = perId;
            cmd.Parameters.Add("@Ad", SqlDbType.VarChar).Value = cp._PersonelAd;
            cmd.Parameters.Add("@Soyad", SqlDbType.VarChar).Value = cp._PersonelSoyad;
            cmd.Parameters.Add("@Parola", SqlDbType.VarChar).Value = cp._PersonelParola;
            cmd.Parameters.Add("@GorevId", SqlDbType.Int).Value = cp._PersonelGorevId;


            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                result = Convert.ToBoolean(cmd.ExecuteNonQuery());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }

            return result;
        }

        public bool PersonelSil(int perId)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update Personeller set Durum = 1 Where Id=@perId", con);

            cmd.Parameters.Add("perId", SqlDbType.Int).Value = perId;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                result = Convert.ToBoolean(cmd.ExecuteNonQuery());
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
            return result;
        }
    }
}
