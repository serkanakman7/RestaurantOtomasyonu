using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    class cAdisyon
    {

        cGenel gnl = new cGenel();

        #region Fields
        private int _Id;
        private int _ServisTurNo;
        private int _PersonelId;
        private decimal _Tutar;
        private DateTime _Tarih;
        private int _MasaId;
        private int _Durum;
        #endregion
        #region Properites
        public int Id { get => _Id; set => _Id = value; }
        public int ServisTurNo { get => _ServisTurNo; set => _ServisTurNo = value; }
        public int PersonelId { get => _PersonelId; set => _PersonelId = value; }
        public decimal Tutar { get => _Tutar; set => _Tutar = value; }
        public DateTime Tarih { get => _Tarih; set => _Tarih = value; }
        public int MasaId { get => _MasaId; set => _MasaId = value; }
        public int Durum { get => _Durum; set => _Durum = value; } 
        #endregion

        public int GetByAddition(int MasaId)
        {

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select top 1 Id from Adisyonlar where MasaId=@masaId Order by Id desc", con);

            cmd.Parameters.Add("@masaId", SqlDbType.Int).Value = MasaId;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                MasaId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
            }
            finally
            {
                con.Close();
            }
            return MasaId;
        }

        public bool SetByAdditionNew(cAdisyon Bilgiler)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Insert Into Adisyonlar(ServisTurNo,Tarih,PersonelId,MasaId,Durum) Values(@ServisTurNo,@Tarih,@PersonelId,@MasaId,@Durum)", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@ServisTurNo", SqlDbType.Int).Value = Bilgiler._ServisTurNo;
                cmd.Parameters.Add("@Tarih", SqlDbType.DateTime).Value = Bilgiler._Tarih;
                cmd.Parameters.Add("@PersonelId", SqlDbType.Int).Value = Bilgiler._PersonelId;
                cmd.Parameters.Add("@MasaId",SqlDbType.Int).Value = Bilgiler._MasaId;
                cmd.Parameters.Add("@Durum", SqlDbType.Bit).Value = Bilgiler._Durum;

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

        public void AdditionClose(int adisyonId,int durum)
        {
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update Adisyonlar Set Durum=@durum where Id=@adisyonId", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Parameters.Add("@durum", SqlDbType.Int).Value = durum;
                cmd.Parameters.Add("@adisyonId", SqlDbType.Int).Value = adisyonId;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
        }

        public int PaketAdisyonIdBulAdedi()
        {
            int miktar = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Count(*) as Sayi From Adisyonlar Where Durum=0 and ServisTurNo=2", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                miktar = Convert.ToInt32(cmd.ExecuteScalar());
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

            return miktar;
        }

        public void AcikPaketAdisyonlar(ListView lv)
        {

            lv.Items.Clear();
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select PaketSiparis.MusteriId , (Musteriler.Ad + ' ' + Musteriler.Soyad ) as Musteri , Adisyonlar.Id From PaketSiparis Inner Join Musteriler on PaketSiparis.MusteriId=Musteriler.Id Inner Join Adisyonlar on PaketSiparis.AdisyonId=Adisyonlar.Id where Adisyonlar.Durum=0", con);

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
                    lv.Items.Add(dr["MusteriId"].ToString());
                    lv.Items[sayac].SubItems.Add(dr["Musteri"].ToString());
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

        public int MusterininSonAdisyonId(int MusteriId)
        {
            int result = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Adisyonlar.Id From PaketSiparis Inner Join Adisyonlar on Paketsiparis.AdisyonId = Adisyonlar.Id Where PaketSiparis.Durum = 0 and Adisyonlar.Durum = 0 and PaketSiparis.MusteriId = @MusteriId",con);

            cmd.Parameters.Add("@MusteriId", SqlDbType.Int).Value = MusteriId;

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                }

                result = Convert.ToInt32(cmd.ExecuteScalar());
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

        public void MusteriDetaylar(ListView lv,int musteriId)
        {

            lv.Items.Clear();
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select PaketSiparis.MusteriId , Musteriler.Ad , Musteriler.Soyad , convert(varchar,adisyonlar.Tarih,104) as Tarih , PaketSiparis.AdisyonId From Adisyonlar Inner Join PaketSiparis on PaketSiparis.AdisyonId=Adisyonlar.Id Inner Join Musteriler on PaketSiparis.MusteriId = Musteriler.Id Where (Adisyonlar.ServisTurNo = 2) and (Adisyonlar.Durum = 0) and (Adisyonlar.Durum = 0) and (PaketSiparis.MusteriId = @MusteriId)", con);

            cmd.Parameters.Add("@MusteriId", SqlDbType.Int).Value = musteriId;

            SqlDataReader dr = null;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            try
            {

                int sayac = 0;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                   lv.Items.Add(dr["MusteriId"].ToString());
                   lv.Items[sayac].SubItems.Add(dr["Ad"].ToString());
                   lv.Items[sayac].SubItems.Add(dr["Soyad"].ToString());
                   lv.Items[sayac].SubItems.Add(dr["Tarih"].ToString());
                   lv.Items[sayac].SubItems.Add(dr["AdisyonId"].ToString());

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

        public int RezervasyonAdisyonAc(cAdisyon Bilgiler)
        {
            int result = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Insert Into Adisyonlar(ServisTurNo,Tarih,PersonelId,MasaId) Values(@ServisTurNo,@Tarih,@PersonelId,@MasaId); Select SCOPE_IDENTITY()", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@ServisTurNo", SqlDbType.Int).Value = Bilgiler._ServisTurNo;
                cmd.Parameters.Add("@Tarih", SqlDbType.DateTime).Value = Bilgiler._Tarih;
                cmd.Parameters.Add("@PersonelId", SqlDbType.Int).Value = Bilgiler._PersonelId;
                cmd.Parameters.Add("@MasaId", SqlDbType.Int).Value = Bilgiler._MasaId;

                result = Convert.ToInt32(cmd.ExecuteScalar());

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

    }
}
