using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    class cRezervasyon
    {
        cGenel gnl = new cGenel();

        #region Fields
        private int _Id;
        private int _TableId;
        private int _ClientId;
        private DateTime _Date;
        private int _ClientCount;
        private string _Description;
        private int _AdditionId;
        #endregion
        #region Properties
        public int Id { get => _Id; set => _Id = value; }
        public int TableId { get => _TableId; set => _TableId = value; }
        public int ClientId { get => _ClientId; set => _ClientId = value; }
        public DateTime Date { get => _Date; set => _Date = value; }
        public int ClientCount { get => _ClientCount; set => _ClientCount = value; }
        public string Description { get => _Description; set => _Description = value; }
        public int AdditionId { get => _AdditionId; set => _AdditionId = value; } 
        #endregion
        
        //Müşteri Id Masa numarasına Göre
        public int GetByClientIdFromRezervasyon(int tableId)
        {
            int clientId = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select top 1 MusteriId from Rezervasyonlar where MasaId=@TableId order by MusteriId Desc",con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Parameters.Add("@TableId", SqlDbType.Int).Value = tableId;
                clientId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
            return clientId;
        }

        //Hesabı Kapatrıken Rezervasyonlu Masayı Kapattık
        public bool RezervationClose(int adisyonId)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update Rezervasyonlar Set Durum=1 where AdisyonId=@AdisyonId", con);

            try
            {
                if (con.State == ConnectionState.Closed) 
                {
                    con.Open();
                }

                cmd.Parameters.Add("@AdisyonId", SqlDbType.Int).Value = adisyonId;

                result = Convert.ToBoolean(cmd.ExecuteNonQuery());
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }
            return result;
        }

        //Rezervasyonları Getir
        public  void MusteriIdGetirFromRezervasyon(ListView lv)
        {
            lv.Items.Clear();
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Rezervasyonlar.MusteriId , (Ad + ' ' + Soyad) as Musteri From Rezervasyonlar Inner Join Musteriler On Rezervasyonlar.MusteriId=Musteriler.Id Where Rezervasyonlar.Durum==", con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlDataReader dr = cmd.ExecuteReader();

            int i = 0;
            while (dr.Read())
            {
                lv.Items.Add(dr["MusteriId"].ToString());
                lv.Items[i].SubItems.Add(dr["Musteri"].ToString());
                i++;
            }
            dr.Close();
            con.Dispose();
            con.Close();
        }

        public void EskiRezervasyonlariGetir(ListView lv , int mId)
        {
            lv.Items.Clear();
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Rezervasyonlar.MusteriId , Ad , Soyad , Tarih , AdisyonId From Rezervasyonlar Inner Join Musteriler On Rezervasyonlar.MusteriId = Musteriler.Id Where Rezervasyonlar.musteriId=@mId and Rezervasyonlar.Durum Order By Rezervasyonlar.Id Desc", con);

            cmd.Parameters.Add("@mId", SqlDbType.Int).Value = mId;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlDataReader dr = cmd.ExecuteReader();

            int i = 0;
            while (dr.Read())
            {
                lv.Items.Add(dr["MusteriId"].ToString());
                lv.Items[i].SubItems.Add(dr["Ad"].ToString());
                lv.Items[i].SubItems.Add(dr["Soyad"].ToString());
                lv.Items[i].SubItems.Add(dr["Tarih"].ToString());
                lv.Items[i].SubItems.Add(dr["AdisyonId"].ToString());
                i++;
            }
            dr.Close();
            con.Dispose();
            con.Close();
        }

        //En Son Rezervasyon Tarihini Getir
        public DateTime EnSonRezervasyon(int mId)
        {
            DateTime tar = DateTime.Now;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Tarih From Rezervasyonlar Where MusteriId=@mId and Durum=1 Order By Id Desc", con);

            cmd.Parameters.Add("@mId", SqlDbType.Int).Value = mId;

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            tar = Convert.ToDateTime(cmd.ExecuteScalar());

            con.Dispose();
            con.Close();
            return tar;
        }

        public int AcikRezervasyonSayisi()
        {
            int result = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Count(*) From Rezervasyonlar Where Durum = 0", con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            try
            {
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
            }
            con.Dispose();
            con.Close();

            return result;
        }

        //Rezervasyon Açık mı Kontrolü
        public bool RezervasyonAcikMiKontrol(int mId)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Top 1 Id From Rezervasyonlar  Where MusteriId=@mId and Durum=1 Order By Id Desc", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@mId", SqlDbType.Int).Value = mId;
                result = Convert.ToBoolean(cmd.ExecuteScalar());
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

        public bool RezervasyonAc(cRezervasyon r)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Insert Into Rezervasyonlar(MusteriId,MasaId,AdisyonId,KisiSayisi,Tarih,Aciklama,Durum)Values(@MusteriId,@MasaId,@AdisyonId,@KisiSayisi,@Tarih,@Aciklama,1)", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@MusteriId", SqlDbType.Int).Value = r._ClientId;
                cmd.Parameters.Add("@MasaId", SqlDbType.Int).Value = r._TableId;
                cmd.Parameters.Add("@AdisyonId", SqlDbType.Int).Value = r._AdditionId;
                cmd.Parameters.Add("@KisiSayisi", SqlDbType.Int).Value = r._ClientCount;
                cmd.Parameters.Add("@Tarih", SqlDbType.Date).Value = r._Date;
                cmd.Parameters.Add("@Aciklama", SqlDbType.NVarChar).Value = r._Description;

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

        //Rezerve Masanın Id sini Getir
        public int RezerveMasaIdGetir(int mId)
        {
            int result = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Rezervasyonlar.MasaId From Rezervasyonlar Inner Join Adisyonlar On Rezervasyonlar.AdisyonId = Adisyonlar.Id Where (Rezervasyonlar.Durum = 1) and (Adisyonlar.Durum=0) and (Rezervasyonlar.MusteriId = @mId)", con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            try
            {
                cmd.Parameters.Add("@mId", SqlDbType.Int).Value = mId;
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
            }
            con.Dispose();
            con.Close();

            return result;
        }


    }
}
