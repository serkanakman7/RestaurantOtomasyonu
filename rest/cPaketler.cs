using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;


namespace rest
{
    class cPaketler
    {
        cGenel gnl = new cGenel();

        #region Fields
        private int _Id;
        private int _AdditionId;
        private int _ClientId;
        private int _PaytypeId;
        private string _Description;
        private int _State;
        #endregion
        #region Properties
        public int Id { get => _Id; set => _Id = value; }
        public int AdditionId { get => _AdditionId; set => _AdditionId = value; }
        public int ClientId { get => _ClientId; set => _ClientId = value; }
        public int PaytypeId { get => _PaytypeId; set => _PaytypeId = value; }
        public string Description { get => _Description; set => _Description = value; }
        public int State { get => _State; set => _State = value; } 
        #endregion

        // Paket Servis Açma 
        public bool OrderServiceOpen(cPaketler order)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Insert Into PaketSiparis (AdisyonId,MusteriId,OdemeTurId,Aciklama Values (@AdisyonId,@MusteriId,@OdemeTuruId,@Aciklama", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@AdisyonId", SqlDbType.Int).Value = order._AdditionId;
                cmd.Parameters.Add("@MusteriId", SqlDbType.Int).Value = order._ClientId;
                cmd.Parameters.Add("@OdemeTurId", SqlDbType.Int).Value = order._PaytypeId;
                cmd.Parameters.Add("@Aciklama", SqlDbType.NVarChar).Value = order._Description;

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

        //Paket Servis Kapatma  
        public void OrderServiceClose(int AdditionId)
        {
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update PaketSiparis set PaketSiparis.Durum=1 where PaketSiparis.AdisyonId=@AdditionId", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@AdditionId", SqlDbType.Int).Value =AdditionId;


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

        //Açılan Adisyon ve Paket Sipariş Ait Ön Girilen Ödeme Tur Id
        public int OdemeTurIdGetir(int AdisyonId)
        {
            int odemeTurId = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select PaketSiparis.OdemeTurId from PaketSiparis Inner Join Adisyonlar on PaketSiparis.AdisyonId=Adisyonlar.Id where Adisyonlar.Id=@AdisyonId", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@AdisyonId", SqlDbType.Int).Value = AdisyonId;


                odemeTurId=Convert.ToInt32(cmd.ExecuteScalar());
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

            return odemeTurId;
        }

        //Bir Müşteriye Ait 2 Tane Siparişin Açık Olmayacağını Belirtiyoruz
        public int MusteriSonAdisyonIdGetir(int musteriId)
        {
            int no = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Adisyonlar.Id from Adisyonlar Inner Join PaketSiparis on " +
                "PaketSiparis.AdisyonId=Adisyonlar.Id where (Adisyonlar.Durum=0) and (PaketSiparis.Durum=0) and PaketSiparis.MusteriId=@MusteriId", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@MusteriId", SqlDbType.Int).Value = musteriId;


                no = Convert.ToInt32(cmd.ExecuteScalar());
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

            return no;
        }

        //Müşteri Arama Ekranında Adisyon Bul Butonu Adisyon Açık Mı Kapalı Mı Kontrol.
        public bool GetCheckOpenAdditionId(int AdditionId)
        {
            bool result = false;
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * from Adisyonlar where (Durum=0),(Id=@AdditionId)", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@AdditionId", SqlDbType.Int).Value =AdditionId;

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
    }
}
