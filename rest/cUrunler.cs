using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    class cUrunler
    {

        cGenel gnl = new cGenel();

        #region Fields
        private int _UrunId;
        private int _UrunTurNo;
        private string _UrunAd;
        private decimal _Fiyat;
        private string _Aciklama;
        #endregion
        #region Properties
        public int UrunId { get => _UrunId; set => _UrunId = value; }
        public int UrunTurNo { get => _UrunTurNo; set => _UrunTurNo = value; }
        public string UrunAd { get => _UrunAd; set => _UrunAd = value; }
        public decimal Fiyat { get => _Fiyat; set => _Fiyat = value; }
        public string Aciklama { get => _Aciklama; set => _Aciklama = value; } 
        #endregion

        //Ürün Adına Göre Listeleme
        public void UrunleriListeleByUrunAdi(ListView lv , string UrunAdi)
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * From Urunler Where Durum = 0 and UrunAd like '%' + @UrunAdi + '%'", con);

            SqlDataReader dr = null;

            cmd.Parameters.Add("@UrunAdi", SqlDbType.VarChar).Value = UrunAdi;

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
                    lv.Items[sayac].SubItems.Add(dr["KategoriId"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["UrunAd"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Aciklama"].ToString());
                    lv.Items[sayac].SubItems.Add(string.Format("{0:0#00.0}", dr["Fiyat"].ToString()));
                    sayac++;
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

        //Ürün Ekle
        public int UrunEkle(cUrunler u)
        {
            int result = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Insert Into Urunler (UrunAd,KategoriId,Aciklama,Fiyat) Values (@urunAd , @katId , @aciklama , @fiyat )", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@urunAd", SqlDbType.VarChar).Value = u._UrunAd;
                cmd.Parameters.Add("@katId", SqlDbType.Int).Value = u._UrunTurNo;
                cmd.Parameters.Add("@aciklama", SqlDbType.VarChar).Value = u._Aciklama;
                cmd.Parameters.Add("@fiyat", SqlDbType.Money).Value = u._Fiyat;

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

        //Ürünler ve Kategorileri Güncelle
        public void UrunleriListele(ListView lv)
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Urunler.*,KategoriAdi From Urunler Inner Join Kategoriler on Kategoriler.Id = Urunler.KategoriId Where Urunler.Durum = 0 ", con);

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
                    lv.Items[sayac].SubItems.Add(dr["KategoriId"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["KategoriAdi"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["UrunAd"].ToString());
                    lv.Items[sayac].SubItems.Add(string.Format("{0:0#00.0}", dr["Fiyat"].ToString()));
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

        //Ürünleri Güncelle
        public int UrunGuncelle(cUrunler u)
        {
            int result = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update Urunler Set UrunAd=@urunAd , KategoriId=@katId , Aciklama=@aciklama , Fiyat=@fiyat Where Id=@id", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Parameters.Add("@urunAd", SqlDbType.VarChar).Value = u._UrunAd;
                cmd.Parameters.Add("@katId", SqlDbType.Int).Value = u._UrunTurNo;
                cmd.Parameters.Add("@aciklama", SqlDbType.VarChar).Value = u._Aciklama;
                cmd.Parameters.Add("@fiyat", SqlDbType.Money).Value = u._Fiyat;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = u._UrunId;

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

        //Ürünleri Sil
        public int UrunSil(int id)
        {
            int result = 0;
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update Urunler Set Durum=1 Where Id = @id", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                result = Convert.ToInt32(cmd.ExecuteNonQuery());
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

        //Ürün Id ye Göre Listeleme
        public void UrunleriListeleByUrunId(ListView lv,int urunId)
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Urunler.*,KategoriAdi From Urunler Inner Join Kategoriler on Kategoriler.Id = Urunler.KategoriId Where Urunler.Durum = 0 and Urunler.KategoriId=@id", con);

            SqlDataReader dr = null;

            cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = urunId;

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
                    lv.Items[sayac].SubItems.Add(dr["KategoriId"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["KategoriAdi"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["UrunAd"].ToString());
                    lv.Items[sayac].SubItems.Add(string.Format("{0:0#00.0}", dr["Fiyat"].ToString()));
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

        //Bütün Ürünleri Getiriyor 2 Tarih Arası
        public void UrunleriListeleIstatistiklereGore(ListView lv ,DateTimePicker Baslangic , DateTimePicker Bitis)
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Urunler.UrunAd , Sum(Satislar.Adet) as adeti From Kategoriler Inner Join Urunler on Kategoriler.Id = Urunler.KategoriId Inner Join Satislar on Urunler.Id = Satislar.UrunId Inner Join Adisyonlar on Satislar.AdisyonId = Adisyonlar.Id Where(Convert(datetime, Tarih, 104) Between Convert(datetime, '@Baslangic', 104) And Convert(datetime, '@Bitis', 104)) Group by Urunler.UrunAd Order by adeti desc", con);

            SqlDataReader dr = null;

            cmd.Parameters.Add("@Baslangic", SqlDbType.VarChar).Value = Baslangic.Value.ToShortDateString();
            cmd.Parameters.Add("@Bitis", SqlDbType.VarChar).Value = Bitis.Value.ToShortDateString();

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

                    lv.Items[sayac].SubItems.Add(dr["UrunAd"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["adeti"].ToString());
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

        //Belli Kategorilere Ait Ürünleri Getiriyor
        public void UrunleriListeleIstatistiklereGoreUrunId(ListView lv,DateTimePicker Baslangic , DateTimePicker Bitis,int urunKatId)
        {
            lv.Items.Clear();

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Urunler.UrunAd , Sum(Satislar.Adet) as adeti From Kategoriler Inner Join Urunler on Kategoriler.Id = Urunler.KategoriId Inner Join Satislar on Urunler.Id = Satislar.UrunId Inner Join Adisyonlar on Satislar.AdisyonId = Adisyonlar.Id Where(Convert(datetime, Tarih, 104) Between Convert(datetime, '@Baslangic', 104) And Convert(datetime, '@Bitis', 104)) and (Urunler.KategoriId=@katId) Group by Urunler.UrunAd Order by adeti desc", con);

            SqlDataReader dr = null;

            cmd.Parameters.Add("@Baslangic", SqlDbType.VarChar).Value = Baslangic.Value.ToShortDateString();
            cmd.Parameters.Add("@Bitis", SqlDbType.VarChar).Value = Bitis.Value.ToShortDateString();
            cmd.Parameters.Add("@katId", SqlDbType.Int).Value = urunKatId;

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

                    lv.Items[sayac].SubItems.Add(dr["UrunAd"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["adeti"].ToString());
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
    }
}
