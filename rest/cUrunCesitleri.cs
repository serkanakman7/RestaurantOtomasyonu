using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text;

namespace rest
{
    class cUrunCesitleri
    {

        cGenel gnl = new cGenel();

        #region Fields
        private int _UrunturNo;
        private string _KategoriAd;
        private string _Aciklama;
        #endregion
        #region Properties
        public int UrunturNo { get => _UrunturNo; set => _UrunturNo = value; }
        public string KategoriAd { get => _KategoriAd; set => _KategoriAd = value; }
        public string Aciklama { get => _Aciklama; set => _Aciklama = value; } 
        #endregion

        public void GetByProductsTypes(ListView Cesitler,Button btn)
        {
            Cesitler.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select UrunAd,Fiyat,Urunler.Id from Kategoriler Inner Join Urunler on Kategoriler.Id=Urunler.KategoriId where Urunler.KategoriId=@KategoriId", con);

            string aa = btn.Name;
            int length = aa.Length;

            cmd.Parameters.Add("@KategoriId", SqlDbType.Int).Value = aa.Substring(length - 1, 1);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlDataReader dr = cmd.ExecuteReader();
            int i = 0;
            while (dr.Read())
            {
                Cesitler.Items.Add(dr["UrunAd"].ToString());
                Cesitler.Items[i].SubItems.Add(dr["Fiyat"].ToString());
                Cesitler.Items[i].SubItems.Add(dr["Id"].ToString());
                i++;
            }
            dr.Close();
            con.Dispose();
            con.Close();
        }

        public void GetByProductSearch(ListView Cesitler,int txt)
        {
            Cesitler.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * from Urunler where Id=@Id", con);

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = txt;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlDataReader dr = cmd.ExecuteReader();
            int i = 0;
            while (dr.Read())
            {
                Cesitler.Items.Add(dr["UrunAd"].ToString());
                Cesitler.Items[i].SubItems.Add(dr["Fiyat"].ToString());
                Cesitler.Items[i].SubItems.Add(dr["Id"].ToString());
                i++;
            }
            dr.Close();
            con.Dispose();
            con.Close();
        }

        //Ürün Çeşitlerini Getir ComboBox
        public void UrunCesitleriniGetir(ComboBox cb)
        {
            cb.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * From Kategoriler Where Durum = 0", con);

            SqlDataReader dr = null;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    cUrunCesitleri uc = new cUrunCesitleri();
                    uc._UrunturNo = Convert.ToInt32(dr["Id"]);
                    uc._KategoriAd = dr["KategoriAdi"].ToString();
                    uc._Aciklama = dr["Aciklama"].ToString();
                    cb.Items.Add(uc);
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

        //Ürün Çeşitlerini Getir ListView
        public void UrunCesitleriniGetir(ListView lv)
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * From Kategoriler Where Durum = 0", con);

            SqlDataReader dr = null;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                dr = cmd.ExecuteReader();
                int sayac = 0;

                while (dr.Read())
                {
                    lv.Items.Add(dr["Id"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["KategoriAdi"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Aciklama"].ToString());

                    sayac++;
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

        //Ürün Çeşitlerini Getir ListView Arama
        public void UrunCesitleriniGetir(ListView lv,string source)
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * From Kategoriler Where Durum = 0 and KategoriAdi like '%' + @source + '%'", con);

            cmd.Parameters.Add("@source", SqlDbType.VarChar).Value = source;

            SqlDataReader dr = null;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                dr = cmd.ExecuteReader();
                int sayac = 0;

                while (dr.Read())
                {
                    lv.Items.Add(dr["Id"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["KategoriAdi"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Aciklama"].ToString());

                    sayac++;
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

        //Ürün Çeşitlerini Getir
        public int UrunKategoriEkle(cUrunCesitleri u)
        {
            int result = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Insert Into Kategoriler(KategoriAdi,Aciklama)Values(@kategoriAdi,@Aciklama)", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@kategoriAdi", SqlDbType.VarChar).Value = u._KategoriAd;
                cmd.Parameters.Add("@Aciklama", SqlDbType.VarChar).Value = u._Aciklama;

                result = Convert.ToInt32(cmd.ExecuteNonQuery());
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

        //Ürün Çeşitlerini Güncelle
        public int UrunKategoriGuncelle(cUrunCesitleri u)
        {
            int result = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update Kategoriler Set KategoriAdi=@kategoriAdi , Aciklama = @aciklama Where Id=@id", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@kategoriAdi", SqlDbType.VarChar).Value = u._KategoriAd;
                cmd.Parameters.Add("@aciklama", SqlDbType.VarChar).Value = u._Aciklama;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = u._UrunturNo;

                result = Convert.ToInt32(cmd.ExecuteNonQuery());
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

        //Ürün Çeşitlerini Sil
        public int UrunKategoriSil(int id)
        {
            int result = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update Kategoriler Set Durum=1 Where Id=@id", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }


                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                result = Convert.ToInt32(cmd.ExecuteNonQuery());
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

        public override string ToString()
        {
            return KategoriAd;
        }
    }
}
