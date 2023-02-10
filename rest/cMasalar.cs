using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace rest
{
    class cMasalar
    {
        cGenel gnl = new cGenel();

        #region Fields
        private int _Id;
        private int _Kapasite;
        private int _ServisTuru;
        private int _Durum;
        private bool _Onay;
        private string _MasaBilgi;
        #endregion
        #region ProPerties
        public int Id { get => _Id; set => _Id = value; }
        public int Kapasite { get => _Kapasite; set => _Kapasite = value; }
        public int ServisTuru { get => _ServisTuru; set => _ServisTuru = value; }
        public int Durum { get => _Durum; set => _Durum = value; }
        public bool Onay { get => _Onay; set => _Onay = value; }
        public string MasaBilgi { get => _MasaBilgi; set => _MasaBilgi = value; }
        #endregion

        public string SessionSum(int state,string masaId)
        {
            string dt = "";
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Tarih,MasaId from Adisyonlar right join Masalar on Adisyonlar.MasaId=Masalar.Id where Adisyonlar.Durum=0 and Masalar.Durum=@durum and Masalar.Id=@MasaId",con);
            SqlDataReader dr = null;
            cmd.Parameters.Add("@durum",SqlDbType.Int).Value = state;
            cmd.Parameters.Add("@MasaId", SqlDbType.Int).Value = Convert.ToInt32(masaId);

            try
            {
                if (con.State==ConnectionState.Closed) {

                    con.Open();
                }
                dr = cmd.ExecuteReader();
                while (dr.Read()) {
                    dt = Convert.ToDateTime(dr["Tarih"]).ToString();
                }
            }
            catch(SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }
            finally
            {
                dr.Close();
                con.Dispose();
                con.Close();
            }
            return dt;
        }

        public int TableGetByNumber(string TableValue)
        {
            string aa = TableValue;
            int length = aa.Length;

            if (length > 8)
            {
                return Convert.ToInt32(aa.Substring(length - 2, 2));
            }
            else
            {
                return Convert.ToInt32(aa.Substring(length - 1, 1));
            }
        }

        public bool TableGetByState(int buttonName,int state)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select Durum from Masalar where Id=@tableId and Durum=@state", con);

            cmd.Parameters.Add("@tableId", SqlDbType.Int).Value = buttonName;
            cmd.Parameters.Add("@state", SqlDbType.Int).Value = state;

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
                string hata = ex.Message;
            }
            finally
            {
                con.Dispose();
                con.Close();

            }
            return result;
        }

        public void SetChangeTableState(string ButtonName,int state)
        {
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Update Masalar Set Durum=@durum where Id=@masaId", con);
            string masaNo = "";

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            string aa = ButtonName;
            int length = aa.Length;

            if (length > 8)
            {
                masaNo = aa.Substring(length - 2, 2);
            }
            else
            {
                masaNo = aa.Substring(length - 1, 1);
            }

            cmd.Parameters.Add("@masaId", SqlDbType.Int).Value = masaNo;
            cmd.Parameters.Add("@durum", SqlDbType.Int).Value = state;
            cmd.ExecuteNonQuery();

            con.Dispose();
            con.Close();
        }

        public void MasaKapasitesiveDurumuGetir(ComboBox cb)
        {
            cb.Items.Clear();
            string durum = "";
            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("Select * From Masalar", con);

            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                cMasalar c = new cMasalar();
                if (c._Durum == 2)
                    durum = "DOLU";
                if (c._Durum == 3)
                    durum = "REZERVE";

                c._Kapasite = Convert.ToInt32(dr["Kapasite"]);
                c._MasaBilgi = "Masa No :" + dr["Id"].ToString() + "Kapasitesi" + dr["Kapasite"].ToString();
                c._Id = Convert.ToInt32(dr["Id"]);
                cb.Items.Add(c);
                         
            }
            dr.Close();
            con.Dispose();
            con.Close();
        }

        public override string ToString()
        {
            return _MasaBilgi;
        }
    }
}
