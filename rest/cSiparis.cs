using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    class cSiparis
    {

        cGenel gnl = new cGenel();

        #region Fields
        private int _Id;
        private int _AdisyonId;
        private int _UrunId;
        private int _MasaId;
        private int _Adet;
        #endregion
        #region Properties
        public int Id { get => _Id; set => _Id = value; }
        public int AdisyonId { get => _AdisyonId; set => _AdisyonId = value; }
        public int UrunId { get => _UrunId; set => _UrunId = value; }
        public int MasaId { get => _MasaId; set => _MasaId = value; }
        public int Adet { get => _Adet; set => _Adet = value; } 
        #endregion

        public void GetByOrder(ListView lv ,int AdisyonId)
        {
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select UrunAd,Fiyat,Satislar.Id,UrunId,Satislar.Adet from Satislar Inner Join Urunler on Satislar.UrunId=Urunler.Id where AdisyonId=@adisyonId", con);

            

            cmd.Parameters.Add("@adisyonId", SqlDbType.Int).Value = AdisyonId;
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
                    lv.Items.Add(dr["UrunAd"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Adet"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["UrunId"].ToString());
                    lv.Items[sayac].SubItems.Add(Convert.ToString(Convert.ToDecimal(dr["Fiyat"]) * Convert.ToDecimal(dr["Adet"])));
                    lv.Items[sayac].SubItems.Add(dr["Id"].ToString());

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

        public bool SetByOrder(cSiparis Bilgiler)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Insert Into Satislar(AdisyonId,UrunId,Adet,MasaId) Values (@AdisyonId,@UrunId,@Adet,@MasaId)", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@AdisyonId", SqlDbType.Int).Value = Bilgiler._AdisyonId;
                cmd.Parameters.Add("@UrunId", SqlDbType.Int).Value = Bilgiler._UrunId;
                cmd.Parameters.Add("@Adet", SqlDbType.Int).Value = Bilgiler._Adet;
                cmd.Parameters.Add("@MasaId", SqlDbType.Int).Value = Bilgiler._MasaId;

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

        public void SetDeleteOrder(int satisId)
        {
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Delete from Satislar where Id=@SatisId", con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            cmd.Parameters.Add("@SatisId", SqlDbType.Int).Value = satisId;
            cmd.ExecuteNonQuery();

            con.Dispose();
            con.Close();
        }

        public decimal GenelToplamBul(int MusteriId)
        {
            decimal genelToplam = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select SUM(Satislar.Adet * Urunler.Fiyat) as Fiyat From Satislar Inner Join Urunler on Satislar.UrunId=Urunler.Id Inner Join Adisyonlar on Adisyonlar.Id = Satislar.AdisyonId Inner Join PaketSiparis on PaketSiparis.AdisyonId = Adisyonlar.Id WHERE(PaketSiparis.MusteriId = @MusteriId) AND(PaketSiparis.Durum = 0)", con);

            cmd.Parameters.Add("@MusteriId", SqlDbType.Int).Value = MusteriId;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                genelToplam = Convert.ToDecimal(cmd.ExecuteScalar());
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
            return genelToplam;
        }

        public void AdisyonPaketSiparisDetaylari(ListView lv , int adisyonId)
        {
            lv.Items.Clear();
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Satislar.Id as SatisId,Urunler.UrunAd,Urunler.Fiyat,Satislar.Adet From Satislar Inner Join Adisyonlar on Satislar.AdisyonId=Adisyonlar.Id Inner Join Urunler on Satislar.UrunId=Urunler.Id Where Satislar.AdisyonId=@AdisyonId", con);

            cmd.Parameters.Add("@AdisyonId", SqlDbType.Int).Value = adisyonId;

            SqlDataReader dr = null;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                int i = 0;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lv.Items.Add(dr["SatisId"].ToString());
                    lv.Items[i].SubItems.Add(dr["UrunAd"].ToString());
                    lv.Items[i].SubItems.Add(dr["Adet"].ToString());
                    lv.Items[i].SubItems.Add(dr["Fiyat"].ToString());

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

    }
}
